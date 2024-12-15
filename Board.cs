using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle;
public class Board
{
    public int Width { get; }
    public int Height { get; }
    //public int[,] Grid { get; }
    public List<PlacedPiece> PlacedPieces { get; } = new();

    public int[,]? Solution { get; set; } = null;

    private int[] ValidCellValues = { 0, 1, 180, -180, 45, -45, 135, -135, 225, -225, 315, -315 };

    // Record all the boards in the queue based on pieces

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        //Grid = new int[width, height];
    }

    public bool Solve(int[,] grid, List<Piece> pieces)
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
                for (int xOffset = 0; xOffset < op.Width; xOffset++)
                {
                    for (int yOffset = 0; yOffset < op.Height; yOffset++)
                    {
                        var pp = PlacePiece(op, xOffset, yOffset, emptySpot.x, emptySpot.y, grid);
                        if (pp != null)
                        {
                            //Console.WriteLine($"{new string(' ', indent)}Placed Piece: {pp.PlacementText}");
                            var availablePieces = pieces.Where(p => p != piece).ToList();
                            if (availablePieces.Any())
                            {
                                if (availablePieces.Count == 1)
                                {
                                    //Console.WriteLine($"{new string(' ', indent)}Last Piece");
                                    //Console.WriteLine(pp.Grid.Print());
                                    //Console.WriteLine(availablePieces.First().OrientedPieces.First().ToString());
                                }
                                //Console.WriteLine(pp.Grid.Print());
                                if (Solve(pp.Grid, availablePieces))
                                {
                                    return true;
                                }
                                else
                                {
                                    // TODO: Handle removing a placed piece and rolling back.
                                    //Console.WriteLine($"{new string(' ', indent)}Removed Piece: {pp.PlacementText}");
                                    PlacedPieces.Remove(pp);
                                    if (PlacedPieces.Any())
                                    {
                                        grid = PlacedPieces.Last().Grid;
                                    }
                                    else
                                    {
                                        grid = new int[Width, Height];
                                    }
                                }
                            }
                            else
                            {
                                if (Solution == null) Solution = pp.Grid;
                                return true;
                            }
                        }
                    }
                }
            }
            if (pieces.Count == 9)
            {
                //Console.WriteLine($"Piece {piece.PieceNumber} didn't fit at {emptySpot.x},{emptySpot.y}");
            }
        }
        return false;
    }


    public PlacedPiece? PlacePiece(OrientedPiece op, int xOffset, int yOffset, int x, int y, int[,] grid)
    {
        // Place to piece in the grid
        // Check to see if the piece fits
        if (CanPlacePiece(op, xOffset, yOffset, x, y, grid))
        {
            // Place the piece
            var newGrid = grid.Copy();
            for (int px = 0; px < op.Width; px++)
            {
                for (int py = 0; py < op.Height; py++)
                {
                    // Add the fields together so we know when we have two parts making a whole.
                    newGrid[x + px - xOffset, y + py - yOffset] = newGrid[x + px - xOffset, y + py - yOffset] + op.Grid[px, py];
                }
            }
            //Console.WriteLine(newGrid.Print());
            if (newGrid.IsGridValid(6))
            {
                PlacedPiece pp = new(x, y, op, newGrid);
                PlacedPieces.Add(pp);
                return pp;
            }
        }
        return null;
    }

    private bool CanPlacePiece(OrientedPiece op, int xOffset, int yOffset, int x, int y, int[,] grid)
    {
        // Try to place the piece in the spot
        for (int px = 0; px < op.Width; px++)
        {
            for (int py = 0; py < op.Height; py++)
            {
                // If the incoming piece has nothing there just ignore it.
                if (op.Grid[px, py] == 0) continue;
                // Check bounds
                if (x + px - xOffset >= Width || x + px - xOffset < 0 || y + py - yOffset >= Height || y + py - yOffset < 0) return false;
                // If target space is empty, ignore it.
                if (grid[x + px - xOffset, y + py - yOffset] == 0) continue;
                // Check if the piece fits
                if (op.Grid[px, py] != 0 && grid[x + px - xOffset, y + py - yOffset] != 0)
                {
                    // Special logic for partial pieces
                    if (Math.Abs(op.Grid[px, py] + grid[x + px - xOffset, y + py - yOffset]) != 180) return false;
                };
            }
        }
        return true;
    }




}

