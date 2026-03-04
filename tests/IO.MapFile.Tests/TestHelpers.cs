namespace Altemiq.IO.MapFile.Tests;

internal static class TestHelpers
{
    public static string NormalizeMapfile(string s)
    {
        if (s is null) return string.Empty;

        var normalized = s.Replace("\r\n", "\n").Replace("\r", "\n");
        var lines = normalized.Split('\n').Select(static l => l.TrimEnd()).ToList();

        var (acc, _) = lines.Aggregate(
            (acc: new List<string>(), lastEmpty: false),
            static (state, line) =>
            {
                var isEmpty = string.IsNullOrWhiteSpace(line);
                if (isEmpty && state.lastEmpty)
                {
                    return state;
                }

                state.acc.Add(line);
                return (state.acc, isEmpty);
            });

        return string.Join("\n", acc).Trim();
    }

    public static string StripWhitespace(string? s)
    {
        return new string([.. (s ?? string.Empty).Where(static c => !char.IsWhiteSpace(c))]);
    }
}
