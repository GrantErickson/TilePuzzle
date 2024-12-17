using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle;

public class OrientedPiece
{
    public int[,] Grid { get; }
    public int Angle { get; }
    public bool IsFlipped { get; }
    public Piece Piece { get; }

    public string PieceNumber
    {
        get
        {
            return $"{Piece.PieceNumber}:{AngleSymbol}{IsFlippedSymbol}";
        }
    }

    public string AngleSymbol
    {
        get
        {
            if (Angle == 0) return "🡑";
            if (Angle == 90) return "🡒";
            if (Angle == 180) return "🡓";
            if (Angle == 270) return "🡐";
            return "▮";
        }
    }
    public string IsFlippedSymbol => IsFlipped ? "🡘" : " ";

    public OrientedPiece(int width, int height, int angle, bool isFlipped, Piece piece)
    {
        Grid = new int[width, height];
        Angle = angle;
        IsFlipped = isFlipped;
        Piece = piece;
    }
    public OrientedPiece(int[,] grid, int angle, bool isFlipped, Piece piece)
    {
        Grid = grid;
        Angle = angle;
        IsFlipped = isFlipped;
        Piece = piece;
    }

    public int Width => Grid.GetLength(0);
    public int Height => Grid.GetLength(1);

    public OrientedPiece CloneRotate()
    {
        var angle = Angle + 90;
        if (angle == 360) angle = 0;
        return new OrientedPiece(RotateArray(Grid), angle, IsFlipped, Piece);
    }
    public OrientedPiece CloneFlip()
    {
        return new OrientedPiece(FlipArray(Grid), Angle, !IsFlipped, Piece);
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
        return Grid.Print();
    }

    public bool IsSame(OrientedPiece op)
    {
        if (op.Width != Width || op.Height != Height) return false;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (op.Grid[x, y] != Grid[x, y]) return false;
            }
        }
        return true;
    }
}
