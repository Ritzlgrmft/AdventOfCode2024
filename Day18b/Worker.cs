namespace AdventOfCode2024.Day18b;

public class Worker : IWorker
{

    public long DoWork(string inputFile)
    {
        var size = 71;
        var bytes = 1024;
        var map = new bool[size + 2, size + 2];
        for (var i = 0; i < size + 2; i++)
        {
            map[i, 0] = true;
            map[i, size + 1] = true;
            map[0, i] = true;
            map[size + 1, i] = true;
        }

        var lines = File.ReadAllLines(inputFile);
        foreach (var line in lines.Take(bytes))
        {
            var coords = line.Split(',').Select(n => int.Parse(n)).ToList();
            map[coords[0] + 1, coords[1] + 1] = true;
        }

        var previousPath = new List<(int x, int y)>();
        foreach (var line in lines.Skip(bytes))
        {
            var coords = line.Split(',').Select(n => int.Parse(n)).ToList();
            map[coords[0] + 1, coords[1] + 1] = true;
            if (previousPath.Count == 0 || previousPath.Contains((coords[0] + 1, coords[1] + 1)))
            {
                previousPath = FindPath(size, map);
                if (previousPath.Count == 0)
                {
                    Console.WriteLine(line);
                    return 1;
                }
            }
        }

        return -1;
    }

    private List<(int x, int y)> FindPath(int size, bool[,] map)
    {
        var visited = new bool[size + 2, size + 2];
        var toBeChecked = new List<(int x, int y, List<(int x, int y)> path)>() { (1, 1, []) };
        while (toBeChecked.Count > 0)
        {
            var nextToBeChecked = new List<(int x, int y, List<(int x, int y)> path)>();
            foreach (var (x, y, path) in toBeChecked)
            {
                visited[x, y] = true;
                path.Add((x, y));
                if (x == size && y == size)
                {
                    return path;
                }
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x - 1, y, path.Select(p => p).ToList()));
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x + 1, y, path.Select(p => p).ToList()));
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x, y - 1, path.Select(p => p).ToList()));
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x, y + 1, path.Select(p => p).ToList()));
            }
            toBeChecked = nextToBeChecked;
        }
        return [];
    }

    private void AddToNextToBeCheckedIfPossible(List<(int x, int y, List<(int x, int y)> path)> nextToBeChecked, bool[,] map, bool[,] visited, (int x, int y, List<(int x, int y)> path) pos)
    {
        if (!map[pos.x, pos.y] && !visited[pos.x, pos.y] && !nextToBeChecked.Any(c => c.x == pos.x && c.y == pos.y))
        {
            nextToBeChecked.Add(pos);
        }
    }
}
