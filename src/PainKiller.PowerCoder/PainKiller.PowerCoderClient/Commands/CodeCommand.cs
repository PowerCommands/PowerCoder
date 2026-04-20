using System.Text;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Get all the code, copied to clipboard", 
                    suggestions: ["cs", "js","ts"],
                       examples: ["//Get all the code, copied to clipboard","code"])]
public class CodeCommand(string identifier) : ConsoleCommandBase<CommandPromptConfiguration>(identifier)
{
    public override RunResult Run(ICommandLineInput input)
    {
        var path = input.GetFullPath();
        if(!Directory.Exists(path)) path = Environment.CurrentDirectory;
        var fileType = string.IsNullOrWhiteSpace(input.Arguments.FirstOrDefault()) ? "cs" : input.Arguments.FirstOrDefault();  //default is C#
        if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
        {
            Writer.WriteLine("Please provide a valid path to a file or directory.");
            return Nok("Invalid file path.");
        }
        Writer.Clear();
        Writer.WriteLine($"Iterate all files in directory [{path}]");
        var files = Directory.GetFiles(path, $"*.{fileType}", SearchOption.AllDirectories);
        var contentAllFiles = new StringBuilder();
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            contentAllFiles.AppendLine(content);
            contentAllFiles.AppendLine("");
            Writer.WriteSuccessLine($"[OK] {file}");
        }
        Writer.WriteDescription($"Files of filetype {fileType}", files.Length.ToString());
        Writer.WriteLine("All content copied to clipboard");
        TextCopy.ClipboardService.SetText(contentAllFiles.ToString());
        return Ok();
    }
}