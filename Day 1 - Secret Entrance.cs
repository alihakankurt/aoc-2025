using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class SecretEntrance
{
    const int DialStartPosition = 50;
    const int DialPositionCount = 100;

    static void Main()
    {
        List<string> input = [];
        while (Console.ReadLine() is string line && !string.IsNullOrEmpty(line))
        {
            input.Add(line);
        }

        ReadOnlySpan<string> instructions = CollectionsMarshal.AsSpan(input);

        Console.WriteLine($"Part1: {SecretEntrance.Part1(instructions)}");
        Console.WriteLine($"Part2: {SecretEntrance.Part2(instructions)}");
    }

    static int Part1(ReadOnlySpan<string> instructions)
    {
        int password = 0;
        int position = DialStartPosition;

        foreach (ReadOnlySpan<char> instruction in instructions)
        {
            int direction = (instruction[0] == 'L') ? -1 : 1;
            int move = int.Parse(instruction[1..]);

            move %= DialPositionCount;
            position += move * direction;
            position = (position + DialPositionCount) % DialPositionCount;

            if (position == 0)
            {
                ++password;
            }
        }

        return password;
    }

    static int Part2(ReadOnlySpan<string> instructions)
    {
        int password = 0;
        int position = DialStartPosition;

        foreach (ReadOnlySpan<char> instruction in instructions)
        {
            int direction = (instruction[0] == 'L') ? -1 : 1;
            int move = int.Parse(instruction[1..]);

            var (turns, remaining) = int.DivRem(move, DialPositionCount);
            password += turns;

            position += remaining * direction;
            if (0 > position || position >= DialPositionCount)
            {
                if (position > -remaining)
                {
                    ++password;
                }

                position = (position + DialPositionCount) % DialPositionCount;
            }

            if (position == 0 && direction == -1 && remaining != 0)
            {
                ++password;
            }
        }

        return password;
    }
}
