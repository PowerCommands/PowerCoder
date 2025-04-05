using PainKiller.PowerCoderClient.Contracts;

namespace PainKiller.PowerCoderClient.Managers;

public class PromptManager
{
    private readonly List<IPrompt> _prompts = [];

    public PromptManager()
    {
        _prompts.Add(new SearchCodeLinesPrompt());
    }
    public IPrompt? GetPrompt(string promptId) => _prompts.FirstOrDefault(p => p.PromptId.Equals(promptId, StringComparison.OrdinalIgnoreCase));
}