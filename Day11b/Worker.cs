using System.ComponentModel;
using System.Data;

namespace AdventOfCode2024.Day11b;

public class Worker : IWorker
{
    List<Dictionary<long, long>> stoneCache;

    public long DoWork(string inputFile)
    {
        var stones = File.ReadAllText(inputFile).Split(" ").Select(s => long.Parse(s)).ToList();

        stoneCache = [];
        for (var i = 0; i < 75; i++)
        {
            stoneCache.Add([]);
        }

        var newStones = 0L;
        foreach (var stone in stones)
        {
            newStones += CalculateStone(stone, 0);
        }

        return newStones;
    }

    long CalculateStone(long stone, int i)
    {
        if (i == 75)
        {
            return 1;
        }
        if (stoneCache[i].TryGetValue(stone, out var result))
        {
            return result;
        }

        long newStones;
        if (stone == 0)
        {
            newStones = CalculateStone(1, i + 1);
        }
        else
        {
            var stoneAsString = stone.ToString();
            if (stoneAsString.Length % 2 == 0)
            {
                newStones = CalculateStone(long.Parse(stoneAsString[..(stoneAsString.Length / 2)]), i + 1) + CalculateStone(long.Parse(stoneAsString[(stoneAsString.Length / 2)..]), i + 1);
            }
            else
            {
                newStones = CalculateStone(stone * 2024, i + 1);
            }
        }
        stoneCache[i][stone] = newStones;
        return newStones;
    }

}
