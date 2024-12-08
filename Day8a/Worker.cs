using System.Data;

namespace AdventOfCode2024.Day8a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        // read input
        var antennas = new Dictionary<char, List<(int x, int y)>>();
        var maxX = 0;
        var maxY = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            for (var x = 0; x < line.Length; x++)
            {
                var antenna = line[x];
                if (antenna != '.')
                {
                    if (!antennas.ContainsKey(antenna))
                    {
                        antennas[antenna] = [];
                    }
                    antennas[antenna].Add((x, maxY));
                }
            }
            maxX = line.Length;
            maxY++;
        }

        // find antinodes
        var antinodes = new List<(int x, int y)>();
        foreach (var antenna in antennas.Keys)
        {
            AddAntinodes(antinodes, antennas[antenna], maxX, maxY);
        }

        return antinodes.Count;
    }

    private void AddAntinodes(List<(int x, int y)> antinodes, List<(int x, int y)> antennas, int maxX, int maxY)
    {
        if (antennas.Count < 2)
        {
            return;
        }
        for (var a = 1; a < antennas.Count; a++)
        {
            AddAntinodes(antinodes, antennas[0], antennas[a], maxX, maxY);
        }
        AddAntinodes(antinodes, antennas.Skip(1).ToList(), maxX, maxY);
    }

    private void AddAntinodes(List<(int x, int y)> antinodes, (int x, int y) antenna1, (int x, int y) antenna2, int maxX, int maxY)
    {
        var deltaX = antenna2.x - antenna1.x;
        var x1 = antenna1.x - deltaX;
        var x2 = antenna2.x + deltaX;
        var deltaY = antenna2.y - antenna1.y;
        var y1 = antenna1.y - deltaY;
        var y2 = antenna2.y + deltaY;

        if (x1 >= 0 && x1 < maxX && y1 >= 0 && y1 < maxY)
        {
            if (!antinodes.Contains((x1, y1)))
            {
                antinodes.Add((x1, y1));
            }
        }
        if (x2 >= 0 && x2 < maxX && y2 >= 0 && y2 < maxY)
        {
            if (!antinodes.Contains((x2, y2)))
            {
                antinodes.Add((x2, y2));
            }
        }
    }
}
