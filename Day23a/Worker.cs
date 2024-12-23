
namespace AdventOfCode2024.Day23a;

public class Worker : IWorker
{
    Dictionary<string, List<string>> connections = [];

    public long DoWork(string inputFile)
    {

        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split("-");
            if (connections.ContainsKey(lineParts[0]))
            {
                connections[lineParts[0]].Add(lineParts[1]);
            }
            else
            {
                connections[lineParts[0]] = [lineParts[1]];
            }
            if (connections.ContainsKey(lineParts[1]))
            {
                connections[lineParts[1]].Add(lineParts[0]);
            }
            else
            {
                connections[lineParts[1]] = [lineParts[0]];
            }
        }

        var groups = new List<string>();
        foreach (var c1 in connections.Keys.Where(k => k[0] == 't'))
        {
            foreach (var group in connections[c1].Combinations(2))
            {
                if (IsLanGroup(group))
                {
                    var completeGroup = string.Join(",", group.Union([c1]).Order());
                    if (!groups.Contains(completeGroup))
                    {
                        groups.Add(completeGroup);
                        Console.WriteLine(completeGroup);
                    }
                }
            }
        }
        return groups.Count;
    }

    private bool IsLanGroup(IEnumerable<string> group)
    {
        var first = group.First();
        var rest = group.Skip(1);
        if (!rest.Any())
        {
            return true;
        }
        foreach (var r in rest)
        {
            if (!connections[first].Contains(r))
            {
                return false;
            }
        }
        return IsLanGroup(rest);
    }
}

public static class ExtensionMethods
{
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int n)
    {
        if (n == 0)
            yield return Enumerable.Empty<T>();


        int count = 1;
        foreach (T item in source)
        {
            foreach (var innerSequence in source.Skip(count).Combinations(n - 1))
            {
                yield return new T[] { item }.Concat(innerSequence);
            }
            count++;
        }
    }
}