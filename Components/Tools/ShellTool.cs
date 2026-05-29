using System.Diagnostics;

namespace PiAssistant.Tools;

public class ShellTool : ITool
{
    public string Name => "shell";

    public string Description =>
        "Executes Linux shell commands";

    public async Task<string> Execute(
        string arguments)
    {
        try
        {
            var process =
                new Process();

            process.StartInfo =
                new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments =
                        $"-c \"{arguments}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

            process.Start();

            var output =
                await process
                    .StandardOutput
                    .ReadToEndAsync();

            var error =
                await process
                    .StandardError
                    .ReadToEndAsync();

            await process.WaitForExitAsync();

            return output + error;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}