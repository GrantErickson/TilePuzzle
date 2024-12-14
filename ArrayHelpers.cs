using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle;
internal static class ArrayHelpers
{
    public static void Clean(this int[,] grid)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] == 2) grid[x, y] = 0;
            }
        }
    }

    public static int[,] Copy(this int[,] grid)
    {
        int[,] result = new int[grid.GetLength(0), grid.GetLength(1)];
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                result[x, y] = grid[x, y];
            }
        }
        return result;
    }

    public static char Char(this int value)
    {
        if (value == 0) return '.';
        if (value == 45) return '◣';
        if (value == 135) return '◤';
        if (value == 225) return '◥';
        if (value == 315) return '◢';
        if (value == -45) return '◺';
        if (value == -135) return '◸';
        if (value == -225) return '◹';
        if (value == -315) return '◿';
        return '▮';
    }

    public static string Print(this int[,] grid)
    {
        StringBuilder result = new();
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                result.Append(grid[i, j].Char());
            }
            result.AppendLine();
        }
        return result.ToString();
    }

}

