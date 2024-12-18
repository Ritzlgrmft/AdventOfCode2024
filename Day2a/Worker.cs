namespace AdventOfCode2024.Day2a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var safeReports = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            var levels = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(l => int.Parse(l)).ToList();
            if (IsReportSafe(levels, 1, 3) || IsReportSafe(levels, -1, -3))
            {
                safeReports++;
            }
        }

        return safeReports;
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
