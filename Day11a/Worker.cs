using System.ComponentModel;
using System.Data;

namespace AdventOfCode2024.Day11a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var stones = File.ReadAllText(inputFile).Split(" ").Select(s => long.Parse(s)).ToList();

        for (var i = 0; i < 25; i++)
        {
            var newStones = new List<long>();
            foreach (var stone in stones)
            {
                if (stone == 0)
                {
                    newStones.Add(1);
                }
                else
                {
                    var stoneAsString = stone.ToString();
                    if (stoneAsString.Length % 2 == 0)
                    {
                        newStones.Add(long.Parse(stoneAsString[..(stoneAsString.Length / 2)]));
                        newStones.Add(long.Parse(stoneAsString[(stoneAsString.Length / 2)..]));
                    }
                    else
                    {
                        newStones.Add(stone * 2024);
                    }
                }
            }
            stones = newStones;
        }

        return stones.Count;
    }

}
