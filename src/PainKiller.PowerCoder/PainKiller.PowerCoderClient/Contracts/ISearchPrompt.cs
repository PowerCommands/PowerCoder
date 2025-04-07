namespace PainKiller.PowerCoderClient.Contracts;

public interface IPrompt
{
    string PromptId { get; }
    string Criteria { get; }
    List<string> Examples { get; }
    string GeneratePrompt(string fileName, string content);
}