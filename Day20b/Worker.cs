namespace AdventOfCode2024.Day20b;

public class Worker : IWorker
{
    List<string> map = [];
    int[,]? times;

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

        var pathWithoutCheating = FindPath(start, end, int.MaxValue);
        pathWithoutCheating.Add(end);

        var minWin = 100;
        List<int> cheats = [];
        for (var i = 0; i < pathWithoutCheating.Count; i++)
        {
            for (var j = i + 1; j < pathWithoutCheating.Count; j++)
            {
                var distanceInPath = Math.Abs(j - i);
                var distanceDirect =
                    Math.Abs(pathWithoutCheating[i].x - pathWithoutCheating[j].x) +
                    Math.Abs(pathWithoutCheating[i].y - pathWithoutCheating[j].y);
                if (distanceDirect <= 20)
                {
                    var cheatWin = distanceInPath - distanceDirect;
                    if (cheatWin >= minWin)
                    {
                        cheats.Add(cheatWin);
                    }
                }
            }
        }

        return cheats.Count;
    }

    private List<(int x, int y)> FindPath((int x, int y) start, (int x, int y) end, int timeWithoutCheating)
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
                    return pos.path;
                }

                visited.Add(pos.pos);
                var nextPath = pos.path.Select(p => p).Union([pos.pos]).ToList();
                var nextPos = (x: pos.pos.x + 1, pos.pos.y);
                if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
                nextPos = (x: pos.pos.x - 1, pos.pos.y);
                if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
                nextPos = (x: pos.pos.x, pos.pos.y + 1);
                if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
                nextPos = (x: pos.pos.x, pos.pos.y - 1);
                if (GetField(nextPos.x, nextPos.y) == '.' && !visited.Contains(nextPos))
                {
                    nextPositionsToCheck.Add((nextPos, nextPath));
                }
            }
            positionsToCheck = nextPositionsToCheck;
        }

        return [];
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

