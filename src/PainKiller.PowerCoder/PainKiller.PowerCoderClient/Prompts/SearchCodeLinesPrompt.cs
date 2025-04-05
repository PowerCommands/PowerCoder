using PainKiller.PowerCoderClient.Contracts;

namespace PainKiller.PowerCoderClient.Prompts;

public class SearchCodeLinesPrompt : IPrompt
{
    public string PromptId => "search";
    public static string Criteria => $"Your task is to find lines of code that contain {{searchTerm}}";
    public string GeneratePrompt(string searchTerm, string fileName, string searchContent)
    {
        return $@"
You are a language model specialized in analyzing code files. Your task is to find lines of code that contain {searchTerm}.

Input: The following message will contain the code to analyze.

Output: If no lines contain {searchTerm}, respond with:
class name = no match

If any lines contain {searchTerm}, respond with a list of matching lines. Format your response as a plain text list, where each line contains the file line number followed by the actual line content. Only include lines that match the given criteria.

Example output (if there are matches):
Find matches in {fileName}
1: [line content containing '{searchTerm}']
5: [line content containing '{searchTerm}']
12: [line content containing '{searchTerm}']

Example output (if there are no matches):
{fileName} = no match

End of instruction now analyze the code below.

{fileName}
{searchContent}";
    }
}
