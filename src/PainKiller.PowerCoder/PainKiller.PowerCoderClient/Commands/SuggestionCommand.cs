using PainKiller.PowerCoderClient.BaseClasses;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Let AI LLM model look at your code file and give you suggestions.", 
                        options: [""],
                       examples: ["//Show suggestions","suggestion"])]
public class SuggestionCommand : PowerCodeBaseCommando
{
    public SuggestionCommand(string identifier) : base(identifier)  => EventBusService.Service.Subscribe<WorkingDirectoryChangedEventArgs>(OnWorkingDirectoryChanged);
    
    public override RunResult Run(ICommandLineInput input)
    {
        var config = Configuration.Core.Modules.Ollama;
        var service = OllamaService.GetInstance(config.BaseAddress, config.Port, config.Model);
        var path = input.GetFullPath();
        if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path)))
        {
            Writer.WriteLine("Please provide a valid path to a file.");
            return Nok("Invalid file path.");
        }
        var search = $"Do you have any suggestion for improvements?";
        Writer.Clear();
        service.Reset();
        Writer.WriteDescription("Question", $"{path} {search}");
        
        var content = File.ReadAllText(path);
        service.AddMessage(new ChatMessage("user", $"{search}\n{content}"));
        var response = service.SendChatToOllama().GetAwaiter().GetResult();
        service.AddMessage(new ChatMessage("assistant", response));
        Writer.WriteLine(response);
        while (true)
        {
            Writer.Write("Any further questions?> ");
            var userInput = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(userInput)) continue;
            if (userInput.Trim().Equals("/bye", StringComparison.OrdinalIgnoreCase)) break;
            service.AddMessage(new ChatMessage("user", $"{search}\n{content}"));
            service.AddMessage(new ChatMessage("assistant", response));
            Writer.WriteLine(response);
        }
        return Ok();
    }
    protected override void UpdateSuggestions(string newWorkingDirectory)
    {
        if (Directory.Exists(newWorkingDirectory))
        {
            var files = Directory.GetFiles(newWorkingDirectory)
                .Select(f => new FileInfo(f).Name)
                .ToArray();
            SuggestionProviderManager.AppendContextBoundSuggestions(Identifier, files.Select(e => e).ToArray());
        }
    }
}