using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class PrintingDepartment
{
    static void Main()
    {
        List<char[]> input = [];
        while (Console.ReadLine() is string line && !string.IsNullOrEmpty(line))
        {
            input.Add(line.ToCharArray());
        }

        ReadOnlySpan<char[]> grid = CollectionsMarshal.AsSpan(input);

        Console.WriteLine($"Part1: {PrintingDepartment.Part1(grid)}");
        Console.WriteLine($"Part2: {PrintingDepartment.Part2(grid)}");
    }

    static long Part1(ReadOnlySpan<char[]> grid)
    {
        long accessiblePaperCount = 0;

        int m = grid.Length, n = grid[0].Length;
        for (int y = 0; y < m; ++y)
        {
            int yMin = int.Max(0, y - 1);
            int yMax = int.Min(m - 1, y + 1);
            for (int x = 0; x < n; ++x)
            {
                if (grid[y][x] == '.')
                    continue;

                int count = 0;
                int xMin = int.Max(0, x - 1);
                int xMax = int.Min(m - 1, x + 1);
                for (int ny = yMin; ny <= yMax; ++ny)
                {
                    for (int nx = xMin; nx <= xMax; ++nx)
                    {
                        if (grid[ny][nx] == '@')
                        {
                            ++count;
                        }
                    }
                }

                if (count <= 4)
                {
                    ++accessiblePaperCount;
                }
            }
        }

        return accessiblePaperCount;
    }

    static long Part2(ReadOnlySpan<char[]> grid)
    {
        long accessiblePaperCount = 0;

        int m = grid.Length, n = grid[0].Length;
        while (true)
        {
            long previousPaperCount = accessiblePaperCount;

            for (int y = 0; y < m; ++y)
            {
                int yMin = int.Max(0, y - 1);
                int yMax = int.Min(m - 1, y + 1);
                for (int x = 0; x < n; ++x)
                {
                    if (grid[y][x] == '.')
                        continue;

                    int count = 0;
                    int xMin = int.Max(0, x - 1);
                    int xMax = int.Min(m - 1, x + 1);
                    for (int ny = yMin; ny <= yMax; ++ny)
                    {
                        for (int nx = xMin; nx <= xMax; ++nx)
                        {
                            if (grid[ny][nx] == '@')
                            {
                                ++count;
                            }
                        }
                    }

                    if (count <= 4)
                    {
                        ++accessiblePaperCount;
                        grid[y][x] = '.';
                    }
                }
            }

            if (previousPaperCount == accessiblePaperCount)
            {
                break;
            }
        }

        return accessiblePaperCount;
    }
}
