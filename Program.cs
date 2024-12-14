// See https://aka.ms/new-console-template for more information
using TilePuzzle;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var pieces = new List<Piece>();

pieces.Add(new Piece(1, new int[2, 4]{
    { -315, 1, 45, 0 },
    { 1, 1, 1, 45},
}));
pieces.Add(new Piece(2, new int[3, 3]{
    { 1, 1, 135 },
    { 1, 1, 0},
    { -225, 1, 0},
}));
pieces.Add(new Piece(3, new int[3, 2]{
    { 1, 45 },
    { 1, 1},
    { 1, 1},
}));
pieces.Add(new Piece(4, new int[2, 4]{
    { 1, 1, 1, -45 },
    { 0, 225, 1, 1 },
}));
pieces.Add(new Piece(5, new int[2, 5]{
    { 225, 1, 1, 1, 1 },
    { 0, 0, 1, -135, 0 },
}));
pieces.Add(new Piece(6, new int[3, 3]{
    { 1, 1, -45 },
    { 225, 1, 1 },
    { 0, 0, 135 },
}));
pieces.Add(new Piece(7, new int[3, 3]{
    { 1, 45, 0 },
    { 1, 1, 45 },
    { 0, 1, 0 },
}));
pieces.Add(new Piece(8, new int[2, 4]{
    { 1, 45, 0, 0 },
    { 1, 1, 1, 1 },
}));
pieces.Add(new Piece(9, new int[3, 3]{
    { 45, 0 ,0 },
    { 1, 1, 0 },
    { 1, 1, 45 },
}));


Board board = new(9, 6);
int[,] grid = new int[9, 6];
board.Solve(grid, pieces);

Console.WriteLine(grid.Print());

//foreach (var piece in pieces)
//{
//    Console.WriteLine("-------------------------");
//    Console.WriteLine(piece);
//}




