namespace AdventOfCode2024.Day13b;

public class Worker : IWorker
{
    List<((int dx, int dy) buttonA, (int dx, int dy) buttonB, (long x, long y) price)> machines = [];

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
            var price = (10000000000000 + long.Parse(parts[1]), 10000000000000 + long.Parse(parts[3]));
            machines.Add((buttonA, buttonB, price));
            i += 4;
        }

        var tokens = 0L;
        foreach (var (buttonA, buttonB, price) in machines)
        {
            var resultsForMachine = new List<long>();

            var px = price.x;
            var py = price.y;
            var ax = buttonA.dx;
            var ay = buttonA.dy;
            var bx = buttonB.dx;
            var by = buttonB.dy;

            // calculate determinants following cramer's rule
            var d = ax * by - ay * bx;
            var d1 = px * by - py * bx;
            var d2 = ax * py - ay * px;
            var pressA = d1 / d;
            if (d1 % d == 0 && d2 % d == 0)
            {
                tokens += 3 * d1 / d + d2 / d;
            }
        }
        return tokens;
    }
}
