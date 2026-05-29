namespace PiAssistant.Tools;

public class FileWriteTool : ITool
{
    public string Name => "write_file";

    public string Description =>
        "Writes text to a file";

    public async Task<string> Execute(
        string arguments)
    {
        try
        {
            var parts =
                arguments.Split(
                    '|',
                    2);

            var path = parts[0];

            var content = parts[1];

            await File.WriteAllTextAsync(
                path,
                content);

            return $"File written: {path}";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}