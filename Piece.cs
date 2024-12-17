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
    public int PieceNumber { get; }
    public Piece(int pieceNumber, int[,] grid)
    {
        PieceNumber = pieceNumber;
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
        var op = new OrientedPiece(newGrid, 0, false, this);
        OrientedPieces.Add(op);
        OrientedPieces.Add(op.CloneFlip());
        for (int x = 0; x < 3; x++)
        {
            op = op.CloneRotate();
            if (IsDuplicate(op)) continue;
            OrientedPieces.Add(op);
            var opFlip = op.CloneFlip();
            if (IsDuplicate(opFlip)) continue;
            OrientedPieces.Add(opFlip);
        }
    }

    private bool IsDuplicate(OrientedPiece op)
    {
        foreach (var oPiece in OrientedPieces)
        {
            if (oPiece.IsSame(op))
            {
                return true;
            }
        }
        return false;
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

