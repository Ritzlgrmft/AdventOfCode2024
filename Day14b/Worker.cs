namespace AdventOfCode2024.Day14b;

public class Worker : IWorker
{
    List<((int dx, int dy) buttonA, (int dx, int dy) buttonB, (int x, int y) price)> machines = [];

    public long DoWork(string inputFile)
    {
        var robots = new List<(int x, int y, int dx, int dy)>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var parts = line.Split(new[] { '=', ',', ' ' });
            robots.Add((int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[4]), int.Parse(parts[5])));
        }
        var width = 101;
        var height = 103;
        var isFound = false;

        int seconds;
        for (seconds = 0; seconds < width * height && !isFound; seconds++)
        {
            var movedRobots = new List<(int x, int y, int dx, int dy)>();
            foreach (var robot in robots)
            {
                var x = (robot.x + robot.dx + width) % width;
                var y = (robot.y + robot.dy + height) % height;
                movedRobots.Add((x, y, robot.dx, robot.dy));
            }

            robots = movedRobots;

            var display = new List<char[]>();
            for (var i = 0; i < height; i++)
            {
                display.Add(new string(' ', width).ToCharArray());
            }
            foreach (var robot in robots)
            {
                display[robot.y][robot.x] = '*';
            }
            for (var i = 0; i < height && !isFound; i++)
            {
                isFound = new string(display[i]).Contains("**********");
            }
            if (isFound)
            {
                for (var i = 0; i < height; i++)
                {
                    Console.WriteLine(display[i]);
                }
            }
        }

        return seconds;
    }
}
