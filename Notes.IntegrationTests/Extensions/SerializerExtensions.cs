using Newtonsoft.Json;

namespace Notes.IntegrationTests.Extensions;

public static class SerializerExtensions
{
    public static async Task<TResponse?> ReadAsAsync<TResponse>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();

        var content = JsonConvert.DeserializeObject<TResponse>(responseAsString);

        return content;
    }
}
