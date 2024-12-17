using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle;
public class Board
{
    public List<PlacedPiece> PlacedPieces { get; } = new();

    private int[] ValidCellValues = { 0, 1, 180, -180, 45, -45, 135, -135, 225, -225, 315, -315 };

    // Record all the boards in the queue based on pieces


    public Grid? Solve(Grid grid, List<Piece> pieces)
    {

        // There will always be an empty spot if we have pieces.
        var emptySpot = grid.FirstEmptySpot();
        var indent = PlacedPieces.Count * 2;
        //Console.WriteLine($"{new string(' ', indent)}Attempting to place at {emptySpot.x},{emptySpot.y}");
        // Try every piece in the empty spot
        foreach (var piece in pieces)
        {
            if (!PlacedPieces.Any())
            {
                Console.WriteLine($"Trying Piece {piece.PieceNumber} at 0,0");
                Console.WriteLine(piece.OrientedPieces.First().ToString());
            }
            foreach (var op in piece.OrientedPieces)
            {
                //Console.WriteLine(op);

                for (int xOffset = 0; xOffset < op.Width; xOffset++)
                {
                    for (int yOffset = 0; yOffset < op.Height; yOffset++)
                    {
                        //Console.WriteLine($"{op.PieceNumber} ({xOffset},{yOffset})");
                        var pp = grid.PlacePiece(op, xOffset, yOffset, emptySpot.x, emptySpot.y);
                        if (pp != null)
                        {
                            PlacedPieces.Add(pp);
                            grid = pp.Grid;
                            //Console.WriteLine($"{new string(' ', indent)}Placed Piece: {pp.PlacementText}");
                            //grid.Print();
                            var availablePieces = pieces.Where(p => p != piece).ToList();
                            if (availablePieces.Any())
                            {
                                if (availablePieces.Count == 1)
                                {
                                    Console.WriteLine($"{new string(' ', indent)}Last Piece");
                                    //Console.WriteLine(pp.Grid.Print());
                                    //Console.WriteLine(availablePieces.First().OrientedPieces.First().ToString());
                                }
                                //Console.WriteLine(pp.Grid.Print());
                                var solution = Solve(pp.Grid, availablePieces);
                                if (solution != null)
                                {
                                    return solution;
                                }
                                else
                                {
                                    // TODO: Handle removing a placed piece and rolling back.
                                    Console.WriteLine($"{new string(' ', indent)}Removed Piece: {pp.PlacementText}");
                                    PlacedPieces.Remove(pp);
                                    if (PlacedPieces.Any())
                                    {
                                        grid = PlacedPieces.Last().Grid;
                                    }
                                    else
                                    {
                                        grid = new Grid(grid.Width, grid.Height);
                                    }
                                }
                            }
                            else
                            {
                                return pp.Grid;
                            }
                        }
                    }
                }
                if (pieces.Count == 9)
                {
                    Console.WriteLine($"Piece {piece.PieceNumber} didn't fit at {emptySpot.x},{emptySpot.y}");
                }
            }
            if (pieces.Count == 9)
            {
                Console.WriteLine($"Piece {piece.PieceNumber} didn't fit at {emptySpot.x},{emptySpot.y}");
            }
        }
        return null;
    }






}

