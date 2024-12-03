using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var pattern = @"(mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\))";
        var sum = 0;
        var isEnabled = true;
        foreach (var line in File.ReadLines(inputFile))
        {
            foreach (Match match in Regex.Matches(line, pattern))
            {
                switch (match.Value)
                {
                    case "do()":
                        isEnabled = true;
                        break;
                    case "don't()":
                        isEnabled = false;
                        break;
                    default:
                        if (isEnabled)
                        {
                            var parts = match.Value.Split("(,)".ToCharArray());
                            sum += int.Parse(parts[1]) * int.Parse(parts[2]);
                        }
                        break;
                }
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
