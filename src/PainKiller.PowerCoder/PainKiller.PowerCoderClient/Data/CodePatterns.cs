using PainKiller.PowerCoderClient.Contracts;
using PainKiller.PowerCoderClient.DomainObjects;
namespace PainKiller.PowerCoderClient.Data;

public class CodePatterns :IDataObjects<CodePattern>
{
    public DateTime LastUpdated { get; set; }
    public List<CodePattern> Items { get; set; } = [];
}