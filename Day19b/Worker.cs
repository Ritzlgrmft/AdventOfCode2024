namespace AdventOfCode2024.Day19b;

public class Worker : IWorker
{
    List<string> towels = [];
    List<string> patterns = [];
    Dictionary<string, long> patternCache = [];

    public long DoWork(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        towels = lines[0].Split(", ").ToList();
        patterns.AddRange(lines.Skip(2));

        var possibles = 0L;
        foreach (var pattern in patterns)
        {
            possibles += IsPossible(pattern);
        }

        return possibles;
    }

    long IsPossible(string pattern)
    {
        if (patternCache.TryGetValue(pattern, out var isPossible))
        {
            return isPossible;
        }

        var result = 0L;
        foreach (var towel in towels.Where(t => pattern.StartsWith(t)))
        {
            if (pattern.Length == towel.Length)
            {
                result++;
            }
            else
            {
                result += IsPossible(pattern[towel.Length..]);
            }
        }
        patternCache[pattern] = result;
        return result;
    }
}
