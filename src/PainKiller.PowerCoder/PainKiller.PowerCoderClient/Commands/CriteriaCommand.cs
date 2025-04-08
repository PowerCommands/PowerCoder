using PainKiller.PowerCoderClient.BaseClasses;
using PainKiller.PowerCoderClient.DomainObjects;

namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Find something in your code using AI LLM model.", 
                        options: [""],
                       examples: ["//Run selected criteria","criteria"])]
public class CriteriaCommand : PowerCodeBaseCommando
{
    public CriteriaCommand(string identifier) : base(identifier)  => EventBusService.Service.Subscribe<WorkingDirectoryChangedEventArgs>(OnWorkingDirectoryChanged);
    
    public override RunResult Run(ICommandLineInput input)
    {
        var config = Configuration.Core.Modules.Ollama;
        var service = OllamaService.GetInstance(config.BaseAddress, config.Port, config.Model);
        var path = input.GetFullPath();
        if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
        {
            Writer.WriteLine("Please provide a valid path to a file or directory.");
            return Nok("Invalid file path.");
        }
        var search = DialogService.ChooseFromOptions("Pick a search criteria!", Configuration.PowerCoder.FindSearchTerms.ToList());
        var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
        Writer.Clear();
        service.Reset();

        var searchPrompt = new CriteriaCodePrompt("search", search, ["namespace MyNamespace;"]);
        Writer.WriteDescription("Criteria", $"{searchPrompt.Criteria} {search}");
        
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var prompt = searchPrompt.GeneratePrompt(file, content);
            service.AddMessage(new ChatMessage("user", prompt));
            var response = service.SendChatToOllama().GetAwaiter().GetResult();
            if(response.Length > 1000) Writer.WriteLine($"{file} = No match");
            else if(response.ToLower().Contains("no match")) Writer.WriteLine(response);
            else Writer.WriteSuccessLine(response);
            service.AddMessage(new ChatMessage("assistant", response));
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