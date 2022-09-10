using System.Text.Json;

namespace Notes.Infra.Data;

public class SeedHelper
{
    public static List<TEntity>? SeedData<TEntity>(string fileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = "Static/Json";
        var fullPath = Path.Combine(currentDirectory, path, fileName);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var result = new List<TEntity>();
        using (var reader = new StreamReader(fullPath))
        {
            string json = reader.ReadToEnd();
            result = JsonSerializer.Deserialize<List<TEntity>>(json, options);
        }

        return result;
    }
}
