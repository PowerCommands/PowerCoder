using PainKiller.CommandPrompt.CoreLib.Configuration.DomainObjects;
namespace PainKiller.PowerCoderClient.Configuration;
public class CommandPromptConfiguration : ApplicationConfiguration
{
    public PowerCoderConfiguration PowerCoder { get; set; } = new();
}