using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle;

public class Grid
{
    public int[,] Pieces { get; }
    public int[,] Spots { get; }
    public int Width { get; }
    public int Height { get; }

    public Grid(int width, int height)
    {
        Pieces = new int[width, height];
        Spots = new int[width, height];
        Width = width;
        Height = height;
    }

    public Grid(Grid grid)
    {
        Pieces = grid.Pieces.Copy();
        Spots = grid.Spots.Copy();
        Width = grid.Spots.GetLength(0);
        Height = grid.Spots.GetLength(1);
    }

    public PlacedPiece? PlacePiece(OrientedPiece op, int xOffset, int yOffset, int x, int y)
    {
        // Place to piece in the grid
        // Check to see if the piece fits
        if (CanPlacePiece(op, xOffset, yOffset, x, y))
        {
            // Place the piece
            var newGrid = new Grid(this);
            for (int px = 0; px < op.Width; px++)
            {
                for (int py = 0; py < op.Height; py++)
                {
                    // Add the fields together so we know when we have two parts making a whole.
                    newGrid.Spots[x + px - xOffset, y + py - yOffset] = newGrid.Spots[x + px - xOffset, y + py - yOffset] + op.Grid[px, py];
                    if (op.Grid[px, py] != 0)
                    {
                        newGrid.Pieces[x + px - xOffset, y + py - yOffset] = op.Piece.PieceNumber;
                    }
                }
            }
            //newGrid.Print();
            if (newGrid.IsGridValid(6))
            {
                PlacedPiece pp = new(x - xOffset, y - yOffset, op, newGrid);
                return pp;
            }
        }
        return null;
    }

    public bool CanPlacePiece(OrientedPiece op, int xOffset, int yOffset, int x, int y)
    {
        // Try to place the piece in the spot
        //Print();
        //Console.WriteLine($"{op.PieceNumber} ({xOffset},{yOffset})");
        //Console.WriteLine(op);
        for (int px = 0; px < op.Width; px++)
        {
            for (int py = 0; py < op.Height; py++)
            {
                // If the incoming piece has nothing there just ignore it.
                if (op.Grid[px, py] == 0) continue;
                // Check bounds
                if (x + px - xOffset >= Width || x + px - xOffset < 0 || y + py - yOffset >= Height || y + py - yOffset < 0) return false;
                // If target space is empty, ignore it.
                if (Spots[x + px - xOffset, y + py - yOffset] == 0) continue;
                // Check if the piece fits
                if (op.Grid[px, py] != 0 && Spots[x + px - xOffset, y + py - yOffset] != 0)
                {
                    // Special logic for partial pieces
                    if (Math.Abs(op.Grid[px, py] + Spots[x + px - xOffset, y + py - yOffset]) != 180) return false;
                };
            }
        }
        return true;
    }

    public (int x, int y) FirstEmptySpot()
    {
        // Find the first empty spot in the grid
        for (int y = 0; y < Spots.GetLength(1); y++)
        {
            for (int x = 0; x < Spots.GetLength(0); x++)
            {
                if (Spots[x, y] <= 0 && Spots[x, y] != -180)
                {
                    return (x, y);
                }
            }
        }
        // This should never be called.
        return (-1, -1);
    }


    public bool IsGridValid(int minSize)
    {
        //Console.WriteLine(grid.Print());
        // Make sure the grid has all spaces that have at least [min] free cells
        // loop through the array and find empty cells
        for (int y = 0; y < Spots.GetLength(1); y++)
        {
            for (int x = 0; x < Spots.GetLength(0); x++)
            {
                if (Spots[x, y] <= 0 && Spots[x, y] != -180)
                {
                    //Console.WriteLine(grid.Print());
                    // find the largest empty space
                    var emptyCells = EmptyNeighbors(x, y);
                    if (emptyCells < minSize)
                    {
                        //Console.WriteLine(grid.Print());
                        Clean();
                        return false;
                    }
                    //Console.WriteLine(grid.Print());
                }
            }
        }
        // Remove all the 2s from the grid
        // This is done instead of creating a new grid to save memory and maybe for speed
        //Console.WriteLine(grid.Print());
        Clean();
        return true;
    }
    public int EmptyNeighbors(int x, int y)
    {
        //Console.WriteLine(grid.Print());
        // Check bounds
        if (x < 0 || x >= Spots.GetLength(0) || y < 0 || y >= Spots.GetLength(1)) return 0;
        // check if the cell is empty
        if (Spots[x, y] > 0) return 0;
        // mark the cell as visited
        Spots[x, y] += 1000;
        // count the empty cells
        int count = 1;
        // check the neighbors
        if (x > 0) count += EmptyNeighbors(x - 1, y);
        if (x < Spots.GetLength(0) - 1) count += EmptyNeighbors(x + 1, y);
        if (y > 0) count += EmptyNeighbors(x, y - 1);
        if (y < Spots.GetLength(1) - 1) count += EmptyNeighbors(x, y + 1);
        return count;
    }

    public void Clean()
    {
        for (int x = 0; x < Spots.GetLength(0); x++)
        {
            for (int y = 0; y < Spots.GetLength(1); y++)
            {
                if (Spots[x, y] == 2) Spots[x, y] = 0;
                if (Spots[x, y] > 500) Spots[x, y] -= 1000;
            }
        }
    }

    public void Print()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Pieces[x, y] == 0) Console.ForegroundColor = ConsoleColor.White;
                else Console.ForegroundColor = (ConsoleColor)Pieces[x, y];
                Console.Write(Spots[x, y].Char());
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    public bool IsSame(Grid grid)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Pieces[x, y] != grid.Pieces[x, y]) return false;
                if (Spots[x, y] != grid.Spots[x, y]) return false;
            }
        }
        return true;
    }

}
