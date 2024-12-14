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
    public int[,] Grid { get; }
    public List<PlacedPiece> PlacedPieces { get; } = new();

    // Record all the boards in the queue based on pieces

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        Grid = new int[width, height];
    }
    public override string ToString()
    {
        return Grid.Print();
    }

    public bool Solve(List<Piece> pieces)
    {
        // Try every piece in every location starting with the first piece
        foreach (var piece in pieces)
        {
            foreach (var op in piece.OrientedPieces)
            {
                var pp = PlacePiece(op); 
                if (pp != null)
                {
                    var availablePieces = pieces.Where(p => p != piece).ToList();
                    if (availablePieces.Count > 0)
                    {
                        Console.WriteLine(pp.Grid.Print());
                        Solve(availablePieces);
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

    public PlacedPiece? PlacePiece(OrientedPiece op)
    {
        // Place to piece in the grid
        // Find the first open spot in the grid
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Grid[x, y] <= 0)
                {
                    // Check to see if the piece fits
                    if (CanPlacePiece(op, x, y))
                    {
                        // Place the piece
                        var grid = Grid.Copy();
                        for (int px = 0; px < op.Width(); px++)
                        {
                            for (int py = 0; py < op.Height(); py++)
                            {
                                // Add the fields together so we know when we have two parts making a whole.
                                grid[x + px, y + py] = grid[x + px, y + py] + op.Grid[px, py];
                            }
                        }
                        PlacedPiece pp = new(x, y, op, grid);
                        PlacedPieces.Add(pp);
                        return pp;
                    }
                }
            }
        }
        return null;
    }

    private bool CanPlacePiece(OrientedPiece op, int x, int y)
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
                if (Grid[x + px, y + py] == 0) continue;
                // Check if the piece fits
                if (op.Grid[px, py] != 0 && Grid[x + px, y + py] != 0)
                {
                    // Special logic for partial pieces
                    if (Math.Abs(op.Grid[px, py] - Grid[x + px, y + py]) != 180) return false;
                };
            }
        }
        return true;
    }

    public bool IsGridValid(int[,] grid)
    {
        // Make sure the grid has all spaces that have at least 6 free cells
        // loop through the array and find empty cells
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (grid[x, y] == 0)
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

    public int EmptyNeighbors(int[,] grid, int x, int y)
    {
        // Check bounds
        if (x < 0 || x >= Width || y < 0 || y >= Height) return 0;
        // check if the cell is empty
        if (grid[x, y] != 0) return 0;
        // mark the cell as visited
        grid[x, y] = 2;
        // count the empty cells
        int count = 1;
        // check the neighbors
        if (x > 0) count += EmptyNeighbors(grid, x - 1, y);
        if (x < Width - 1) count += EmptyNeighbors(grid, x + 1, y);
        if (y > 0) count += EmptyNeighbors(grid, x, y - 1);
        if (y < Height - 1) count += EmptyNeighbors(grid, x, y + 1);
        return count;
    }
}

