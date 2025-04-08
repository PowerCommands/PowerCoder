using Spectre.Console;

namespace PainKiller.PowerCoderClient.Presentation;

public class FileConsoleWriter(string filePath) : IConsoleWriter
{
    private readonly SpectreConsoleWriter _consoleWriter = SpectreConsoleWriter.Instance;

    public void Write(string text, bool writeLog = true, Color? consoleColor = null, string scope = "")
    {
        _consoleWriter.Write(text, writeLog, consoleColor, scope);
        if (writeLog) WriteToFile(text);
    }

    public void WriteLine(string text = "", bool writeLog = true, Color? consoleColor = null, string scope = "")
    {
        _consoleWriter.WriteLine(text, writeLog, consoleColor, scope);
        if (writeLog) WriteToFile(text);
    }

    public void WriteSuccessLine(string text, bool writeLog = true, string scope = "")
    {
        _consoleWriter.WriteSuccessLine(text, writeLog, scope);
        if (writeLog) WriteToFile("✅ " + text);
    }

    public void WriteWarning(string text, string scope = "")
    {
        _consoleWriter.WriteWarning(text, scope);
        WriteToFile("⚠️ " + text);
    }

    public void WriteError(string text, string scope = "")
    {
        _consoleWriter.WriteError(text, scope);
        WriteToFile("❌ " + text);
    }

    public void WriteCritical(string text, string scope = "")
    {
        _consoleWriter.WriteCritical(text, scope);
        WriteToFile("💥 " + text);
    }

    public void WriteHeadLine(string text, bool writeLog = true, string scope = "")
    {
        _consoleWriter.WriteHeadLine(text, writeLog, scope);
        if (writeLog) WriteToFile("### " + text);
    }

    public void WriteUrl(string text, bool writeLog = true, string scope = "")
    {
        _consoleWriter.WriteUrl(text, writeLog, scope);
        if (writeLog) WriteToFile("🔗 " + text);
    }

    public void WriteDescription(string label, string text, bool writeToLog = true, Color? consoleColor = null, bool noBorder = false, string scope = "")
    {
        _consoleWriter.WriteDescription(label, text, writeToLog, consoleColor, noBorder, scope);
        if (writeToLog) WriteToFile($"{label}: {text}");
    }

    public void WriteTable<T>(IEnumerable<T> items, string[]? columnNames = null, Color? consoleColor = null, Color? borderColor = null, bool expand = true)
    {
        _consoleWriter.WriteTable(items, columnNames, consoleColor, borderColor, expand);
        WriteToFile("[Table output written to console]");
    }

    public void WritePrompt(string prompt)
    {
        _consoleWriter.WritePrompt(prompt);
    }

    public void WriteRowWithColor(int top, ConsoleColor foregroundColor, ConsoleColor backgroundColor, string rowContent)
    {
        _consoleWriter.WriteRowWithColor(top, foregroundColor, backgroundColor, rowContent);
    }

    public void Clear()
    {
        _consoleWriter.Clear();
    }

    public void ClearRow(int top)
    {
        _consoleWriter.ClearRow(top);
    }

    public void SetMargin(int reservedLines)
    {
        _consoleWriter.SetMargin(reservedLines);
    }
    private void WriteToFile(string text) => File.AppendAllText(filePath, text + Environment.NewLine);
}
