using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day4a;

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
                count += CountWord(0, x, y, 1, 0) + CountWord(0, x, y, -1, 0)
                + CountWord(0, x, y, -1, 1) + CountWord(0, x, y, 0, 1) + CountWord(0, x, y, 1, 1)
                + CountWord(0, x, y, -1, -1) + CountWord(0, x, y, 0, -1) + CountWord(0, x, y, 1, -1);
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

    private int CountWord(int index, int x, int y, int deltaX, int deltaY)
    {
        if (GetLetter(x, y) != word[index])
        {
            return 0;
        }
        else if (index == word.Length - 1)
        {
            return 1;
        }
        else
        {
            return CountWord(index + 1, x + deltaX, y + deltaY, deltaX, deltaY);
        }
    }
}
