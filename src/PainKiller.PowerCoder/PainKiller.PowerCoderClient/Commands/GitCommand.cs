using Microsoft.Extensions.Logging;
using PainKiller.CommandPrompt.CoreLib.Logging.Services;
using PainKiller.CommandPrompt.CoreLib.Modules.ShellModule.Services;
using PainKiller.PowerCoderClient.BaseClasses;

namespace PainKiller.PowerCoderClient.Commands;

public class GitCommand(string identifier) : PowerCodeBaseCommando(identifier)
{
    private readonly ILogger<GitCommand> _logger = LoggerProvider.CreateLogger<GitCommand>();
    public override RunResult Run(ICommandLineInput input)
    {
        var comment = string.IsNullOrEmpty(input.Quotes.FirstOrDefault()) ? "." : input.Arguments.First();
        Publish(comment);
        return Ok();
    }
    private void Publish(string comment)
    {
        if (string.IsNullOrEmpty(comment)) comment = "\"refactoring\"";
        RunSingleCommand("add .");
        RunSingleCommand($"commit -m \"{comment}\"");
        var response = ShellService.Default.StartInteractiveProcess("git", $"push");
        Writer.WriteLine(response);
        _logger.LogInformation($"GIT commit and push with message: {comment}");
    }
    private void RunSingleCommand(string command, string name = "")
    {
        var response = ShellService.Default.StartInteractiveProcess("git", $"{command} {name}");
        Writer.WriteLine(response);
        _logger.LogInformation($"GIT command executed: {command} {name}");
    }
}