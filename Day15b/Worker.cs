namespace AdventOfCode2024.Day15b;

public class Worker : IWorker
{
    List<char[]> map = [];
    List<char> directions = [];
    int maxX = 0;
    int maxY = 0;

    public long DoWork(string inputFile)
    {
        var readMap = true;
        foreach (var line in File.ReadLines(inputFile))
        {
            if (string.IsNullOrEmpty(line))
            {
                readMap = false;
            }
            else if (readMap)
            {
                var row = new List<char>();
                foreach (var c in line.ToCharArray())
                {
                    switch (c)
                    {
                        case '#':
                            row.AddRange(['#', '#']);
                            break;
                        case 'O':
                            row.AddRange(['[', ']']);
                            break;
                        case '.':
                            row.AddRange(['.', '.']);
                            break;
                        case '@':
                            row.AddRange(['@', '.']);
                            break;
                    }
                }
                map.Add(row.ToArray());
            }
            else
            {
                directions.AddRange(line.ToCharArray());
            }
        }
        maxX = map[0].Length;
        maxY = map.Count;

        var robot = FindRobot();
        foreach (var direction in directions)
        {
            switch (direction)
            {
                case '^':
                    robot = MoveVertical(robot, -1);
                    break;
                case 'v':
                    robot = MoveVertical(robot, 1);
                    break;
                case '>':
                    robot = MoveHorizontal(robot, 1);
                    break;
                case '<':
                    robot = MoveHorizontal(robot, -1);
                    break;
            }
        }

        var sum = 0;
        for (var x = 0; x < maxX; x++)
        {
            for (var y = 0; y < maxY; y++)
            {
                if (GetField(x, y) == '[')
                {
                    sum += 100 * y + x;
                }
            }
        }
        return sum;
    }

    (int x, int y) FindRobot()
    {
        for (var x = 0; x < maxX; x++)
        {
            for (var y = 0; y < maxY; y++)
            {
                if (GetField(x, y) == '@')
                {
                    return (x, y);
                }
            }
        }
        throw new Exception("robot not found");
    }

    char GetField(int x, int y)
    {
        return map[y][x];
    }

    void SetField(int x, int y, char field)
    {
        map[y][x] = field;
    }

    (int x, int y) MoveHorizontal((int x, int y) robot, int dx)
    {
        var nextCheck = robot.x + dx;
        while (GetField(nextCheck, robot.y) == '[' || GetField(nextCheck, robot.y) == ']')
        {
            nextCheck += 2 * dx;
        }
        if (GetField(nextCheck, robot.y) == '.')
        {
            while (nextCheck != robot.x)
            {
                SetField(nextCheck, robot.y, GetField(nextCheck - dx, robot.y));
                nextCheck -= dx;
            }
            SetField(robot.x, robot.y, '.');
            robot = (x: robot.x + dx, y: robot.y);
        }
        return robot;
    }

    (int x, int y) MoveVertical((int x, int y) robot, int dy)
    {
        var fieldsToMove = new List<List<int>>();
        fieldsToMove.Add([robot.x]);
        var isBlocked = false;
        var isMovable = false;
        var nextRowToMove = new List<int>();
        while (!isBlocked && !isMovable)
        {
            nextRowToMove = [];
            foreach (var field in fieldsToMove.Last())
            {
                if (GetField(field, robot.y + fieldsToMove.Count * dy) == '[')
                {
                    nextRowToMove.AddRange([field, field + 1]);
                }
                else if (GetField(field, robot.y + fieldsToMove.Count * dy) == ']')
                {
                    nextRowToMove.AddRange([field - 1, field]);
                }
                else if (GetField(field, robot.y + fieldsToMove.Count * dy) == '#')
                {
                    isBlocked = true;
                }
            }
            if (nextRowToMove.Count > 0)
            {
                fieldsToMove.Add(nextRowToMove.Distinct().ToList());
            }
            else if (!isBlocked)
            {
                isMovable = true;
            }
        }
        if (isMovable)
        {
            for (var i = fieldsToMove.Count - 1; i >= 0; i--)
            {
                foreach (var field in fieldsToMove[i])
                {
                    SetField(field, robot.y + (i + 1) * dy, GetField(field, robot.y + i * dy));
                    SetField(field, robot.y + i * dy, '.');
                }
            }
            robot = (x: robot.x, y: robot.y + dy);
        }
        return robot;
    }
}
