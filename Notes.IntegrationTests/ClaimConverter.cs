using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Notes.IntegrationTests;

public class ClaimConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Claim);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        var type = (string)jo["type"]!;
        var value = (string)jo["value"]!;
        var valueType = (string?)jo["valueType"];
        var issuer = (string?)jo["issuer"];
        var originalIssuer = (string?)jo["originalIssuer"];
        return new Claim(type, value, valueType, issuer, originalIssuer);
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
