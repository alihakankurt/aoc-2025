using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Lobby
{
    static void Main()
    {
        List<string> input = [];
        while (Console.ReadLine() is string line && !string.IsNullOrEmpty(line))
        {
            input.Add(line);
        }

        ReadOnlySpan<string> banks = CollectionsMarshal.AsSpan(input);

        Console.WriteLine($"Part1: {Lobby.Part1(banks)}");
        Console.WriteLine($"Part2: {Lobby.Part2(banks)}");
    }

    static long Part1(ReadOnlySpan<string> banks)
    {
        long joltageSum = 0;

        foreach (ReadOnlySpan<char> bank in banks)
        {
            int ones = bank[^1] - '0';
            int tens = bank[^2] - '0';
            for (int index = bank.Length - 3; index >= 0; --index)
            {
                int digit = bank[index] - '0';
                if (digit >= tens)
                {
                    ones = int.Max(ones, tens);
                    tens = digit;
                }
            }

            long joltage = tens * 10 + ones;
            joltageSum += joltage;
        }

        return joltageSum;
    }

    static long Part2(ReadOnlySpan<string> banks)
    {
        long joltageSum = 0;

        Span<int> currentNumber = stackalloc int[12];
        Span<int> nextNumber = stackalloc int[12];
        foreach (ReadOnlySpan<char> bank in banks)
        {
            for (int place = 0; place < 12; ++place)
            {
                currentNumber[place] = bank[^(12 - place)] - '0';
            }

            for (int index = bank.Length - 13; index >= 0; --index)
            {
                int digit = bank[index] - '0';

                (nextNumber[0], bool update) = (digit >= currentNumber[0])
                    ? (digit, true)
                    : (currentNumber[0], false);

                for (int place = 1; place < 12; ++place)
                {
                    if (update && currentNumber[place - 1] >= currentNumber[place])
                    {
                        nextNumber[place] = currentNumber[place - 1];
                        continue;
                    }

                    nextNumber[place] = currentNumber[place];
                    update = false;
                }

                nextNumber.CopyTo(currentNumber);
            }

            long joltage = 0;
            for (int place = 0; place < 12; ++place)
            {
                joltage = joltage * 10 + currentNumber[place];
            }

            joltageSum += joltage;
        }

        return joltageSum;
    }
}
