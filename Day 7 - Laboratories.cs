using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Laboratories
{
    const char StartLocationMark = 'S';
    const char EmptySpaceMark = '.';
    const char SplitterMark = '^';

    static void Main()
    {
        List<string> input = [];
        while (Console.ReadLine() is string line && !string.IsNullOrEmpty(line))
        {
            input.Add(line);
        }

        ReadOnlySpan<string> diagram = CollectionsMarshal.AsSpan(input);

        Console.WriteLine($"Part1: {Laboratories.Part1(diagram)}");
        Console.WriteLine($"Part2: {Laboratories.Part2(diagram)}");
    }

    static ulong Part1(ReadOnlySpan<string> diagram)
    {
        var beams = new HashSet<int>();

        beams.Add(diagram[0].IndexOf(StartLocationMark));

        ulong splitCount = 0;
        foreach (ReadOnlySpan<char> layer in diagram[1..])
        {
            var newBeams = new HashSet<int>();
            foreach (int beam in beams)
            {
                if (layer[beam] == SplitterMark)
                {
                    newBeams.Add(beam - 1);
                    newBeams.Add(beam + 1);
                    ++splitCount;
                }
                else
                {
                    newBeams.Add(beam);
                }
            }

            beams = newBeams;
        }

        return splitCount;
    }

    static ulong Part2(ReadOnlySpan<string> diagram)
    {
        var beams = new Dictionary<int, ulong>();
        beams.Add(diagram[0].IndexOf(StartLocationMark), 1);

        foreach (ReadOnlySpan<char> layer in diagram[1..])
        {
            var newBeams = new Dictionary<int, ulong>();
            foreach (var (beam, timeline) in beams)
            {
                if (layer[beam] == SplitterMark)
                {
                    _ = newBeams.TryGetValue(beam - 1, out var left);
                    newBeams[beam - 1] = left + timeline;

                    _ = newBeams.TryGetValue(beam + 1, out var right);
                    newBeams[beam + 1] = right + timeline;
                }
                else
                {
                    _ = newBeams.TryGetValue(beam, out var current);
                    newBeams[beam] = current + timeline;
                }
            }

            beams = newBeams;
        }

        ulong timelines = 0;
        foreach (var timeline in beams.Values)
        {
            timelines += timeline;
        }

        return timelines;
    }
}
