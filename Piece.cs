using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle;
public class Piece
{
    public List<OrientedPiece> OrientedPieces { get; }
    public Piece(int[,] grid)
    {
        OrientedPieces = new List<OrientedPiece>();
        // fix the piece orientation because we want it to be easy to build the arrays.
        var cols = grid.GetLength(0);
        var rows = grid.GetLength(1);
        var newGrid = new int[rows, cols];

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                newGrid[y, x] = grid[x,y];
            }
        }
        var op = new OrientedPiece(newGrid, 0, false);
        //op = op.CloneRotate();
        //op = op.CloneFlip();
        //op = new OrientedPiece(op.Grid, 0, false);
        OrientedPieces.Add(op);
        OrientedPieces.Add(op.CloneFlip());
        for (int x = 0; x < 3; x++)
        {
            op = op.CloneRotate();
            OrientedPieces.Add(op);
            OrientedPieces.Add(op.CloneFlip());
        }
    }

    public override string ToString()
    {
        StringBuilder result = new();

        foreach (var piece in OrientedPieces)
        {
            result.AppendLine($"Angle: {piece.Angle}  Flipped: {piece.IsFlipped}");
            result.AppendLine(piece.ToString());
        }
        return result.ToString();
    }
}

