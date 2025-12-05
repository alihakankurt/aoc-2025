using System;

class GiftShop
{
    static void Main()
    {
        string line = Console.ReadLine()!;
        string[] ranges = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"Part1: {GiftShop.Part1(ranges)}");
        Console.WriteLine($"Part2: {GiftShop.Part2(ranges)}");
    }

    static ulong Part1(ReadOnlySpan<string> ranges)
    {
        ulong invalidIdSum = 0;
        foreach (ReadOnlySpan<char> range in ranges)
        {
            var enumerator = range.Split('-');

            if (!enumerator.MoveNext()) return 0;
            ulong firstProductId = ulong.Parse(range[enumerator.Current]);

            if (!enumerator.MoveNext()) return 0;
            ulong lastProductId = ulong.Parse(range[enumerator.Current]);

            ulong productId = firstProductId;
            while (productId <= lastProductId)
            {
                ReadOnlySpan<char> digits = productId.ToString();
                if (digits.Length % 2 == 1)
                {
                    productId = (ulong)float.Pow(10, digits.Length);
                    continue;
                }

                int length = digits.Length / 2;
                ReadOnlySpan<char> sequence1 = digits[..length];
                ReadOnlySpan<char> sequence2 = digits[length..];

                if (sequence1.Equals(sequence2, StringComparison.Ordinal))
                {
                    invalidIdSum += productId;
                }

                ++productId;
            }
        }

        return invalidIdSum;
    }

    static ulong Part2(ReadOnlySpan<string> ranges)
    {
        ulong invalidIdSum = 0;
        foreach (ReadOnlySpan<char> range in ranges)
        {
            var enumerator = range.Split('-');

            if (!enumerator.MoveNext()) return 0;
            ulong firstProductId = ulong.Parse(range[enumerator.Current]);

            if (!enumerator.MoveNext()) return 0;
            ulong lastProductId = ulong.Parse(range[enumerator.Current]);

            ulong productId = firstProductId;
            while (productId <= lastProductId)
            {
                ReadOnlySpan<char> digits = productId.ToString();

                for (int length = 1; length <= digits.Length / 2; ++length)
                {
                    if (digits[length] != digits[0] || digits.Length % length != 0)
                        continue;

                    ReadOnlySpan<char> sequence = digits[..length];
                    if (digits.Count(sequence) == digits.Length / length)
                    {
                        invalidIdSum += productId;
                        break;
                    }
                }

                ++productId;
            }
        }

        return invalidIdSum;
    }
}
