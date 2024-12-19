namespace AdventOfCode2024.Day19a;

public class Worker : IWorker
{
    List<string> towels = [];
    List<string> patterns = [];
    List<string> impossiblePatterns = [];

    public long DoWork(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        towels = lines[0].Split(", ").ToList();
        patterns.AddRange(lines.Skip(2));

        var possibles = 0;
        foreach (var pattern in patterns)
        {
            if (IsPossible(pattern))
            {
                possibles++;
            }
        }

        return possibles;
    }

    bool IsPossible(string pattern)
    {
        if (impossiblePatterns.Contains(pattern))
        {
            return false;
        }

        foreach (var towel in towels.Where(t => pattern.StartsWith(t)))
        {
            if (pattern.Length == towel.Length || IsPossible(pattern[towel.Length..]))
            {
                return true;
            }
        }
        impossiblePatterns.Add(pattern);
        return false;
    }
}
