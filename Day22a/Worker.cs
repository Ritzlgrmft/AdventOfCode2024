namespace AdventOfCode2024.Day22a;

public class Worker : IWorker
{

    public long DoWork(string inputFile)
    {
        var sum = 0L;
        foreach (var line in File.ReadLines(inputFile))
        {
            var secretNumber = CalculateSecretNumber(long.Parse(line), 2000);
            Console.WriteLine($"{line}: {secretNumber}");
            sum += secretNumber;
        }

        return sum;
    }

    long CalculateSecretNumber(long previousSecretNumber, int iterations)
    {
        var result = previousSecretNumber;
        for (int i = 0; i < iterations; i++)
        {
            result = CalculateSecretNumber(result);
        }
        return result;
    }

    long CalculateSecretNumber(long previousSecretNumber)
    {
        var result = MixAndPrune(previousSecretNumber, previousSecretNumber * 64);
        result = MixAndPrune(result, result / 32);
        result = MixAndPrune(result, result * 2048);
        return result;
    }

    long MixAndPrune(long previousSecretNumber, long value)
    {
        var result = previousSecretNumber ^ value;
        result = result % 16777216L;
        return result;
    }
}
