namespace PiAssistant.Tools;

public class TimeTool : ITool
{
    public string Name => "time";

    public string Description =>
        "Gets the current system time";

    public Task<string> Execute(
        string arguments)
    {
        return Task.FromResult(
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}