using System.Diagnostics;
using AdventOfCode2024;

if (args.Length < 2)
{
    Console.Error.WriteLine("Invalid args, day and part expected");
    return;
}

var day = args[0];
var part = args[1];
var fileName = args.Length == 3 ? "TestInput.txt" : "Input.txt";
var inputFile = $"../../../Day{day}{part}/{fileName}";

var workerType = Type.GetType($"AdventOfCode2024.Day{day}{part}.Worker");
if (workerType == null)
{
    Console.Error.WriteLine("Invalid values for day and part");
    return;
}
var worker = Activator.CreateInstance(workerType) as IWorker;
if (worker == null)
{
    Console.Error.WriteLine("Worker cannot be instantiated");
    return;
}

Console.WriteLine($"Calling {workerType} with {fileName}");
var stopwatch = Stopwatch.StartNew();
var result = worker.DoWork(inputFile);
Console.WriteLine("{0}, {1}", result, stopwatch.Elapsed);