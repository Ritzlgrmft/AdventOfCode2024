namespace AdventOfCode2024.Day18a;

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

        var lines = File.ReadAllLines(inputFile).Take(bytes);
        foreach (var line in lines)
        {
            var coords = line.Split(',').Select(n => int.Parse(n)).ToList();
            map[coords[0] + 1, coords[1] + 1] = true;
        }

        var visited = new bool[size + 2, size + 2];
        var toBeChecked = new List<(int x, int y, int length)>() { (1, 1, 0) };
        while (toBeChecked.Count > 0)
        {
            var nextToBeChecked = new List<(int x, int y, int length)>();
            foreach (var (x, y, length) in toBeChecked)
            {
                visited[x, y] = true;
                if (x == size && y == size)
                {
                    return length;
                }
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x - 1, y, length + 1));
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x + 1, y, length + 1));
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x, y - 1, length + 1));
                AddToNextToBeCheckedIfPossible(nextToBeChecked, map, visited, (x, y + 1, length + 1));
            }
            toBeChecked = nextToBeChecked;
        }

        return 0;
    }

    private void AddToNextToBeCheckedIfPossible(List<(int x, int y, int length)> nextToBeChecked, bool[,] map, bool[,] visited, (int x, int y, int length) pos)
    {
        if (!map[pos.x, pos.y] && !visited[pos.x, pos.y] && !nextToBeChecked.Any(c => c.x == pos.x && c.y == pos.y))
        {
            nextToBeChecked.Add(pos);
        }
    }
}
