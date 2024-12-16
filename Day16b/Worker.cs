using System.Data;

namespace AdventOfCode2024.Day16b;

public class Worker : IWorker
{
    List<string> map = [];
    List<(int score, (int x, int y) position, (int dx, int dy) direction, List<(int x, int y)> route)> positionsToCheck = [];
    List<(int score, (int x, int y) position, (int dx, int dy) direction, List<(int x, int y)> route)> visited = [];

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
        var bestRoute = new List<(int x, int y)>();
        positionsToCheck.Add((0, start, (1, 0), []));
        while (positionsToCheck.Min(p => p.score) <= bestScore)
        {
            // PrintMap();
            var minScore = positionsToCheck.Min(p => p.score);
            var pos = positionsToCheck.First(p => p.score == minScore);
            positionsToCheck.Remove(pos);
            visited.Add(pos);
            pos.route.Add(pos.position);

            if (pos.position == end)
            {
                bestScore = Math.Min(bestScore, pos.score);
                bestRoute.AddRange(pos.route);
            }
            else
            {
                var positionStraight = (pos.position.x + pos.direction.dx, pos.position.y + pos.direction.dy);
                AddIfFreeAndNotAlreadyVisited(pos.score + 1, positionStraight, pos.direction, pos.route);
                var directionLeft = (dx: pos.direction.dy, dy: -pos.direction.dx);
                var positionLeft = (pos.position.x + directionLeft.dx, pos.position.y + directionLeft.dy);
                AddIfFreeAndNotAlreadyVisited(pos.score + 1001, positionLeft, directionLeft, pos.route);
                var directionRight = (dx: -pos.direction.dy, dy: pos.direction.dx);
                var positionRight = (pos.position.x + directionRight.dx, pos.position.y + directionRight.dy);
                AddIfFreeAndNotAlreadyVisited(pos.score + 1001, positionRight, directionRight, pos.route);
            }
        }

        return bestRoute.Distinct().Count();
    }

    private void AddIfFreeAndNotAlreadyVisited(int score, (int x, int y) position, (int dx, int dy) direction, List<(int x, int y)> previousRoute)
    {
        if (GetField(position.x, position.y) == '.' &&
            !visited.Any(p => p.position == position && p.direction == direction && p.score < score))
        {
            var existingPositionToCheck = positionsToCheck.FirstOrDefault(p => p.score == score && p.position == position && p.direction == direction);
            if (existingPositionToCheck == (0, (0, 0), (0, 0), null))
            {
                positionsToCheck.Add((score, position, direction, previousRoute.Select(p => p).ToList()));
            }
            else
            {
                existingPositionToCheck.route.AddRange(previousRoute);
            }
        }
    }

    char GetField(int x, int y)
    {
        return map[y][x];
    }
}
