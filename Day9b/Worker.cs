namespace AdventOfCode2024.Day9b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var input = File.ReadAllText(inputFile).ToCharArray();

        // read disk
        var disk = new List<(int id, int length)>();
        var id = 0;
        var isFile = true;
        foreach (var data in input)
        {
            var number = data - '0';
            if (isFile)
            {
                if (number > 0)
                {
                    disk.Add((id, number));
                }
                isFile = false;
                id++;
            }
            else
            {
                if (number > 0)
                {
                    disk.Add((-1, number));
                }
                isFile = true;
            }
        }

        // compact disk
        var fileToCheck = disk.Count - 1;
        if (disk[fileToCheck].id == -1)
        {
            fileToCheck--;
        }
        while (fileToCheck > 0)
        {
            var isFileMoved = false;
            for (var i = 1; i < fileToCheck && !isFileMoved; i++)
            {
                if (disk[i].id == -1 && disk[i].length >= disk[fileToCheck].length)
                {
                    var remaininingSpace = disk[i].length - disk[fileToCheck].length;
                    disk[i] = disk[fileToCheck];
                    disk[fileToCheck] = (-1, disk[fileToCheck].length);
                    if (remaininingSpace > 0)
                    {
                        disk.Insert(i + 1, (-1, remaininingSpace));
                        fileToCheck++;
                    }
                    isFileMoved = true;
                }
            }
            fileToCheck--;
            while (disk[fileToCheck].id == -1)
            {
                fileToCheck--;
            }
        }

        // calculate checksum
        var sum = 0L;
        var index = 0;
        foreach (var block in disk)
        {
            if (block.id != -1)
            {
                for (var i = 0; i < block.length; i++)
                {
                    sum += (index + i) * block.id;
                }
            }
            index += block.length;
        }
        return sum;
    }
}
