using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var pattern = @"(mul\(\d{1,3},\d{1,3}\))";
        var sum = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            foreach (Match match in Regex.Matches(line, pattern))
            {
                var parts = match.Value.Split("(,)".ToCharArray());
                sum += int.Parse(parts[1]) * int.Parse(parts[2]);
            }
        }

        return sum;
    }

    bool IsReportSafe(List<int> levels, int min, int max)
    {
        for (var i = 0; i < levels.Count - 1; i++)
        {
            var diff = levels[i + 1] - levels[i];
            if (min > 0 && diff >= min && diff <= max)
            {
                // ok
            }
            else if (min < 0 && diff <= min && diff >= max)
            {
                // ok
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
