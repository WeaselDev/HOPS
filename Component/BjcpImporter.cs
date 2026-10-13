using System.Text.Json;

namespace HOPS.Component;

public static class BjcpImporter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<BjcpDocument> LoadAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        await using var stream = File.OpenRead(path);

        var document = await JsonSerializer.DeserializeAsync<BjcpDocument>(
            stream,
            JsonOptions,
            cancellationToken);

        return document
            ?? throw new InvalidDataException("Die BJCP-JSON-Datei ist leer oder ungültig.");
    }
}