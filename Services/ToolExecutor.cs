using System.Text.Json;

namespace PiAssistant.Services;

public class ToolExecutor
{
    private readonly ToolRegistry _registry;

    public ToolExecutor(
        ToolRegistry registry)
    {
        _registry = registry;
    }

    public async Task<string?> TryExecuteTool(
        string response)
    {
        try
        {
            response = response
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();
            var json =
                JsonDocument.Parse(response);

            var root = json.RootElement;

            if (!root.TryGetProperty(
                "tool",
                out var toolName))
            {
                return null;
            }

            var tool =
                _registry.GetTool(
                    toolName.GetString()!);

            if (tool == null)
                return null;

            var arguments =
                root.TryGetProperty(
                    "arguments",
                    out var args)
                    ? args.ToString()
                    : "";

            return await tool.Execute(arguments);
        }
        catch
        {
            return null;
        }
    }
}