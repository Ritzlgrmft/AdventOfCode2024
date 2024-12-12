using System.ComponentModel;
using System.Data;

namespace AdventOfCode2024.Day12b;

public class Worker : IWorker
{
    List<string> map;
    List<(char plant, List<(int x, int y)> positions)> regions = [];

    public long DoWork(string inputFile)
    {
        // read map
        map = File.ReadAllLines(inputFile).Select(l => " " + l + " ").ToList();
        var emptyRow = new string(' ', map[0].Length);
        map.Insert(0, emptyRow);
        map.Add(emptyRow);

        // find regions
        for (var x = 1; x < map[0].Length - 1; x++)
        {
            for (var y = 1; y < map.Count - 1; y++)
            {
                if (FindRegionId(x, y) == -1)
                {
                    var region = (GetField(x, y), new List<(int x, int y)>() { (x, y) });
                    TryToExtendRegion(region, x + 1, y);
                    TryToExtendRegion(region, x - 1, y);
                    TryToExtendRegion(region, x, y + 1);
                    TryToExtendRegion(region, x, y - 1);
                    regions.Add(region);
                }
            }
        }

        // calculate price
        var price = 0L;
        foreach (var region in regions)
        {
            var area = region.positions.Count;
            var sides = CalculateSides(region.positions);
            price += area * sides;
        }

        return price;
    }

    private char GetField(int x, int y)
    {
        return map[y][x];
    }

    private int FindRegionId(int x, int y)
    {
        for (var r = 0; r < regions.Count; r++)
        {
            var region = regions[r];
            if (region.positions.Contains((x, y)))
            {
                return r;
            }
        }
        return -1;
    }

    private void TryToExtendRegion((char plant, List<(int x, int y)> positions) region, int x, int y)
    {
        if (GetField(x, y) == region.plant && !region.positions.Contains((x, y)))
        {
            region.positions.Add((x, y));
            TryToExtendRegion(region, x + 1, y);
            TryToExtendRegion(region, x - 1, y);
            TryToExtendRegion(region, x, y + 1);
            TryToExtendRegion(region, x, y - 1);
        }
    }

    private int CalculateSides(List<(int x, int y)> positions)
    {
        var bounds = new List<(int x, int y, char direction)>();
        foreach (var pos in positions)
        {
            if (!positions.Contains((pos.x + 1, pos.y)))
            {
                bounds.Add((pos.x, pos.y, 'R'));
            }
            if (!positions.Contains((pos.x - 1, pos.y)))
            {
                bounds.Add((pos.x, pos.y, 'L'));
            }
            if (!positions.Contains((pos.x, pos.y + 1)))
            {
                bounds.Add((pos.x, pos.y, 'D'));
            }
            if (!positions.Contains((pos.x, pos.y - 1)))
            {
                bounds.Add((pos.x, pos.y, 'U'));
            }
        }

        var sides = 0;
        while (bounds.Count > 0)
        {
            var boundsToRemove = new List<(int x, int y, char direction)>() { bounds[0] };
            for (var i = 1; bounds.Contains((bounds[0].x + i, bounds[0].y, bounds[0].direction)); i++)
            {
                boundsToRemove.Add((bounds[0].x + i, bounds[0].y, bounds[0].direction));
            }
            for (var i = 1; bounds.Contains((bounds[0].x - i, bounds[0].y, bounds[0].direction)); i++)
            {
                boundsToRemove.Add((bounds[0].x - i, bounds[0].y, bounds[0].direction));
            }
            for (var i = 1; bounds.Contains((bounds[0].x, bounds[0].y + i, bounds[0].direction)); i++)
            {
                boundsToRemove.Add((bounds[0].x, bounds[0].y + i, bounds[0].direction));
            }
            for (var i = 1; bounds.Contains((bounds[0].x, bounds[0].y - i, bounds[0].direction)); i++)
            {
                boundsToRemove.Add((bounds[0].x, bounds[0].y - i, bounds[0].direction));
            }
            sides++;
            bounds = bounds.Except(boundsToRemove).ToList();
        }
        return sides;
    }



}
