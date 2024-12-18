namespace AdventOfCode2024.Day5a;

public class Worker : IWorker
{


    public long DoWork(string inputFile)
    {
        var readRules = true;
        var rules = new Dictionary<int, List<int>>();
        var updates = new List<List<int>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            if (line.Length == 0)
            {
                readRules = false;
            }
            else if (readRules)
            {
                var parts = line.Split('|').Select(p => int.Parse(p)).ToList();
                if (rules.ContainsKey(parts[0]))
                {
                    rules[parts[0]].Add(parts[1]);
                }
                else
                {
                    rules[parts[0]] = [parts[1]];
                }
            }
            else
            {
                updates.Add(line.Split(',').Select(p => int.Parse(p)).ToList());
            }
        }

        var sum = 0;
        foreach (var update in updates)
        {
            if (IsUpdateValid(update, rules))
            {
                sum += update[(update.Count - 1) / 2];
            }
        }
        return sum;
    }

    private bool IsUpdateValid(List<int> update, Dictionary<int, List<int>> rules)
    {
        for (var i = 0; i < update.Count - 1; i++)
        {
            if (rules.TryGetValue(update[i], out var rule))
            {
                if (!rule.Contains(update[i + 1]))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
