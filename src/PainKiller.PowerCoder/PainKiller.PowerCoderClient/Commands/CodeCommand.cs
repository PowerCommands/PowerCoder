using System.Text;
using PainKiller.PowerCoderClient.BaseClasses;
using PainKiller.PowerCoderClient.DomainObjects;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Get all the code, copied to clipboard", 
                        options: [""],
                       examples: ["//Get all the code, copied to clipboard","code"])]
public class CodeCommand : PowerCodeBaseCommando
{
    public CodeCommand(string identifier) : base(identifier)  => EventBusService.Service.Subscribe<WorkingDirectoryChangedEventArgs>(OnWorkingDirectoryChanged);
    
    public override RunResult Run(ICommandLineInput input)
    {
        if (input.HasOption("add")) return Add();

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
    private RunResult Add()
    {
        var id = DialogService.QuestionAnswerDialog("Id of your pattern: ");
        var pattern = DialogService.QuestionAnswerDialog("Pattern: ");
        var isAntiPattern = DialogService.YesNoDialog("Is anti pattern (means match is not success) : ");
        CodePatterns.Insert(new CodePattern { Id = id, Pattern = pattern, AntiPattern = isAntiPattern }, codePattern => codePattern.Id == id);
        return Ok();
    }
}