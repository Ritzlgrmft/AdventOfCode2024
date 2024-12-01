using System.Runtime.ExceptionServices;

namespace AdventOfCode2024.Day1a;

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
        leftList.Sort();
        rightList.Sort();

        var sum = 0;
        for (var i = 0; i < leftList.Count; i++)
        {
            sum += Math.Abs(leftList[i] - rightList[i]);
        }
        return sum;
    }
}
