namespace PainKiller.PowerCoderClient.Contracts;

public interface IPrompt
{
    string PromptId { get; }
    string GeneratePrompt(string searchTerm, string fileName, string searchContent);
}