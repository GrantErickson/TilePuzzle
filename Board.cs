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
    public List<PlacedPiece> Pieces { get; } = new();

    // Record all the boards in the queue based on pieces

    public Board(int width, int height, List<Piece> pieces)
    {
        Width = width;
        Height = height;
        Grid = new int[width, height];
    }
    public override string ToString()
    {
        StringBuilder result = new();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                result.Append(Grid[x, y].ToString().PadLeft(4));
            }
            result.AppendLine();
        }
        return result.ToString();
    }

    public void Solve(IEnumerable<Piece> pieces)
    {
        // Try every piece in every location starting with the first piece
        foreach (var piece in pieces)
        {
            foreach(var op in piece.OrientedPieces)
            {
                PlacePiece(op);
            }
        }
    }

    public void PlacePiece(OrientedPiece op)
    {
        // Place to piece in the grid

        // Check to make sure that placement is valid
    }


    public bool IsGridValid(int[,] grid)
    {
        // Make sure the grid has all spaces that have at least 6 free cells
        int[,] visited = (int[,])grid.Clone();
        // loop through the array and find empty cells
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (visited[x, y] <= 0)
                {
                    // find the largest empty space
                    var emptyCells = EmptyNeighbors(visited, x, y);
                    if (emptyCells < 6) return false;
                }
            }
        }
        return true;
    }

    public int EmptyNeighbors(int[,] grid, int x, int y)
    {
        // Check bounds
        if (x < 0 || x >= Width || y < 0 || y >= Height) return 0;
        // check if the cell is empty
        if (grid[x, y] > 0) return 0;
        // mark the cell as visited
        grid[x, y] = 1;
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

