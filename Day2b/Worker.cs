namespace AdventOfCode2024.Day2b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var safeReports = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            var levels = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(l => int.Parse(l)).ToList();
            if (IsReportSafe(levels))
            {
                safeReports++;
            }
        }

        return safeReports;
    }

    bool IsReportSafe(List<int> levels)
    {
        var result = CheckReport(levels);
        if (!result)
        {
            for (var j = 0; j < levels.Count; j++)
            {
                var dampenedLevels = levels
                .Select((level, index) => (level, index))
                .Where(x => x.index != j)
                .Select(x => x.level)
                .ToList();
                result = result || CheckReport(dampenedLevels);
            }
        }
        return result;
    }

    private bool CheckReport(List<int> levels)
    {
        int min, max;
        if (levels[0] < levels[1])
        {
            min = 1;
            max = 3;
        }
        else if (levels[0] > levels[1])
        {
            min = -1;
            max = -3;
        }
        else
        {
            return false;
        }

        for (var i = 0; i < levels.Count - 1; i++)
        {
            var diff = levels[i + 1] - levels[i];
            if ((min > 0 && diff >= min && diff <= max) || (min < 0 && diff <= min && diff >= max))
            {
                // check next level
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}

