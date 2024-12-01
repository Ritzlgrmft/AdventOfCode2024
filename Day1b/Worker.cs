using System.Runtime.ExceptionServices;

namespace AdventOfCode2024.Day1b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            leftList.Add(int.Parse(parts[0]));
            rightList.Add(int.Parse(parts[1]));
        }

        var sum = 0;
        foreach (var left in leftList)
        {
            sum += left * rightList.Count(n => n == left);
        }
        return sum;
    }
}
