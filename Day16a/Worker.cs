namespace AdventOfCode2024.Day16a;

public class Worker : IWorker
{
    List<string> map = [];
    List<(int score, (int x, int y) position, (int dx, int dy) direction)> positionsToCheck = [];
    List<(int score, (int x, int y) position, (int dx, int dy) direction)> visited = [];

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

        var bestScore = int.MaxValue;
        positionsToCheck.Add((0, start, (1, 0)));
        while (positionsToCheck.Count > 0)
        {
            var minScore = positionsToCheck.Min(p => p.score);
            var pos = positionsToCheck.First(p => p.score == minScore);
            positionsToCheck.Remove(pos);
            visited.Add(pos);

            if (pos.position == end)
            {
                bestScore = Math.Min(bestScore, pos.score);
                positionsToCheck = [];
            }
            else
            {
                var positionStraight = (pos.position.x + pos.direction.dx, pos.position.y + pos.direction.dy);
                AddIfFreeAndNotAlreadyVisited(pos.score + 1, positionStraight, pos.direction);
                var directionLeft = (dx: pos.direction.dy, dy: -pos.direction.dx);
                var positionLeft = (pos.position.x + directionLeft.dx, pos.position.y + directionLeft.dy);
                AddIfFreeAndNotAlreadyVisited(pos.score + 1001, positionLeft, directionLeft);
                var directionRight = (dx: -pos.direction.dy, dy: pos.direction.dx);
                var positionRight = (pos.position.x + directionRight.dx, pos.position.y + directionRight.dy);
                AddIfFreeAndNotAlreadyVisited(pos.score + 1001, positionRight, directionRight);
            }
        }

        return bestScore;
    }

    private void AddIfFreeAndNotAlreadyVisited(int score, (int x, int y) position, (int dx, int dy) direction)
    {
        if (GetField(position.x, position.y) == '.' &&
            !visited.Any(p => p.position == position && p.direction == direction && p.score <= score) &&
            !positionsToCheck.Contains((score, position, direction)))
        {
            positionsToCheck.Add((score, position, direction));
        }
    }

    char GetField(int x, int y)
    {
        return map[y][x];
    }
}
