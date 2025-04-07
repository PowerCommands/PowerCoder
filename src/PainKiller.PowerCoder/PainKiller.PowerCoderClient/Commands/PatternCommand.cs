using PainKiller.PowerCoderClient.BaseClasses;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Start your coding with this command", 
                        options: [""],
                       examples: ["//Start coding","code"])]
public class PatternCommand : PowerCodeBaseCommando
{
    public PatternCommand(string identifier) : base(identifier)  => EventBusService.Service.Subscribe<WorkingDirectoryChangedEventArgs>(OnWorkingDirectoryChanged);
    
    public override RunResult Run(ICommandLineInput input)
    {
        var path = input.GetFullPath();
        if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
        {
            Writer.WriteLine("Please provide a valid path to a file or directory.");
            return Nok("Invalid file path.");
        }
        var search = DialogService.ChooseFromOptions("Pick a search criteria!", Configuration.PowerCoder.FindPatterns.ToList());
        var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
        Writer.Clear();

        var searchPrompt = new SearchCodePrompt("search", search, ["namespace MyNamespace;"]);
        Writer.WriteDescription("Criteria", $"{searchPrompt.Criteria} {search}");
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var prompt = searchPrompt.GeneratePrompt(file, content);
        }
        while (true)
        {
            Writer.Write("Any further questions?> ");
            var userInput = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(userInput)) continue;
            if (userInput.Trim().Equals("/bye", StringComparison.OrdinalIgnoreCase)) break;
        }
        return Ok();
    }
}