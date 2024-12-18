namespace AdventOfCode2024.Day4b;

public class Worker : IWorker
{
    List<char[]> letters = new List<char[]>();
    char[] word = "XMAS".ToCharArray();

    public long DoWork(string inputFile)
    {
        foreach (var line in File.ReadLines(inputFile))
        {
            letters.Add(line.ToCharArray());
        }

        var count = 0;
        for (var y = 0; y < letters.Count; y++)
        {
            for (var x = 0; x < letters[y].Length; x++)
            {
                if (GetLetter(x, y) == 'A')
                {
                    if (
                         ((GetLetter(x - 1, y - 1) == 'M' && GetLetter(x + 1, y + 1) == 'S') || (GetLetter(x - 1, y - 1) == 'S' && GetLetter(x + 1, y + 1) == 'M'))
                         && ((GetLetter(x - 1, y + 1) == 'M' && GetLetter(x + 1, y - 1) == 'S') || (GetLetter(x - 1, y + 1) == 'S' && GetLetter(x + 1, y - 1) == 'M'))
                    )
                    {
                        count++;
                    }
                }

            }
        }
        return count;
    }

    char GetLetter(int x, int y)
    {
        if (y < 0 || y >= letters.Count || x < 0 || x >= letters[y].Length)
        {
            return '-';
        }
        else
        {
            return letters[y][x];
        }
    }
}
