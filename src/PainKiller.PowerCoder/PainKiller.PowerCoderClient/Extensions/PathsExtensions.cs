namespace PainKiller.PowerCoderClient.Extensions;

public static class PathsExtensions
{
    public static DirectoryInfo GetRoot(this DirectoryInfo directory)
    {
        if (directory.FullName == AppContext.BaseDirectory)
        {
            var baseDirInfo = new DirectoryInfo(AppContext.BaseDirectory);
            var candidate = GetAncestor(baseDirInfo, 4);
            if (candidate == null) return new DirectoryInfo(Environment.CurrentDirectory);
        }
        if (directory.Name == "PainKiller.CommandPrompt.CoreLib") return directory.Parent!;
        if (directory.GetFiles("program.cs").Length == 1) return directory.Parent!;
        return new DirectoryInfo(Environment.CurrentDirectory);
    }

    private static DirectoryInfo? GetAncestor(DirectoryInfo? dir, int levels)
    {
        if (dir == null) return null;
        DirectoryInfo? current = dir;
        for (int i = 0; i < levels; i++)
        {
            current = current.Parent;
            if (current == null) 
                return null;
        }
        return current;
    }
}