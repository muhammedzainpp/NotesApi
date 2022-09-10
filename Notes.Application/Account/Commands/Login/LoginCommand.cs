using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes.Application.Account.Commands.Login;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfigurationSection _jwtSettings;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _configuration = configuration;
        _userManager = userManager;
        _jwtSettings = _configuration.GetSection("JwtSettings");
    }
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return new AuthResponseDto { IsAuthSuccessful = false, Token = string.Empty };

        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new AuthResponseDto { IsAuthSuccessful = true, Token = token };
    }
    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    private List<Claim> GetClaims(IdentityUser user)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email)
    };

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
