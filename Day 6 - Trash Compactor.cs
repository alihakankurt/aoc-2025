using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class TrashCompactor
{
    const int MaxGroupSize = 1024;

    static void Main()
    {
        List<string> input = [];
        while (Console.ReadLine() is string line && !string.IsNullOrEmpty(line))
        {
            input.Add(line);
        }

        ReadOnlySpan<string> inputSpan = CollectionsMarshal.AsSpan(input);
        ReadOnlySpan<string> numberGroups = inputSpan[..^1];
        ReadOnlySpan<char> operationGroup = input[^1];

        Console.WriteLine($"Part1: {TrashCompactor.Part1(numberGroups, operationGroup)}");
        Console.WriteLine($"Part2: {TrashCompactor.Part2(numberGroups, operationGroup)}");
    }

    static long Part1(ReadOnlySpan<string> numberGroups, ReadOnlySpan<char> operationGroup)
    {
        long totalAnswer = 0;
        Range[] destinationRanges = ArrayPool<Range>.Shared.Rent(MaxGroupSize);

        var numbersArray = new long[numberGroups.Length][];
        for (int groupIndex = 0; groupIndex < numberGroups.Length; ++groupIndex)
        {
            ReadOnlySpan<char> numberGroup = numberGroups[groupIndex];
            int numberCount = numberGroup.Split(destinationRanges, ' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var numbers = new long[numberCount];
            numbersArray[groupIndex] = numbers;

            for (int index = 0; index < numberCount; ++index)
            {
                numbers[index] = long.Parse(numberGroup[destinationRanges[index]]);
            }
        }

        int operationCount = operationGroup.Split(destinationRanges, ' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        for (int index = 0; index < operationCount; ++index)
        {
            long answer;
            char operation = operationGroup[destinationRanges[index]][0];

            if (operation == '*')
            {
                answer = 1;
                foreach (ReadOnlySpan<long> numbers in numbersArray)
                {
                    answer *= numbers[index];
                }
            }
            else
            {
                answer = 0;
                foreach (ReadOnlySpan<long> numbers in numbersArray)
                {
                    answer += numbers[index];
                }
            }

            totalAnswer += answer;
        }

        ArrayPool<Range>.Shared.Return(destinationRanges);
        return totalAnswer;
    }

    static long Part2(ReadOnlySpan<string> numberGroups, ReadOnlySpan<char> operationGroup)
    {
        long totalAnswer = 0;

        int lastOperationIndex = operationGroup.Length;
        for (int operationIndex = operationGroup.Length - 1; operationIndex >= 0; --operationIndex)
        {
            char operation = operationGroup[operationIndex];
            if (char.IsWhiteSpace(operation))
                continue;

            List<long> numbers = [];
            for (int index = operationIndex; index < lastOperationIndex; ++index)
            {
                int groupIndex = 0;
                while (groupIndex < numberGroups.Length)
                {
                    char ch = numberGroups[groupIndex][index];
                    if (!char.IsDigit(ch))
                    {
                        ++groupIndex;
                        continue;
                    }

                    long number = ch - '0';
                    while (++groupIndex < numberGroups.Length && char.IsDigit(ch = numberGroups[groupIndex][index]))
                    {
                        number = number * 10 + (ch - '0');
                    }

                    numbers.Add(number);
                }
            }

            long answer;
            if (operation == '*')
            {
                answer = 1;
                foreach (long number in numbers)
                {
                    answer *= number;
                }
            }
            else
            {
                answer = 0;
                foreach (long number in numbers)
                {
                    answer += number;
                }
            }

            totalAnswer += answer;
            lastOperationIndex = operationIndex;
        }

        return totalAnswer;
    }
}
