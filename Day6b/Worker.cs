using System.Data;

namespace AdventOfCode2024.Day6b;

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

        var path = GetPath(guard, maxX, maxY, obstacles).Skip(1).ToList();

        var loops = 0;
        foreach (var pos in path)
        {
            var newObstacles = obstacles.Select(o => o).ToList();
            newObstacles.Add(pos);
            if (IsLoop(guard, maxX, maxY, (0, -1), newObstacles))
            {
                loops++;
            }
        }

        return loops;
    }

    private List<(int, int)> GetPath((int x, int y) guard, int maxX, int maxY, List<(int x, int y)> obstacles)
    {
        (int x, int y) direction = (0, -1);
        var path = new List<(int, int)>();
        while (IsValid(guard.x, maxX) && IsValid(guard.y, maxY))
        {
            if (!path.Contains(guard))
            {
                path.Add(guard);
            }
            if (obstacles.Contains((guard.x + direction.x, guard.y + direction.y)))
            {
                direction = TurnRight(direction);
            }
            guard = (guard.x + direction.x, guard.y + direction.y);
        }
        return path;
    }

    private bool IsLoop((int x, int y) guard, int maxX, int maxY, (int x, int y) direction, List<(int x, int y)> obstacles)
    {
        var visited = new List<(int x, int y)>[maxX + 1, maxY + 1];
        for (var x = 0; x <= maxX; x++)
        {
            for (var y = 0; y <= maxY; y++)
            {
                visited[x, y] = [];
            }
        }

        while (IsValid(guard.x, maxX) && IsValid(guard.y, maxY))
        {
            if (visited[guard.x, guard.y].Contains(direction))
            {
                return true;
            }
            else
            {
                visited[guard.x, guard.y].Add(direction);
            }

            while (obstacles.Contains((guard.x + direction.x, guard.y + direction.y)))
            {
                direction = TurnRight(direction);
            }
            guard = (guard.x + direction.x, guard.y + direction.y);
        }
        return false;
    }

    private bool IsValid(int v, int max)
    {
        return v >= 0 && v <= max;
    }

    private (int x, int y) TurnRight((int x, int y) direction)
    {
        return (-direction.y, direction.x);
    }
}
