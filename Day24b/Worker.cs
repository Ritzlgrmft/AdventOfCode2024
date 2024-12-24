using System.Globalization;
using System.Numerics;
using System.Text;

namespace AdventOfCode2024.Day24b;

public class Worker : IWorker
{
    Dictionary<string, int> initialWires = [];
    List<(string in1, string op, string in2, string next)> gates = [];

    public long DoWork(string inputFile)
    {
        var readWires = true;
        foreach (var line in File.ReadLines(inputFile))
        {
            if (line.Length == 0)
            {
                readWires = false;
            }
            else if (readWires)
            {
                var lineParts = line.Split(": ");
                initialWires[lineParts[0]] = int.Parse(lineParts[1]);
            }
            else
            {
                var lineParts = line.Split(" ");
                gates.Add((lineParts[0], lineParts[1], lineParts[2], lineParts[4]));
            }
        }

        var knownWires = CalculateGates();
        var x = new string(knownWires.Keys.Where(k => k[0] == 'x').OrderDescending().Select(k => (char)(knownWires[k] + 48)).ToArray());
        var y = new string(knownWires.Keys.Where(k => k[0] == 'y').OrderDescending().Select(k => (char)(knownWires[k] + 48)).ToArray());
        var currentZ = new string(knownWires.Keys.Where(k => k[0] == 'z').OrderDescending().Select(k => (char)(knownWires[k] + 48)).ToArray());
        var targetZ = (BigInteger.Parse(x, NumberStyles.BinaryNumber) + BigInteger.Parse(y, NumberStyles.BinaryNumber)).ToBinaryString();
        Console.WriteLine($"{currentZ}");
        Console.WriteLine($"{targetZ}");

        // credits to https://www.reddit.com/r/adventofcode/comments/1hl698z/comment/m3kj0hp/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button

        // find XOR gates without connection to x, y or z
        var illegalGates = gates.Where(g => g.op == "XOR" && g.in1[0] != 'x' && g.in1[0] != 'y' && g.in2[0] != 'x' && g.in2[0] != 'y' && g.next[0] != 'z').ToList();

        List<string> swappedGates = [];
        foreach (var (in1, op, in2, next) in illegalGates)
        {
            var zGate = FirstZGateDependingOn(next);
            var previousZGate = "z" + (int.Parse(zGate[1..]) - 1).ToString().PadLeft(2, '0');
            SwapGates(next, previousZGate);
            swappedGates.Add(next);
            swappedGates.Add(previousZGate);
        }

        knownWires = CalculateGates();
        currentZ = new string(knownWires.Keys.Where(k => k[0] == 'z').OrderDescending().Select(k => (char)(knownWires[k] + 48)).ToArray());
        Console.WriteLine($"{currentZ}");
        Console.WriteLine($"{targetZ}");

        var comparison = (BigInteger.Parse(currentZ, NumberStyles.BinaryNumber) ^ BigInteger.Parse(targetZ, NumberStyles.BinaryNumber)).ToBinaryString();
        var n = 0;
        for (n = 0; comparison[comparison.Length - n - 1] == '0'; n++) { }

        var finalGates = gates.Where(g => g.in1[1..] == n.ToString() && g.in2[1..] == n.ToString()).Select(g => g.next).ToList();
        SwapGates(finalGates[0], finalGates[1]);
        swappedGates.AddRange(finalGates);

        knownWires = CalculateGates();
        currentZ = new string(knownWires.Keys.Where(k => k[0] == 'z').OrderDescending().Select(k => (char)(knownWires[k] + 48)).ToArray());
        Console.WriteLine($"{currentZ}");
        Console.WriteLine($"{targetZ}");

        Console.WriteLine(string.Join(",", swappedGates.Order()));
        return 0;
    }

    private Dictionary<string, int> CalculateGates()
    {
        var knownWires = initialWires.Select(w => w).ToDictionary(w => w.Key, w => w.Value);
        var unknownWires = gates.Select(g => g.next).Except(knownWires.Keys).ToList();
        while (unknownWires.Any())
        {
            foreach (var wire in unknownWires)
            {
                var gate = gates.First(g => g.next == wire);
                if (knownWires.ContainsKey(gate.in1) && knownWires.ContainsKey(gate.in2))
                {
                    knownWires[wire] = CalculateValue(knownWires, gate.in1, gate.op, gate.in2);
                    //gates.Remove(gate);
                }
            }
            unknownWires = gates.Select(g => g.next).Except(knownWires.Keys).ToList();
        }

        return knownWires;
    }

    private void SwapGates(string gate1, string gate2)
    {

        var g1 = gates.First(g => g.next == gate1);
        var g2 = gates.First(g => g.next == gate2);
        gates.Remove(g1);
        gates.Remove(g2);
        gates.Add((g1.in1, g1.op, g1.in2, g2.next));
        gates.Add((g2.in1, g2.op, g2.in2, g1.next));
    }

    private string FirstZGateDependingOn(string input)
    {
        List<string> gatesToCheck = [input];
        while (gatesToCheck.Count > 0)
        {
            List<string> nextGatesToCheck = [];
            foreach (var gateToCheck in gatesToCheck)
            {
                if (gateToCheck[0] == 'z')
                {
                    return gateToCheck;
                }
                foreach (var gate in gates.Where(g => g.in1 == gateToCheck || g.in2 == gateToCheck))
                {
                    nextGatesToCheck.Add(gate.next);
                }
            }
            gatesToCheck = nextGatesToCheck.Order().ToList();
        }
        throw new Exception();
    }

    private int CalculateValue(Dictionary<string, int> knownWires, string in1, string op, string in2)
    {
        switch (op)
        {
            case "AND":
                return knownWires[in1] & knownWires[in2];
            case "OR":
                return knownWires[in1] | knownWires[in2];
            case "XOR":
                return knownWires[in1] ^ knownWires[in2];
            default:
                throw new NotImplementedException();
        }
    }
}

public static class BigIntegerExtensions
{
    /// <summary>
    /// Converts a <see cref="BigInteger"/> to a binary string.
    /// </summary>
    /// <param name="bigint">A <see cref="BigInteger"/>.</param>
    /// <returns>
    /// A <see cref="System.String"/> containing a binary
    /// representation of the supplied <see cref="BigInteger"/>.
    /// </returns>
    public static string ToBinaryString(this BigInteger bigint)
    {
        var bytes = bigint.ToByteArray();
        var idx = bytes.Length - 1;

        // Create a StringBuilder having appropriate capacity.
        var base2 = new StringBuilder(bytes.Length * 8);

        // Convert first byte to binary.
        var binary = Convert.ToString(bytes[idx], 2);

        // Ensure leading zero exists if value is positive.
        if (binary[0] != '0' && bigint.Sign == 1)
        {
            base2.Append('0');
        }

        // Append binary string to StringBuilder.
        base2.Append(binary);

        // Convert remaining bytes adding leading zeros.
        for (idx--; idx >= 0; idx--)
        {
            base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
        }

        return base2.ToString();
    }
}