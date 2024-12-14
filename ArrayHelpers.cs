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
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 2) grid[x, y] = 0;
                if (grid[x, y] > 500) grid[x, y] -= 1000;
            }
        }
    }

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
        if (value == -180 || value == 180) return '▭';
        throw new Exception($"Unknown value: {value}");
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

    public static (int x, int y) FirstEmptySpot(this int[,] grid)
    {
        // Find the first empty spot in the grid
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] <= 0)
                {
                    return (x, y);
                }
            }
        }
        // This should never be called.
        return (-1, -1);
    }

    public static bool IsGridValid(this int[,] grid, int min)
    {
        // Make sure the grid has all spaces that have at least [min] free cells
        // loop through the array and find empty cells
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] <= 0)
                {
                    // find the largest empty space
                    var emptyCells = EmptyNeighbors(grid, x, y);
                    if (emptyCells < 6) return false;
                }
            }
        }
        // Remove all the 2s from the grid
        // This is done instead of creating a new grid to save memory and maybe for speed
        grid.Clean();
        return true;
    }
    public static int EmptyNeighbors(int[,] grid, int x, int y)
    {
        // Check bounds
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) return 0;
        // check if the cell is empty
        if (grid[x, y] > 0) return 0;
        // mark the cell as visited
        grid[x, y] += 1000;
        // count the empty cells
        int count = 1;
        // check the neighbors
        if (x > 0) count += EmptyNeighbors(grid, x - 1, y);
        if (x < grid.GetLength(1) - 1) count += EmptyNeighbors(grid, x + 1, y);
        if (y > 0) count += EmptyNeighbors(grid, x, y - 1);
        if (y < grid.GetLength(0) - 1) count += EmptyNeighbors(grid, x, y + 1);
        return count;
    }
}

