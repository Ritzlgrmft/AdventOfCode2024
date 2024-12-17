namespace AdventOfCode2024.Day17a;

public class Worker : IWorker
{
    uint a, b, c;
    uint[] program = [];

    public long DoWork(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        a = uint.Parse(lines[0].Split(": ")[1]);
        b = uint.Parse(lines[1].Split(": ")[1]);
        c = uint.Parse(lines[2].Split(": ")[1]);
        program = lines[4].Split([':', ','], StringSplitOptions.TrimEntries).Skip(1).Select(n => uint.Parse(n)).ToArray();

        var instructionPointer = 0;
        var output = new List<long>();
        while (instructionPointer < program.Length)
        {
            var operand = program[instructionPointer + 1];
            switch (program[instructionPointer])
            {
                case 0:
                    // adv
                    a = (uint)Math.Truncate(a / Math.Pow(2, GetComboOperand(operand)));
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
                    b = (uint)Math.Truncate(a / Math.Pow(2, GetComboOperand(operand)));
                    instructionPointer += 2;
                    break;
                case 7:
                    // cdv
                    c = (uint)Math.Truncate(a / Math.Pow(2, GetComboOperand(operand)));
                    instructionPointer += 2;
                    break;
            }

        }

        Console.WriteLine(string.Join(',', output.Select(n => n.ToString())));
        return 0;
    }

    uint GetComboOperand(uint operand)
    {
        switch (operand)
        {
            case 4:
                return a;
            case 5:
                return b;
            case 6:
                return c;
            case 7:
                throw new NotImplementedException();
            default:
                return operand;
        }
    }

}
