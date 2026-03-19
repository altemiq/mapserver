namespace Altemiq.IO.MapFile.Tests;

internal static class TestHelpers
{
    public static string NormalizeMapfile(string s)
    {
        if (s is null)
        {
            return string.Empty;
        }

        var (nonEmptyLines, _) = s
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split('\n')
            .Select(static l => l.TrimEnd())
            .Aggregate(
                (Lines: new List<string>(), LastEmpty: false),
                static (state, line) =>
                {
                    var currentEmpty = string.IsNullOrWhiteSpace(line);
                    if (currentEmpty && state.LastEmpty)
                    {
                        return state;
                    }

                    state.Lines.Add(line);
                    return (state.Lines, currentEmpty);
                });

        return string.Join("\n", nonEmptyLines).Trim();
    }

    public static string StripWhitespace(string? s)
    {
        return new string([.. (s ?? string.Empty).Where(static c => !char.IsWhiteSpace(c))]);
    }
}
