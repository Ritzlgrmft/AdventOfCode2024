using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AdventOfCode2024.Day13a;

public class Worker : IWorker
{
    List<((int dx, int dy) buttonA, (int dx, int dy) buttonB, (int x, int y) price)> machines = [];

    public long DoWork(string inputFile)
    {

        var lines = File.ReadLines(inputFile).ToList();
        var i = 0;
        while (i < lines.Count)
        {
            var parts = lines[i].Split(['+', ',']);
            var buttonA = (int.Parse(parts[1]), int.Parse(parts[3]));
            parts = lines[i + 1].Split(['+', ',']);
            var buttonB = (int.Parse(parts[1]), int.Parse(parts[3]));
            parts = lines[i + 2].Split(['=', ',']);
            var price = (int.Parse(parts[1]), int.Parse(parts[3]));
            machines.Add((buttonA, buttonB, price));
            i += 4;
        }

        var tokens = 0;
        foreach (var machine in machines)
        {
            var resultsForMachine = new List<int>();
            var minA = Math.Max(
                Math.Max(0, (machine.price.x - 100 * machine.buttonB.dx) / machine.buttonA.dx),
                Math.Max(0, (machine.price.y - 100 * machine.buttonB.dy) / machine.buttonA.dy));
            var maxA = new List<int>() { machine.price.x / machine.buttonA.dx, machine.price.y / machine.buttonA.dy, 100 }.Min();
            for (var a = minA; a <= maxA; a++)
            {
                if ((machine.price.x - a * machine.buttonA.dx) / machine.buttonB.dx == (machine.price.y - a * machine.buttonA.dy) / machine.buttonB.dy &&
                    (machine.price.x - a * machine.buttonA.dx) % machine.buttonB.dx == 0)
                {
                    var b = (machine.price.x - a * machine.buttonA.dx) / machine.buttonB.dx;
                    if (b <= 100)
                    {
                        resultsForMachine.Add(a * 3 + b);
                    }
                }
            }
            if (resultsForMachine.Any())
            {
                tokens += resultsForMachine.Min();
            }
        }
        return tokens;
    }
}
