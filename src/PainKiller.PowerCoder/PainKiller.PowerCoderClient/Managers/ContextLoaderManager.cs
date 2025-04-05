namespace PainKiller.PowerCoderClient.Managers;

public class ContextLoaderManager
{
    public string LoadContext(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return "Invalid path.";
        if (File.Exists(path)) return LoadFileContext(path);
        if (Directory.Exists(path)) return LoadDirectoryContext(path);
        return "Path does not exist.";
    }
    private string LoadFileContext(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            return $"Error reading file: {ex.Message}";
        }
    }
    private string LoadDirectoryContext(string directoryPath)
    {
        var fileContents = new List<string>();

        try
        {
            var files = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                fileContents.Add($"File: {Path.GetFileName(file)}\n{content}");
            }
        }
        catch (Exception ex)
        {
            return $"Error reading directory: {ex.Message}";
        }

        return string.Join("\n\n", fileContents);
    }
}