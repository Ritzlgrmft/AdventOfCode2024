using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day7b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var results = new List<long>();
        var numbers = new List<long[]>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var parts = line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries).Select(p => long.Parse(p));
            results.Add(parts.First());
            numbers.Add(parts.Skip(1).ToArray());
        }

        var sum = 0L;
        for (var i = 0; i < results.Count; i++)
        {
            if (Calculate(numbers[i].First(), numbers[i].Skip(1)).Contains(results[i]))
            {
                sum += results[i];
            }
        }
        return sum;
    }

    IEnumerable<long> Calculate(long first, IEnumerable<long> rest)
    {
        if (!rest.Any())
        {
            yield return first;
        }
        else
        {
            foreach (var result in Calculate(first + rest.First(), rest.Skip(1)))
            {
                yield return result;
            }
            foreach (var result in Calculate(first * rest.First(), rest.Skip(1)))
            {
                yield return result;
            }
            foreach (var result in Calculate(long.Parse(first.ToString() + rest.First().ToString()), rest.Skip(1)))
            {
                yield return result;
            }
        }
    }

}
