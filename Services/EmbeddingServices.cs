using System.Net.Http.Json;
using System.Text.Json;

namespace PiAssistant.Services;

public class EmbeddingService
{
    private readonly HttpClient _http;

    public EmbeddingService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<float>> GenerateEmbedding(
        string text)
    {
        var response = await _http.PostAsJsonAsync(
            "http://localhost:11434/api/embeddings",
            new
            {
                model = "nomic-embed-text",
                prompt = text
            });

        response.EnsureSuccessStatusCode();

        var json =
            await response.Content.ReadFromJsonAsync<JsonElement>();

        var embedding =
            json.GetProperty("embedding");

        return embedding
            .EnumerateArray()
            .Select(x => x.GetSingle())
            .ToList();
    }
}