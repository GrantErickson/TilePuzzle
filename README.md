# TilePuzzle
I ran across this 3d printed puzzle the other day and was having trouble solving it. 
I thought I could build a little program to help me solve it.

![Pices in the puzzle](Pieces.png "Pieces")

However, like most other things one thinks they can knock out in an hour or so, this took many hours.

And it turns out that there is a 'trick' to this puzzle which basically makes it unsolvable with the approach I was taking.

I decided to put a little twist on it and make it solvable by adjusting a piece. However, it now finds 96 solutions with this change. 

This code actually does find a solution to a tiling puzzle where you have tiles that you can rotate and flit and put into a grid. 

On my older i9 PC from 2019, it takes about 10 seconds to find a solution for a 9x6 grid with 9 pieces.

I thought that the process for solving was interesting and the fact the the corners are partially rounded and convex and convave make things even trickier.

## The Process
1. Create an array for each piece
1. Create all the flip and rotate permutations.
1. Place the first piece in the 0,0 location in the grid
1. Make sure that there are only regions that have 6 or more contiguous spaces. If not this doesn't work.
1. Find the next open spot and place the next piece there that fits. 
1. Recursively do this until all the pieces are placed. 

Doing the validity checks and making sure that if there is a space that can't be filled, the program backtracks and tries a different piece in the location iteratively finding the solution.

BTW: I just wanted to get this done, so there are probably lots of little issues here that need to get cleaned up. Please don't take this as pristine code.