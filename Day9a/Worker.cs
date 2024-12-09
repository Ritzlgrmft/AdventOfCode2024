using System.Data;

namespace AdventOfCode2024.Day9a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var input = File.ReadAllText(inputFile).ToCharArray();

        // read disk
        var disk = new List<int>();
        var id = 0;
        var isFile = true;
        foreach (var data in input)
        {
            var number = data - '0';
            if (isFile)
            {
                disk.AddRange(Enumerable.Repeat(id, number));
                isFile = false;
                id++;
            }
            else
            {
                disk.AddRange(Enumerable.Repeat(-1, number));
                isFile = true;
            }
        }

        // compact disk
        var last = disk.Count - 1;
        while (disk[last] == -1)
        {
            last--;
        }
        for (var i = 1; i < last; i++)
        {
            if (disk[i] == -1)
            {
                disk[i] = disk[last];
                disk[last] = -1;
                while (disk[last] == -1)
                {
                    last--;
                }
            }
        }

        // calculate checksum
        var sum = 0L;
        for (var i = 0; disk[i] != -1; i++)
        {
            sum += i * disk[i];
        }
        return sum;
    }


}
