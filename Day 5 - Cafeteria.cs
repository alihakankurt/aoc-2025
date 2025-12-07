using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Cafeteria
{
    static void Main()
    {
        List<IngredientRange> freshIngredientRangeList = [];
        Span<Range> buffer = stackalloc Range[2];
        while (Console.ReadLine() is string line && !string.IsNullOrEmpty(line))
        {
            ReadOnlySpan<char> chars = line;
            ArgumentOutOfRangeException.ThrowIfNotEqual(chars.Split(buffer, '-'), 2);

            freshIngredientRangeList.Add(new IngredientRange
            {
                FirstId = IngredientId.Parse(chars[buffer[0]]),
                LastId = IngredientId.Parse(chars[buffer[1]]),
            });
        }

        freshIngredientRangeList.Sort();

        int mergeIndex = 1;
        while (mergeIndex < freshIngredientRangeList.Count)
        {
            IngredientRange previous = freshIngredientRangeList[mergeIndex - 1];
            IngredientRange current = freshIngredientRangeList[mergeIndex];

            bool intersects = current.FirstId <= previous.LastId;
            bool contains = current.LastId <= previous.LastId;

            if (!intersects && !contains)
            {
                ++mergeIndex;
                continue;
            }

            if (intersects && !contains)
            {
                freshIngredientRangeList[mergeIndex - 1] = new IngredientRange
                {
                    FirstId = previous.FirstId,
                    LastId = current.LastId,
                };
            }

            freshIngredientRangeList.RemoveAt(mergeIndex);
        }

        List<IngredientId> availableIngredientIdList = [];
        while (Console.ReadLine() is string line && !string.IsNullOrEmpty(line))
        {
            availableIngredientIdList.Add(IngredientId.Parse(line));
        }

        ReadOnlySpan<IngredientRange> freshIngredientRanges = CollectionsMarshal.AsSpan(freshIngredientRangeList);
        ReadOnlySpan<IngredientId> availableIngredientIds = CollectionsMarshal.AsSpan(availableIngredientIdList);

        Console.WriteLine($"Part1: {Cafeteria.Part1(freshIngredientRanges, availableIngredientIds)}");
        Console.WriteLine($"Part2: {Cafeteria.Part2(freshIngredientRanges, availableIngredientIds)}");
    }

    static long Part1(ReadOnlySpan<IngredientRange> freshIngredientRanges, ReadOnlySpan<IngredientId> availableIngredientIds)
    {
        long freshIngredientCount = 0;
        foreach (IngredientId ingredientId in availableIngredientIds)
        {
            int left = 0, right = freshIngredientRanges.Length - 1;

            while (left <= right)
            {
                int middle = left + (right - left) / 2;
                IngredientRange freshRange = freshIngredientRanges[middle];

                if (ingredientId < freshRange.FirstId)
                {
                    right = middle - 1;
                }
                else if (ingredientId > freshRange.LastId)
                {
                    left = middle + 1;
                }
                else
                {
                    ++freshIngredientCount;
                    break;
                }
            }
        }

        return freshIngredientCount;
    }

    static long Part2(ReadOnlySpan<IngredientRange> freshIngredientRanges, ReadOnlySpan<IngredientId> availableIngredientIds)
    {
        long freshIngredientCount = 0;

        foreach (IngredientRange freshRange in freshIngredientRanges)
        {
            freshIngredientCount += freshRange.LastId - freshRange.FirstId + 1;
        }

        return freshIngredientCount;
    }
}

readonly struct IngredientId
{
    public readonly long Value { get; init; }

    public static IngredientId Parse(scoped ReadOnlySpan<char> source)
    {
        return new IngredientId { Value = long.Parse(source) };
    }

    public static implicit operator IngredientId(long value)
    {
        return new IngredientId { Value = value };
    }

    public static implicit operator long(IngredientId ingredient)
    {
        return ingredient.Value;
    }
}

readonly struct IngredientRange : IComparable<IngredientRange>
{
    public readonly IngredientId FirstId { get; init; }
    public readonly IngredientId LastId { get; init; }

    public readonly bool Contains(IngredientId ingredientId)
    {
        return FirstId <= ingredientId && ingredientId <= LastId;
    }

    public readonly int CompareTo(IngredientRange other)
    {
        int comparison = FirstId.Value.CompareTo(other.FirstId.Value);

        if (comparison == 0)
        {
            comparison = LastId.Value.CompareTo(other.LastId.Value);
        }

        return comparison;
    }
}
