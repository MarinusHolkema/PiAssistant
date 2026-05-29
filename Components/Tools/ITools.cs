namespace PiAssistant.Tools;

public interface ITool
{
    string Name { get; }

    string Description { get; }

    Task<string> Execute(string arguments);
}