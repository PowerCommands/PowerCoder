namespace PainKiller.PowerCoderClient.DomainObjects;

public class CodePattern
{
    public string Id { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public bool AntiPattern { get; set; }
}