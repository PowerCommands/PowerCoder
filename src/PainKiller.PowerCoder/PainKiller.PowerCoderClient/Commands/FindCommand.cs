namespace PainKiller.PowerCoderClient.Commands;

[CommandDesign(     description: "Start your coding with this command", 
                        options: [""],
                       examples: ["//Start coding","code"])]
public class FindCommand : ConsoleCommandBase<CommandPromptConfiguration>
{
    public FindCommand(string identifier) : base(identifier)  => EventBusService.Service.Subscribe<WorkingDirectoryChangedEventArgs>(OnWorkingDirectoryChanged);
    public override void OnInitialized()
    {
        Environment.CurrentDirectory = new DirectoryInfo(AppContext.BaseDirectory).GetRoot().FullName;
        EventBusService.Service.Publish(new WorkingDirectoryChangedEventArgs(Environment.CurrentDirectory));
        InfoPanelService.Instance.RegisterContent(new DefaultInfoPanel(new DefaultInfoPanelContent()));
    }
    private void OnWorkingDirectoryChanged(WorkingDirectoryChangedEventArgs e) => UpdateSuggestions(e.NewWorkingDirectory);
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

        Writer.WriteDescription("Criteria", $"{SearchCodePrompt.Criteria} {search}");
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var prompt = new SearchCodePrompt().GeneratePrompt(search, new FileInfo(file).Name, content);
            service.AddMessage(new ChatMessage("user", prompt));
            var response = service.SendChatToOllama().GetAwaiter().GetResult();
            Writer.WriteLine(response);
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
    private void UpdateSuggestions(string newWorkingDirectory)
    {
        if (Directory.Exists(newWorkingDirectory))
        {
            var files = Directory.GetFileSystemEntries(newWorkingDirectory)
                .Select(f => new FileInfo(f).Name)
                .ToArray();
            SuggestionProviderManager.AppendContextBoundSuggestions(Identifier, files.Select(e => e).ToArray());
        }
    }
}