using PainKiller.PowerCoderClient.Data;
using PainKiller.PowerCoderClient.DomainObjects;

namespace PainKiller.PowerCoderClient.BaseClasses;
public abstract class PowerCodeBaseCommando(string identifier) : ConsoleCommandBase<CommandPromptConfiguration>(identifier)
{
    protected ObjectStorageBase<CodePatterns, CodePattern> CodePatterns = new();
    public override void OnInitialized()
    {
        Environment.CurrentDirectory = new DirectoryInfo(AppContext.BaseDirectory).GetRoot().FullName;
        EventBusService.Service.Publish(new WorkingDirectoryChangedEventArgs(Environment.CurrentDirectory));
        InfoPanelService.Instance.RegisterContent(new DefaultInfoPanel(new DefaultInfoPanelContent()));
    }
    protected void OnWorkingDirectoryChanged(WorkingDirectoryChangedEventArgs e) => UpdateSuggestions(e.NewWorkingDirectory);
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