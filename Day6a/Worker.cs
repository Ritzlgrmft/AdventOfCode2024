using System.Data;

namespace AdventOfCode2024.Day6a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var obstacles = new List<(int x, int y)>();
        var y = 0;
        int maxX = 0;
        (int x, int y) guard = (0, 0);
        foreach (var line in File.ReadLines(inputFile).Select(l => l.ToCharArray()))
        {
            maxX = line.Length - 1;
            for (var x = 0; x <= maxX; x++)
            {
                if (line[x] == '#')
                {
                    obstacles.Add((x, y));
                }
                else if (line[x] == '^')
                {
                    guard = (x, y);
                }
            }
            y++;
        }
        var maxY = y - 1;

        (int x, int y) direction = (0, -1);
        var visited = new bool[maxX + 1, maxY + 1];
        while (guard.x >= 0 && guard.x <= maxX && guard.y >= 0 && guard.y <= maxY)
        {
            visited[guard.x, guard.y] = true;
            if (obstacles.Contains((guard.x + direction.x, guard.y + direction.y)))
            {
                // turn right
                if (direction == (0, -1))
                {
                    direction = (1, 0);
                }
                else if (direction == (1, 0))
                {
                    direction = (0, 1);
                }
                else if (direction == (0, 1))
                {
                    direction = (-1, 0);
                }
                else if (direction == (-1, 0))
                {
                    direction = (0, -1);
                }
            }
            guard = (guard.x + direction.x, guard.y + direction.y);
        }
        var sum = 0;
        for (var x = 0; x <= maxX; x++)
        {
            for (y = 0; y <= maxY; y++)
            {
                if (visited[x, y])
                {
                    sum++;
                }
            }
        }
        return sum;
    }

}
