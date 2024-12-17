using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilePuzzle
{
    public class PlacedPiece
    {
        public int X { get; }
        public int Y { get; }
        public OrientedPiece OrientedPiece { get; }
        public Grid Grid { get; }
        public string PlacementText { get
            {
                return $"{OrientedPiece.PieceNumber} at {X},{Y} ";
            }
        }

        public PlacedPiece(int x, int y, OrientedPiece orientedPiece, Grid grid)
        {
            X = x;
            Y = y;
            OrientedPiece = orientedPiece;
            Grid = grid;
        }
    }
}
