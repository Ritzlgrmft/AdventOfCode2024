using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace AdventOfCode2024.Day15a;

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
                map.Add(line.ToCharArray());
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
            var dx = 0;
            var dy = 0;
            switch (direction)
            {
                case '^':
                    dy = -1;
                    break;
                case 'v':
                    dy = 1;
                    break;
                case '>':
                    dx = 1;
                    break;
                case '<':
                    dx = -1;
                    break;
            }
            var nextCheck = (x: robot.x + dx, y: robot.y + dy);
            while (GetField(nextCheck.x, nextCheck.y) == 'O')
            {
                nextCheck = (x: nextCheck.x + dx, y: nextCheck.y + dy);
            }
            if (GetField(nextCheck.x, nextCheck.y) == '.')
            {
                while (nextCheck != robot)
                {
                    SetField(nextCheck.x, nextCheck.y, GetField(nextCheck.x - dx, nextCheck.y - dy));
                    nextCheck = (nextCheck.x - dx, nextCheck.y - dy);
                }
                SetField(robot.x, robot.y, '.');
                robot = (x: robot.x + dx, y: robot.y + dy);
            }
        }

        var sum = 0;
        for (var x = 0; x < maxX; x++)
        {
            for (var y = 0; y < maxY; y++)
            {
                if (GetField(x, y) == 'O')
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

    private void SetField(int x, int y, char field)
    {
        map[y][x] = field;
    }


}
