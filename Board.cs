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

        // Try every piece in the empty spot
        foreach (var piece in pieces)
        {
            foreach (var op in piece.OrientedPieces)
            {
                var pp = PlacePiece(op, emptySpot.x, emptySpot.y, grid);
                if (pp != null)
                {
                    Console.WriteLine($"Placed Piece: {pp.PlacementText}");
                    var availablePieces = pieces.Where(p => p != piece).ToList();
                    if (availablePieces.Count > 0)
                    {
                        Console.WriteLine(pp.Grid.Print());
                        if (Solve(pp.Grid, availablePieces))
                        {
                            return true;
                        }
                        else
                        {
                            // TODO: Handle removing a placed piece and rolling back.
                            Console.WriteLine($"Removed Piece: {pp.PlacementText}");
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
                        return true;
                    }
                }
            }
        }
        return false;
    }


    public PlacedPiece? PlacePiece(OrientedPiece op, int x, int y, int[,] grid)
    {
        // Place to piece in the grid
        // Check to see if the piece fits
        if (CanPlacePiece(op, x, y, grid))
        {
            // Place the piece
            var newGrid = grid.Copy();
            for (int px = 0; px < op.Width(); px++)
            {
                for (int py = 0; py < op.Height(); py++)
                {
                    // Add the fields together so we know when we have two parts making a whole.
                    newGrid[x + px, y + py] = newGrid[x + px, y + py] + op.Grid[px, py];
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

    private bool CanPlacePiece(OrientedPiece op, int x, int y, int[,] grid)
    {
        // Try to place the piece in the spot
        for (int px = 0; px < op.Width(); px++)
        {
            for (int py = 0; py < op.Height(); py++)
            {
                // If the incoming piece has nothing there just ignore it.
                if (op.Grid[px, py] == 0) continue;
                // Check bounds
                if (x + px >= Width || y + py >= Height) return false;
                // If target space is empty, ignore it.
                if (grid[x + px, y + py] == 0) continue;
                // Check if the piece fits
                if (op.Grid[px, py] != 0 && grid[x + px, y + py] != 0)
                {
                    // Special logic for partial pieces
                    if (Math.Abs(op.Grid[px, py] + grid[x + px, y + py]) != 180) return false;
                };
            }
        }
        return true;
    }




}

