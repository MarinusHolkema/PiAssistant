using System.Text;
using System.Text.Json;

namespace PiAssistant.Services;

public class OllamaService
{
    private readonly HttpClient _http;

    public OllamaService(HttpClient http)
    {
        _http = http;
    }

    public async IAsyncEnumerable<string> AskStreamAsync(string prompt)
    {
        var requestObject = new
        {
            model = "qwen2.5-coder:3b",
            prompt = prompt,
            stream = true
        };

        var json = JsonSerializer.Serialize(requestObject);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "http://localhost:11434/api/generate");

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _http.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        using var stream =
            await response.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var document = JsonDocument.Parse(line);

            if (document.RootElement.TryGetProperty(
                "response",
                out var responseText))
            {
                yield return responseText.GetString() ?? "";
            }
        }
    }
}