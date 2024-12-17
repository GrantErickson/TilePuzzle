using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle;
internal static class ArrayHelpers
{


    public static int[,] Copy(this int[,] grid)
    {
        int[,] result = new int[grid.GetLength(0), grid.GetLength(1)];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                result[x, y] = grid[x, y];
            }
        }
        return result;
    }

    public static char Char(this int value)
    {
        if (value == 0) return '.';
        if (value == 1) return '■';
        if (value == 45) return '◣';
        if (value == 135) return '◤';
        if (value == 225) return '◥';
        if (value == 315) return '◢';
        if (value == -45) return '◺';
        if (value == -135) return '◸';
        if (value == -225) return '◹';
        if (value == -315) return '◿';
        if (value == -180) return '◩'; // Note: this isn't quite right
        if (value == 180) return '◪'; // Note: this isn't quite right
        return 'o';
    }

    public static string Print(this int[,] grid)
    {
        StringBuilder result = new();
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                result.Append(grid[x, y].Char());
            }
            result.AppendLine();
        }
        return result.ToString();
    }


}

