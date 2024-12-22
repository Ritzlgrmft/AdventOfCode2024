namespace AdventOfCode2024.Day22b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var prices = new List<List<(int price, int change, String changes)>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            var pricesPerBuyer = new List<(int price, int change, String changes)>();
            var secretNumber = long.Parse(line);
            int price = int.Parse(line.Substring(line.Length - 1));
            pricesPerBuyer.Add((price, 0, ""));
            for (int i = 0; i < 2000; i++)
            {
                var nextSecretNumber = CalculateSecretNumber(secretNumber);
                var nextPrice = int.Parse(nextSecretNumber.ToString().Substring(nextSecretNumber.ToString().Length - 1));
                var nextChange = nextPrice - price;
                var nextChanges = i < 3 ? "" : $"{pricesPerBuyer[i - 2].change},{pricesPerBuyer[i - 1].change},{pricesPerBuyer[i].change},{nextChange}";
                pricesPerBuyer.Add((nextPrice, nextChange, nextChanges));

                secretNumber = nextSecretNumber;
                price = nextPrice;
            }
            prices.Add(pricesPerBuyer);
        }

        var allChanges = new List<string>();
        foreach (var pricesPerBuyer in prices)
        {
            allChanges = allChanges.Union(pricesPerBuyer.Select(p => p.changes).Where(c => c.Length > 0)).ToList();
        }

        var bestPrice = 0;
        foreach (var changes in allChanges)
        {
            var pricePerChange = 0;
            foreach (var pricesPerBuyer in prices)
            {
                pricePerChange += pricesPerBuyer.FirstOrDefault(p => p.changes == changes).price;
            }
            if (pricePerChange > bestPrice)
            {
                bestPrice = pricePerChange;
            }
        }

        return bestPrice;
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
