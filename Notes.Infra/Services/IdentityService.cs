using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Notes.Application;
using Notes.Application.Common.Dtos.IdentityDtos;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Notes.Infra.Services;

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _context;

    public IdentityService(IConfiguration configuration, UserManager<AppUser> userManager,
        IAppDbContext context)
    {
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JwtSettings");
        _userManager = userManager;
        _context = context;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto request)
    {
        var appUser = await _userManager.FindByNameAsync(request.Email);

        if (appUser is null || !await _userManager.CheckPasswordAsync(appUser, request.Password))
            return GetResponseIfNotSuccessful();

        var user = await _context.Users.SingleAsync(u => u.AppUserId == appUser.Id);

        var token = await GetTokenAsync(appUser, user);
        await SetRefreshTokenAsync(appUser);

        return await GetResponseIfSuccessfull(appUser, user, token);
    }


    public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
    {
        var appUser = new AppUser { UserName = request.Email, Email = request.Email };

        var result = await _userManager.CreateAsync(appUser, request.Password);

        if (result.Succeeded) GetResponseIfNotSuccessful();

        appUser = await _userManager.FindByEmailAsync(request.Email);

        var user = MapToUser(appUser, request);



        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        var loginRequest = MapToLoginDto(request);

        return await LoginAsync(loginRequest);
    }


    public async Task LogoutAsync(LogoutDto request)
    {
        var user = await _context.Users.FindAsync(request.UserId);

        if (user is null)
            throw new UserNotFoundException(request.UserId);

        var appUser =await _userManager.FindByIdAsync(user.AppUserId);
        appUser.RefreshToken = null;
        appUser.RefreshTokenExpiryTime = null;

       await _userManager.UpdateAsync(appUser);
    }





    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto request)
    {
        if (request is null) return GetResponseIfNotSuccessful();

        var principal = GetPrincipalFromExpiredToken(request.Token);
        var username = principal.Identity?.Name;
        var appUser = await _userManager.FindByEmailAsync(username);

        if (appUser == null || appUser.RefreshToken != request.RefreshToken || appUser.RefreshTokenExpiryTime <= DateTime.Now)
            return GetResponseIfNotSuccessful();

        var user = await _context.Users.SingleAsync(u => u.AppUserId == appUser.Id);

        var token = await GetTokenAsync(appUser, user);
        await SetRefreshTokenAsync(appUser);

        return await GetResponseIfSuccessfull(appUser, user, token);
    }
    private async Task<AuthResponseDto> GetResponseIfSuccessfull(AppUser appUser, User user, string token) => 
        new()
        {
            FirstName = user.FirstName,
            UserId = user.Id,
            IsSuccessful = true,
            Token = token,
            RefreshToken = appUser.RefreshToken,
            RefreshTokenExpiryTime = appUser.RefreshTokenExpiryTime,
            Claims = await GetClaimsAsync(user, appUser)
        };
    private static User MapToUser(AppUser appUser, RegisterDto request) => new()
    {
        AppUserId = appUser.Id,
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName
    };

    private static LoginDto MapToLoginDto(RegisterDto request) => new()
    {
        Email = request.Email,
        Password = request.Password
    };

    private static AuthResponseDto GetResponseIfNotSuccessful() =>
        new() { IsSuccessful = false };

    private async Task SetRefreshTokenAsync(AppUser appUser)
    {
        appUser.RefreshToken = GenerateRefreshToken();
        appUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(appUser);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings["securityKey"])),
            ValidateLifetime = false,
            ValidIssuer = _jwtSettings["validIssuer"],
            ValidAudience = _jwtSettings["validAudience"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
            out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    private async Task<string> GetTokenAsync(AppUser appUser, User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsAsync(user, appUser);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }
    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    private async Task<List<Claim>> GetClaimsAsync(User user, AppUser appUser)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var roles = await _userManager.GetRolesAsync(appUser);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["validIssuer"],
            audience: _jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }

}