using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AdventOfCode2024.Day14a;

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
        var seconds = 100;

        var movedRobots = new List<(int x, int y)>();
        foreach (var robot in robots)
        {
            var x = (robot.x + seconds * robot.dx + seconds * width) % width;
            var y = (robot.y + seconds * robot.dy + seconds * height) % height;
            movedRobots.Add((x, y));
        }

        var lt = movedRobots.Count(r => r.x < (width - 1) / 2 && r.y < (height - 1) / 2);
        var rt = movedRobots.Count(r => r.x > (width - 1) / 2 && r.y < (height - 1) / 2);
        var lb = movedRobots.Count(r => r.x < (width - 1) / 2 && r.y > (height - 1) / 2);
        var rb = movedRobots.Count(r => r.x > (width - 1) / 2 && r.y > (height - 1) / 2);
        return lt * rt * lb * rb;
    }
}
