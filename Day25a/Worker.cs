namespace AdventOfCode2024.Day25a;

public class Worker : IWorker
{
    Dictionary<string, int> knownWires = [];
    List<string> unknownWires = [];
    List<(string in1, string op, string in2, string next)> gates = [];

    public long DoWork(string inputFile)
    {
        var schema = new List<string>();
        var schematics = new List<List<string>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            if (line.Length == 0)
            {
                schematics.Add(schema);
                schema = [];
            }
            else
            {
                schema.Add(line);
            }
        }
        schematics.Add(schema);

        var locks = new List<int[]>();
        var keys = new List<int[]>();
        foreach (var schematic in schematics)
        {
            if (schematic[0] == "#####")
            {
                int[] _lock = [0, 0, 0, 0, 0];
                for (var i = 1; i < schematic.Count; i++)
                {
                    for (var j = 0; j < 5; j++)
                    {
                        if (schematic[i][j] == '#')
                        {
                            _lock[j]++;
                        }
                    }
                }
                locks.Add(_lock);
            }
            else if (schematic[schematic.Count - 1] == "#####")
            {
                int[] _key = [0, 0, 0, 0, 0];
                for (var i = schematic.Count - 2; i >= 0; i--)
                {
                    for (var j = 0; j < 5; j++)
                    {
                        if (schematic[i][j] == '#')
                        {
                            _key[j]++;
                        }
                    }
                }
                keys.Add(_key);
            }

        }

        var fit = 0;
        foreach (var _lock in locks)
        {
            foreach (var _key in keys)
            {
                var isMatch = true;
                for (var i = 0; i < 5; i++)
                {
                    if (_lock[i] + _key[i] > 5)
                    {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch)
                {
                    fit++;
                }
            }
        }

        return fit;
    }


}
