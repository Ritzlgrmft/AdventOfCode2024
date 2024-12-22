namespace AdventOfCode2024.Day20a;

public class Worker : IWorker
{
    List<string> map = [];
    int[,] times = new int[0, 0];

    public long DoWork(string inputFile)
    {
        (int x, int y) start = (0, 0);
        (int x, int y) end = (0, 0);
        var row = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            var index = line.IndexOf('S');
            if (index != -1)
            {
                start = (index, row);
            }
            index = line.IndexOf('E');
            if (index != -1)
            {
                end = (index, row);
            }

            map.Add(line);
            row++;
        }

        map[start.y] = map[start.y].Replace('S', '.');
        map[end.y] = map[end.y].Replace('E', '.');
        times = new int[map.Count, map[0].Length];

        var timeWithoutCheating = 0;
        timeWithoutCheating = FindPath(start, end, int.MaxValue);

        List<(int x, int y, bool alreadyCheated, List<(int x, int y)> visited)> positionsToCheckWithCheating = [];
        List<int> cheats = [];
        positionsToCheckWithCheating.Add((start.x, start.y, false, []));
        for (var time = 0; time < timeWithoutCheating; time++)
        {
            var nextPositionsToCheck = new List<(int x, int y, bool alreadyCheated, List<(int x, int y)> visited)>();
            foreach (var pos in positionsToCheckWithCheating)
            {
                if ((pos.x, pos.y) == end)
                {
                    cheats.Add(time);
                }
                else
                {
                    AddIfPossible(nextPositionsToCheck, pos, (1, 0), end, timeWithoutCheating, time, cheats);
                    AddIfPossible(nextPositionsToCheck, pos, (-1, 0), end, timeWithoutCheating, time, cheats);
                    AddIfPossible(nextPositionsToCheck, pos, (0, 1), end, timeWithoutCheating, time, cheats);
                    AddIfPossible(nextPositionsToCheck, pos, (0, -1), end, timeWithoutCheating, time, cheats);
                }
            }
            positionsToCheckWithCheating = nextPositionsToCheck.Distinct().ToList();
        }

        return cheats.Count(t => t <= timeWithoutCheating - 100);
    }

    private int FindPath((int x, int y) start, (int x, int y) end, int timeWithoutCheating)
    {
        List<((int x, int y) pos, List<(int x, int y)> path)> positionsToCheck = [(start, [])];
        List<(int x, int y)> visited = [];

        for (var time = 0; time < timeWithoutCheating; time++)
        {
            var nextPositionsToCheck = new List<((int x, int y) pos, List<(int x, int y)> path)>();
            foreach (var pos in positionsToCheck)
            {
                if (pos.pos == end)
                {
                    for (var i = 1; i <= pos.path.Count; i++)
                    {
                        var pathPos = pos.path[pos.path.Count - i];
                        times[pathPos.y, pathPos.x] = i;
                    }
                    return time;
                }

                visited.Add(pos.pos);
                var nextPath = pos.path.Select(p => p).Union([pos.pos]).ToList();
                var nextPos = (x: pos.pos.x + 1, pos.pos.y);
                if (times[nextPos.y, nextPos.x] > 0)
                {
                    return time + times[nextPos.y, nextPos.x];
                }
                else if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
                nextPos = (x: pos.pos.x - 1, pos.pos.y);
                if (times[nextPos.y, nextPos.x] > 0)
                {
                    return time + times[nextPos.y, nextPos.x];
                }
                else if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
                nextPos = (x: pos.pos.x, pos.pos.y + 1);
                if (times[nextPos.y, nextPos.x] > 0)
                {
                    return time + times[nextPos.y, nextPos.x];
                }
                else if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
                nextPos = (x: pos.pos.x, pos.pos.y - 1);
                if (times[nextPos.y, nextPos.x] > 0)
                {
                    return time + times[nextPos.y, nextPos.x];
                }
                else if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
            }
            positionsToCheck = nextPositionsToCheck;
        }

        return -1;
    }

    private void AddIfPossible(List<(int x, int y, bool alreadyCheated, List<(int x, int y)> visited)> nextPositionsToCheck,
        (int x, int y, bool alreadyCheated, List<(int x, int y)> visited) pos, (int dx, int dy) direction, (int x, int y) end,
        int timeWithoutCheating, int time, List<int> cheats)
    {
        var nextPos = (x: pos.x + direction.dx, y: pos.y + direction.dy);
        if (GetField(nextPos.x, nextPos.y) == '.')
        {
            if (!pos.visited.Contains(nextPos))
            {
                nextPositionsToCheck.Add((nextPos.x, nextPos.y, pos.alreadyCheated,
                    pos.visited.Select(v => v).Union([(pos.x, pos.y)]).ToList()));
            }
        }
        else
        {
            var nextPosCheated = (x: pos.x + 2 * direction.dx, y: pos.y + 2 * direction.dy);
            if (!pos.alreadyCheated && GetField(nextPosCheated.x, nextPosCheated.y) == '.' && !pos.visited.Contains(nextPosCheated))
            {
                if (nextPosCheated == end)
                {
                    cheats.Add(time + 2);
                }
                else
                {
                    if (times[nextPosCheated.y, nextPosCheated.x] == 0)
                    {
                        times[nextPosCheated.y, nextPosCheated.x] = FindPath(nextPosCheated, end, timeWithoutCheating - time);
                    }
                    if (times[nextPosCheated.y, nextPosCheated.x] > 0)
                    {
                        cheats.Add(time + times[nextPosCheated.y, nextPosCheated.x] + 2);
                    }
                }
            }
        }
    }

    char GetField(int x, int y)
    {
        if (x >= 0 && x < map[0].Length && y >= 0 && y < map.Count)
        {
            return map[y][x];
        }
        else
        {
            return 'x';
        }
    }
}

