using System.ComponentModel;
using System.Data;

namespace AdventOfCode2024.Day10b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var map = new List<int[]>();
        foreach (var line in File.ReadLines(inputFile))
        {
            map.Add(line.ToCharArray().Select(c => c - '0').ToArray());
        }
        var maxX = map[0].Length;
        var maxY = map.Count;

        var targets = new Dictionary<int, Dictionary<(int x, int y), List<(int x, int y)>>>();
        for (var n = 9; n >= 0; n--)
        {
            for (var y = 0; y < maxY; y++)
            {
                for (var x = 0; x < maxX; x++)
                {
                    if (GetNumber(map, x, y, maxX, maxY) == n)
                    {
                        if (n == 9)
                        {
                            AddTarget(targets, n, x, y);
                        }
                        else
                        {
                            if (GetNumber(map, x - 1, y, maxX, maxY) == n + 1)
                            {
                                AddTarget(targets, n, x, y, n + 1, x - 1, y);
                            }
                            if (GetNumber(map, x + 1, y, maxX, maxY) == n + 1)
                            {
                                AddTarget(targets, n, x, y, n + 1, x + 1, y);
                            }
                            if (GetNumber(map, x, y - 1, maxX, maxY) == n + 1)
                            {
                                AddTarget(targets, n, x, y, n + 1, x, y - 1);
                            }
                            if (GetNumber(map, x, y + 1, maxX, maxY) == n + 1)
                            {
                                AddTarget(targets, n, x, y, n + 1, x, y + 1);
                            }
                        }

                    }
                }
            }
        }

        var sum = 0L;
        foreach (var target in targets[0].Values)
        {
            sum += target.Count;
        }
        return sum;
    }

    private void AddTarget(Dictionary<int, Dictionary<(int x, int y), List<(int x, int y)>>> targets, int n, int x, int y)
    {
        if (!targets.ContainsKey(n))
        {
            targets[n] = [];
        }
        if (!targets[n].ContainsKey((x, y)))
        {
            targets[n][(x, y)] = [];
        }
        AddTargetEvenIfExists(targets[n][(x, y)], (x, y));
    }

    private void AddTarget(Dictionary<int, Dictionary<(int x, int y), List<(int x, int y)>>> targets, int n, int x, int y, int n2, int x2, int y2)
    {
        if (targets[n2].ContainsKey((x2, y2)))
        {
            if (!targets.ContainsKey(n))
            {
                targets[n] = [];
            }
            if (!targets[n].ContainsKey((x, y)))
            {
                targets[n][(x, y)] = [];
            }
            foreach (var pos in targets[n2][(x2, y2)])
            {
                AddTargetEvenIfExists(targets[n][(x, y)], pos);
            }
        }
    }

    private void AddTargetEvenIfExists(List<(int x, int y)> list, (int x, int y) value)
    {
        list.Add(value);
    }

    int GetNumber(List<int[]> map, int x, int y, int maxX, int maxY)
    {
        if (x < 0 || x >= maxX || y < 0 || y >= maxY)
        {
            return -1;
        }
        else
        {
            return map[y][x];
        }
    }
}
