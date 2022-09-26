using Notes.Application.Interfaces;

namespace Notes.IntegrationTests.Mocks;

public class MockLoggedInUser : ILoggedInUserInfo
{
    public string? GetLoggedInUserEmail() => "test@test.com";
}
