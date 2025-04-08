using System.Text;
using PainKiller.PowerCoderClient.BaseClasses;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Get all the code, copied to clipboard", 
                        options: [""],
                       examples: ["//Get all the code, copied to clipboard","code"])]
public class CodeCommand : PowerCodeBaseCommando
{
    public CodeCommand(string identifier) : base(identifier)  => EventBusService.Service.Subscribe<WorkingDirectoryChangedEventArgs>(OnWorkingDirectoryChanged);
    
    public override RunResult Run(ICommandLineInput input)
    {
        var path = input.GetFullPath();
        if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
        {
            Writer.WriteLine("Please provide a valid path to a file or directory.");
            return Nok("Invalid file path.");
        }
        Writer.Clear();
        Writer.WriteLine($"Iterate all files in directory [{path}]");
        var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
        var contentAllFiles = new StringBuilder();
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            contentAllFiles.AppendLine(content);
            contentAllFiles.AppendLine("");
            Writer.WriteSuccessLine($"[OK] {file}");
        }
        Writer.WriteDescription("Files", files.Length.ToString());
        Writer.WriteLine("All content copied to clipboard");
        TextCopy.ClipboardService.SetText(contentAllFiles.ToString());
        return Ok();
    }
}