using System.Text.RegularExpressions;
using PainKiller.PowerCoderClient.BaseClasses;
using PainKiller.PowerCoderClient.DomainObjects;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Search for certain pattern.", 
                        options: ["add"],
                       examples: ["//Add a pattern","pattern --add"])]
public class PatternCommand : PowerCodeBaseCommando
{
    public PatternCommand(string identifier) : base(identifier)  => EventBusService.Service.Subscribe<WorkingDirectoryChangedEventArgs>(OnWorkingDirectoryChanged);
    
    public override RunResult Run(ICommandLineInput input)
    {
        if (input.HasOption("add")) return Add();

        var path = input.GetFullPath();
        if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
        {
            Writer.WriteLine("Please provide a valid path to a file or directory.");
            return Nok("Invalid file path.");
        }
        var selectedPattern = DialogService.ChooseFromOptions("Pick a pattern!", CodePatterns.GetItems().Select(p => p.Id).ToList());
        var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
        Writer.Clear();

        var codePattern = CodePatterns.GetItems().First(p => p.Id == selectedPattern);
        Writer.WriteLine(codePattern.Pattern);
        foreach (var file in files)
        {
            var rows = File.ReadAllText(file).Split('\n');
            var patternMatch = false;
            foreach (var row in rows)
            {
                if(string.IsNullOrEmpty(row)) continue;
                if (Regex.IsMatch(row, codePattern.Pattern))
                {
                    patternMatch = true;
                }
            }
            if (!patternMatch == codePattern.AntiPattern) Writer.WriteSuccessLine($"[MATCH   ] {file}");
        }
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