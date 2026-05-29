using PiAssistant.Tools;

namespace PiAssistant.Services;

public class ToolRegistry
{
    private readonly Dictionary<string, ITool>
        _tools = new();

    public ToolRegistry()
    {
        Register(new TimeTool());
        Register(new ShellTool());
        Register(new FileWriteTool());
    }

    public void Register(ITool tool)
    {
        _tools[tool.Name] = tool;
    }

    public ITool? GetTool(string name)
    {
        return _tools.TryGetValue(
            name,
            out var tool)
            ? tool
            : null;
    }

    public IEnumerable<ITool> GetAllTools()
    {
        return _tools.Values;
    }
}