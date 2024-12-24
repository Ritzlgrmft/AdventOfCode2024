namespace AdventOfCode2024.Day24a;

public class Worker : IWorker
{
    Dictionary<string, int> knownWires = [];
    List<string> unknownWires = [];
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
                knownWires[lineParts[0]] = int.Parse(lineParts[1]);
            }
            else
            {
                var lineParts = line.Split(" ");
                gates.Add((lineParts[0], lineParts[1], lineParts[2], lineParts[4]));
            }
        }

        unknownWires = gates.Select(g => g.next).Except(knownWires.Keys).ToList();
        while (unknownWires.Any())
        {
            foreach (var wire in unknownWires)
            {
                var gate = gates.First(g => g.next == wire);
                if (knownWires.ContainsKey(gate.in1) && knownWires.ContainsKey(gate.in2))
                {
                    knownWires[wire] = CalculateValue(gate.in1, gate.op, gate.in2);
                    gates.Remove(gate);
                }
            }
            unknownWires = gates.Select(g => g.next).Except(knownWires.Keys).ToList();
        }

        var binary = new string(knownWires.Keys.Where(k => k[0] == 'z').OrderDescending().Select(k => (char)(knownWires[k] + 48)).ToArray());
        return Convert.ToInt64(binary, 2);
    }

    private int CalculateValue(string in1, string op, string in2)
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
