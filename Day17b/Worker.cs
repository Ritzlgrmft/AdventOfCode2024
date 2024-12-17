using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day17b;

public class Worker : IWorker
{
    long a, b, c;
    public long DoWork(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        b = long.Parse(lines[1].Split(": ")[1]);
        c = long.Parse(lines[2].Split(": ")[1]);
        var programComplete = lines[4].Split(' ')[1];
        var program = programComplete.Split(',').Select(n => long.Parse(n)).ToArray();

        long i = 0;
        while (i < long.MaxValue)
        {
            a = i;
            var instructionPointer = 0;
            var output = new List<long>();
            while (instructionPointer < program.Length)
            {
                var operand = program[instructionPointer + 1];
                switch (program[instructionPointer])
                {
                    case 0:
                        // adv
                        a = (long)Math.Truncate(a / Math.Pow(2, GetComboOperand(operand)));
                        instructionPointer += 2;
                        break;
                    case 1:
                        // bxl
                        b ^= operand;
                        instructionPointer += 2;
                        break;
                    case 2:
                        // bst
                        b = GetComboOperand(operand) % 8;
                        instructionPointer += 2;
                        break;
                    case 3:
                        // jnz
                        if (a == 0)
                        {
                            instructionPointer += 2;
                        }
                        else
                        {
                            instructionPointer = (int)operand;
                        }
                        break;
                    case 4:
                        // bxc
                        b ^= c;
                        instructionPointer += 2;
                        break;
                    case 5:
                        // out
                        output.Add(GetComboOperand(operand) % 8);
                        instructionPointer += 2;
                        break;
                    case 6:
                        // bdv
                        b = (long)Math.Truncate(a / Math.Pow(2, GetComboOperand(operand)));
                        instructionPointer += 2;
                        break;
                    case 7:
                        // cdv
                        c = (long)Math.Truncate(a / Math.Pow(2, GetComboOperand(operand)));
                        instructionPointer += 2;
                        break;
                }
            }

            // each iteration outputs only based on the lowest 3 bits, and divides a by 8 after each iteration so the lowest 3 bits are thrown out. 
            // thus each iteration is completely independent. so i loop, starting with a = 0, incrementing by one each time until i find an a that outputs the last instruction.
            // then i multiply it by 8 and iterate starting with that value until i get the last two instructions. i repeat this process until i get the entire program.

            // true if output is identical to the end of program
            var match = program.TakeLast(output.Count).Zip(output, (p, o) => p == o).All(b => b);
            if (match && output.Count == program.Length)
            {
                break;
            }

            i = match && i > 0 ? i * 8 : i + 1;
        }
        return i;
    }

    long GetComboOperand(long operand)
    {
        return operand switch
        {
            4 => a,
            5 => b,
            6 => c,
            7 => throw new NotImplementedException(),
            _ => operand,
        };
    }

}
