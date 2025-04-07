using PainKiller.PowerCoderClient.Contracts;

namespace PainKiller.PowerCoderClient.Prompts;

public class SearchCodePrompt(string promptId, string criteria, List<string> examples) : IPrompt
{
    public string PromptId { get; } = promptId; 
    public string Criteria { get; } = criteria;
    public List<string> Examples { get; } = examples;

    public string GeneratePrompt(string fileName, string content)
    {
        var example = string.Join("\n", Examples.Select((x, i) => $"{i + 1}: [line content containing {x}]"));
        return $@"
You are a language model specialized in analyzing code files. Your task is to find lines of code that contain {Criteria}.



Output: If no lines contain {Criteria}, respond with:
{fileName} = no match

If any lines contain {Criteria}, respond with a list of matching lines. Format: [line number]: [line content]

Example output (if there are matches):
Find matches in {fileName}
{example}

Example output (if there are no matches):
{fileName} = no match

Input: This message contains the search content and a file name. The file name is {fileName}. The content of the file is as follows:

{content}";
    }
}
