using Microsoft.Extensions.Logging;
using PainKiller.CommandPrompt.CoreLib.Logging.Services;
using PainKiller.CommandPrompt.CoreLib.Modules.ShellModule.Services;
using PainKiller.PowerCoderClient.BaseClasses;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(description: "Run simple git command, push is default, pull is using the --pull option flag.", 
                   options: ["pull"],
                  examples: ["//Add, commit and push current changes","git \"commit comments\"","//Pull changes from git, overwrite local changes","git --pull"])]
public class GitCommand(string identifier) : PowerCodeBaseCommando(identifier)
{
    private readonly ILogger<GitCommand> _logger = LoggerProvider.CreateLogger<GitCommand>();
    public override RunResult Run(ICommandLineInput input)
    {
        if (input.HasOption("pull"))
        {
            var confirm = DialogService.YesNoDialog("Do you really want to pull and discard all local changes? This cannot be undone!");
            if(confirm) Pull();
            return Ok();
        }
        var comment = string.IsNullOrEmpty(input.Quotes.FirstOrDefault()) ? "." : input.Arguments.First();
        Publish(comment);
        return Ok();
    }

    private void Pull()
    {
        RunSingleCommand("reset --hard");
        RunSingleCommand("clean -fd");
        RunSingleCommand("pull --rebase");
        _logger.LogInformation("GIT pull completed, local changes discarded.");
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