using PainKiller.PowerCoderClient.Contracts;

namespace PainKiller.PowerCoderClient.Prompts;

public class BasicSearchPrompt : IPrompt
{
    public string PromptId => "basic";
    public static string Criteria => $"Your task is to find lines of code that contain {{searchTerm}}";
    public string GeneratePrompt(string searchTerm, string fileName, string searchContent)
    {
        return $@"
You are a language model specialized in analyzing code files. Your task is to find lines of code that contain {searchTerm}.

Output: If no lines contain {searchTerm}, respond with:
class name = no match

If any lines contain {searchTerm}, respond with a list of matching lines.

End of instruction now analyze the code below.

{fileName}
{searchContent}"; }
}