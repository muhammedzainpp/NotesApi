using Microsoft.AspNetCore.Http;
using Notes.Application.Interfaces;

namespace Notes.Infra.Services;

public class LoggedInUserInfo : ILoggedInUserInfo
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggedInUserInfo(IHttpContextAccessor httpContextAccessor) => 
        _httpContextAccessor = httpContextAccessor;

    public string? GetLoggedInUserEmail()
    {
        var currentSessionUserEmail = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        return currentSessionUserEmail;
    }
}
