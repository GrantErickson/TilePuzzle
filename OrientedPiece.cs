using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver;

public class OrientedPiece
{
    public int[,] Grid { get; }
    public int Angle { get; }
    public bool IsFlipped { get; }

    public OrientedPiece(int width, int height, int angle, bool isFlipped)
    {
        Grid = new int[width, height];
        Angle = angle;
        IsFlipped = isFlipped;
    }
    public OrientedPiece(int[,] grid, int angle, bool isFlipped)
    {
        Grid = grid;
        Angle = angle;
        IsFlipped = isFlipped;
    }

    public int Width() => Grid.GetLength(0);
    public int Height() => Grid.GetLength(1);

    public OrientedPiece CloneRotate()
    {
        var angle = Angle + 90;
        if (angle == 360) angle = 0;
        return new OrientedPiece(RotateArray(Grid), angle, IsFlipped);
    }
    public OrientedPiece CloneFlip()
    {
        return new OrientedPiece(FlipArray(Grid), Angle, !IsFlipped);
    }

    static int[,] RotateArray(int[,] array)
    {
        int cols = array.GetLength(0);
        int rows = array.GetLength(1);
        int[,] rotated = new int[rows, cols];
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var value = array[x, y];
                if (value > 1)
                {
                    value += 90;
                    if (value > 360) value = 45;
                }
                else if (value < -1)
                {
                    value -= 90;
                    if (value < -360) value = -45;
                }
                rotated[rows - 1 - y, x] = value;
            }
        }
        return rotated;
    }
    static int[,] FlipArray(int[,] array)
    {
        int cols = array.GetLength(0);
        int rows = array.GetLength(1);
        int[,] flipped = new int[cols, rows];
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var value = array[x, y];
                if (value == 45) value = 315;
                else if (value == 315) value = 45;
                else if (value == 135) value = 225;
                else if (value == 225) value = 135;
                else if (value == -45) value = -315;
                else if (value == -315) value = -45;
                else if (value == -135) value = -225;
                else if (value == -225) value = -135;
                flipped[cols - 1 - x, y] = value;
            }
        }
        return flipped;
    }

    public override string ToString()
    {
        StringBuilder result = new();
        for (int j = 0; j < Grid.GetLength(1); j++)
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                char c = '▮';
                var value = Grid[i, j];
                if (value == 0) c = ' ';
                if (value == 45) c = '◣';
                if (value == 135) c = '◤';
                if (value == 225) c = '◥';
                if (value == 315) c = '◢';
                if (value == -45) c = '◺';
                if (value == -135) c = '◸';
                if (value == -225) c = '◹';
                if (value == -315) c = '◿';
                result.Append(c);
            }
            result.AppendLine();
        }
        return result.ToString();
    }
}
