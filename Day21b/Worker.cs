namespace AdventOfCode2024.Day21b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var sum = 0L;
        foreach (var line in File.ReadLines(inputFile))
        {
            var results = new List<string>();
            var minLength = CalculateNumericMoves("A" + line);

            var c1 = minLength;
            var c2 = int.Parse(new string(line.ToCharArray().Where(c => char.IsDigit(c)).ToArray()));
            sum += c1 * c2;
            Console.WriteLine("{0}: {1}", line, c1);
        }

        return sum;
    }

    private long CalculateNumericMoves(string input)
    {
        var sequences = new List<string>();
        for (var i = 0; i < input.Length - 1; i++)
        {
            var nextSequences = GetNumericSequences(input[i], input[i + 1]);
            if (i == 0)
            {
                sequences = nextSequences;
            }
            else
            {
                sequences = sequences.SelectMany(s => nextSequences, (s1, s2) => s1 + s2).ToList();
            }
        }

        var sequenceLengths = new List<long>();
        foreach (var sequence in sequences)
        {
            sequenceLengths.Add(CalculateDirectionalMoves("A" + sequence, 25));
        }
        return sequenceLengths.Min();
    }

    readonly Dictionary<(string input, int count), long> cache = [];

    private long CalculateDirectionalMoves(string input, int count)
    {
        var length = 0L;
        foreach (var inputPart in input.Split('A').Skip(1).SkipLast(1))
        {
            length += CalculateDirectionalMoves2("A" + inputPart + "A", count);
        }
        return length;
    }

    private long CalculateDirectionalMoves2(string input, int count)
    {
        if (cache.TryGetValue((input, count), out long value))
        {
            return value;
        }

        var sequences = new List<string>();
        for (var i = 0; i < input.Length - 1; i++)
        {
            var nextSequences = GetDirectionalSequences(input[i], input[i + 1]);
            if (i == 0)
            {
                sequences = nextSequences;
            }
            else
            {
                sequences = sequences.SelectMany(s => nextSequences, (s1, s2) => s1 + s2).ToList();
            }
        }
        var minLength = (long)sequences.Min(s => s.Length);

        if (count > 1)
        {
            minLength = sequences.Select(s => CalculateDirectionalMoves("A" + s, count - 1)).Min();
        }

        cache[(input, count)] = minLength;
        return minLength;
    }

    //     +---+---+
    //     | ^ | A |
    // +---+---+---+
    // | < | v | > |
    // +---+---+---+
    Dictionary<char, (int x, int y)> directionalPad = new(){
    {' ',(0,0)},
    {'^',(1,0)},
    {'A',(2,0)},
    {'<',(0,1)},
    {'v',(1,1)},
    {'>',(2,1)}};

    Dictionary<(char start, char end), List<string>> sequenceCache = [];

    private List<string> GetDirectionalSequences(char start, char end)
    {
        if (sequenceCache.TryGetValue((start, end), out List<string>? value))
        {
            return value;
        }

        var startPos = directionalPad[start];
        var endPos = directionalPad[end];
        (char c, int count) horizontal = startPos.x < endPos.x ? ('>', endPos.x - startPos.x) : ('<', startPos.x - endPos.x);
        (char c, int count) vertical = startPos.y < endPos.y ? ('v', endPos.y - startPos.y) : ('^', startPos.y - endPos.y);
        var sequences = GenerateSequences(horizontal, vertical);

        if (end == '<')
        {
            sequences = sequences.Where(s => !s.EndsWith("vA")).ToList();
        }
        else if (start == '<')
        {
            sequences = sequences.Where(s => !s.StartsWith('^')).ToList();
        }

        sequenceCache[(start, end)] = sequences;
        return sequences;
    }

    // +---+---+---+
    // | 7 | 8 | 9 |
    // +---+---+---+
    // | 4 | 5 | 6 |
    // +---+---+---+
    // | 1 | 2 | 3 |
    // +---+---+---+
    //     | 0 | A |
    //     +---+---+
    Dictionary<char, (int x, int y)> numericPad = new(){
    {'7',(0,0)},
    {'8',(1,0)},
    {'9',(2,0)},
    {'4',(0,1)},
    {'5',(1,1)},
    {'6',(2,1)},
    {'1',(0,2)},
    {'2',(1,2)},
    {'3',(2,2)},
    {' ',(0,3)},
    {'0',(1,3)},
    {'A',(2,3)}};

    private List<string> GetNumericSequences(char start, char end)
    {
        if (sequenceCache.TryGetValue((start, end), out List<string>? value))
        {
            return value;
        }

        var startPos = numericPad[start];
        var endPos = numericPad[end];
        (char c, int count) horizontal = startPos.x < endPos.x ? ('>', endPos.x - startPos.x) : ('<', startPos.x - endPos.x);
        (char c, int count) vertical = startPos.y < endPos.y ? ('v', endPos.y - startPos.y) : ('^', startPos.y - endPos.y);
        var sequences = GenerateSequences(horizontal, vertical);

        if (end == '0')
        {
            sequences = sequences.Where(s => !s.EndsWith(">A")).ToList();
        }
        else if (end == 'A')
        {
            sequences = sequences.Where(s => !s.EndsWith(">>A")).ToList();
        }
        else if (start == '0')
        {
            sequences = sequences.Where(s => !s.StartsWith('<')).ToList();
        }
        else if (start == 'A')
        {
            sequences = sequences.Where(s => !s.StartsWith("<<")).ToList();
        }

        sequenceCache[(start, end)] = sequences;
        return sequences;
    }

    List<string> GenerateSequences((char c, int count) horizontal, (char c, int count) vertical)
    {
        var sequences = new List<string>();
        if (horizontal.count == 0)
        {
            sequences.Add(new string(vertical.c, vertical.count));
        }
        else if (vertical.count == 0)
        {
            sequences.Add(new string(horizontal.c, horizontal.count));
        }
        else
        {
            sequences.Add(new string(horizontal.c, horizontal.count) + new string(vertical.c, vertical.count));
            sequences.Add(new string(vertical.c, vertical.count) + new string(horizontal.c, horizontal.count));
        }
        return sequences.Select(s => s + "A").ToList();
    }
}