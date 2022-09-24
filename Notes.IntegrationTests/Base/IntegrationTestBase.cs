using Newtonsoft.Json;
using Notes.Application.Common.Dtos.IdentityDtos;
using System.Net.Http.Headers;
using Tynamix.ObjectFiller;

namespace Notes.IntegrationTests.Base;

public class IntegrationTestBase
{
    private static string? _token;

    protected static async Task AuthenticateAsync(HttpClient testClient) =>
        testClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", await GetJwtAsync(testClient));


    private static async Task<AuthResponseDto> RegisterUserAsync(HttpClient testClient)
    {
        var request = new RegisterDto
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName= "Test",
            Password = "TestTest@098" 
        };

        var response = await testClient.PostAsJsonAsync(@"/api/Account/Registration", request);

        var registrationResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<AuthResponseDto>(registrationResponse, new ClaimConverter());
        return result!;
    }

    private static async Task<string> GetJwtAsync(HttpClient testClient)
    {
        _token = _token is null ? 
            (await RegisterUserAsync(testClient)).Token!
            : _token;

        return _token;
    }
    protected T GetRandomObjectAs<T>() where T : class
    {
        var filler = new Filler<T>();

        return filler.Create();
    }
}