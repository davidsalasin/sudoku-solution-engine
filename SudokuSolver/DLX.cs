using System.Text;

namespace SudokuSolver;

/**
 * ! BEHIND DLX !
 * 
 * Originally named Algorithm X (and given the name "Dancing links" by Donald Knuth) 
 * is a recursive, nondeterministic, depth-first, backtracking algorithm that finds all 
 * solutions to the exact cover problem. Some of the better-known exact cover problems
 * include tiling, the n queens problem, and (The one I used the algorithm for) Sudoku. 
 * dancing links is a technique for reverting the operation of deleting a node from 
 * a circular double linked list (sparse Matrix). It is particularly useful for efficiently
 * implementing backtracking algorithms.
 * 
 * * More about the background of the solving algorithm:
 * Exact cover - https://en.wikipedia.org/wiki/Exact_cover
 * Knuth's Algorithm X - https://en.wikipedia.org/wiki/Knuth%27s_Algorithm_X
 * Dancing Links - https://en.wikipedia.org/wiki/Dancing_Links
 */

// Static Class DLX: includes DLX, Soduko puzzle solving algorithm.
static class DLX
{
    // Static method: FillDoubleLinkedListMatrix (Private).
    // RECEIVES: SudokuPuzzle sp (SudokuPuzzle class reference, with included puzzle details).
    // RETURNS: MatNode head, for a newly created sparse double linked matrix with included candidates
    //          (as rows, based on already set positions from the received Sudoku puzzle), and constraints
    //          (as cols, which present which 4 constraints each candidate presents).
    private static MatNode FillDoubleLinkedListMatrix(SudokuPuzzle sp)
    {
        // Sudoku puzzle's matrix board.
        int[,] puzzelMatrix = sp.SudokuMatrix;

        // Side lenght of the puzzle matrix.
        int side = sp.Side;

        // Root square of the side lenght of the puzzle Matrix -> The side lenght of the inner squares of the puzzle.
        int root = sp.RootSquareSide;

        // Used in Tag values for MatNodes constructed for the sparse matrix.
        int row, column, innerBox, value;

        // Used as index positioning in Toroidal.
        int header;

        // Index values from values on Sudoku puzzle's board matrix.
        int indexValue;

        // Square area of the board.
        int square = side * side;

        // MatNode[,] Toroidal -> Used as framework for the sparse matrix (constructing candidates and constraints the
        //                        way they are set to be. [0,0] will be saved for MatNode sparse matrix's head reference.
        //                        The Borders of the Matrix will also initiate at Side(of a Sudoku board)^3 x Side^2 * 4
        //                        as the amount of constraints there are in a Sudoku board:
        //                        (1) Each row and column containing 1 value: Row - Column constraint.
        //                        (2) Each row contains all values:           Row - Value constraint.
        //                        (3) Each column contains all values:        Column - Value constraint.
        //                        (4) Each innerBox contains all values:      InnerBox - Value constraint.
        MatNode[,] Toroidal = new MatNode[side * square + 1, square * 4 + 1];
        Toroidal[0, 0] = new MatNode("Head");
        Toroidal[0, 0].ColumnLength = side * square;

        // Sets MatNode head value.
        MatNode head = Toroidal[0, 0];

        MatNode ptr;

        // Setting all constraint columns:

        // Row - Column constraint.
        row = 1;
        column = 0;
        for (var i = 1; i <= square; i++)
        {
            // Moving on Rows according to Columns.
            column++;
            if (column > side)
            {
                column = 1;
                row++;
            }

            // Constructing new constraint column node for sparse matrix.
            ptr = Toroidal[0, i] = new MatNode($"R{row}C{column}");
            ptr.ColumnLength = 0;

            // Connecting ptr (column) nodes to head.
            ptr.LeftNode = head.LeftNode;
            ptr.RightNode = head;
            head.LeftNode.RightNode = ptr;
            head.LeftNode = ptr;
        }

        // Row - Value constraint.
        row = 1;
        value = 0;
        for (var i = 1; i <= square; i++)
        {
            // Moving on Rows according to Values.
            value++;
            if (value > side)
            {
                value = 1;
                row++;
            }

            // Constructing new constraint column node for sparse matrix.
            ptr = Toroidal[0, square + i] = new MatNode($"R{row}#{value}");
            ptr.ColumnLength = 0;

            // Connecting ptr (column) nodes to head.
            ptr.LeftNode = head.LeftNode;
            ptr.RightNode = head;
            head.LeftNode.RightNode = ptr;
            head.LeftNode = ptr;
        }

        // Columns - Value constraint.
        column = 1;
        value = 0;
        for (var i = 1; i <= square; i++)
        {
            // Moving on Columns according to Values.
            value++;
            if (value > side)
            {
                value = 1;
                column++;
            }

            // Constructing new constraint column node for sparse matrix.
            ptr = Toroidal[0, 2 * square + i] = new MatNode($"C{column}#{value}");
            ptr.ColumnLength = 0;

            // Connecting ptr (column) nodes to head.
            ptr.LeftNode = head.LeftNode;
            ptr.RightNode = head;
            head.LeftNode.RightNode = ptr;
            head.LeftNode = ptr;
        }


        //InnerBox - Value constraint.
        innerBox = 1;
        value = 0;
        for (var i = 1; i <= square; i++)
        {
            // Moving on InnerBoxes according to Values.
            value++;
            if (value > side)
            {
                value = 1;
                innerBox++;
            }

            // Constructing new constraint column node for sparse matrix.
            ptr = Toroidal[0, 3 * square + i] = new MatNode($"B{innerBox}#{value}");
            ptr.ColumnLength = 0;

            // Connecting ptr (column) nodes to head.
            ptr.LeftNode = head.LeftNode;
            ptr.RightNode = head;
            head.LeftNode.RightNode = ptr;
            head.LeftNode = ptr;
        }

        // Setting all candidate rows:
        row = 1;
        column = 1;
        value = 0;
        for (var i = 1; i <= square * side; i++)
        {
            // Moving on Rows according to Columns according to Values.
            value++;
            if (value > side)
            {
                value = 1;
                column++;
                if (column > side)
                {
                    column = 1;
                    row++;
                }
            }

            // Constructing new candidate row node for sparse matrix.
            ptr = Toroidal[i, 0] = new MatNode($"R{row}C{column}#{value}");

            // Connecting ptr (row) nodes to head.               
            ptr.UpNode = head.UpNode;
            ptr.DownNode = head;
            head.UpNode.DownNode = ptr;
            head.UpNode = ptr;
            ptr.TopNode = head;

            // Constructing 4 constraint nodes for each cell value (position value) on Sudoku board.

            // Pleasing Row - Column constraint.
            header = side * (row - 1) + column;
            Toroidal[i, header] = new MatNode($"R{row}C{column}#{value}", Toroidal[0, header].UpNode, Toroidal[0, header], Toroidal[i, 0], Toroidal[i, 0].LeftNode, Toroidal[0, header]);

            // Pleasing Row - Value constraint.
            header = square + side * (row - 1) + value;
            Toroidal[i, header] = new MatNode($"R{row}C{column}#{value}", Toroidal[0, header].UpNode, Toroidal[0, header], Toroidal[i, 0], Toroidal[i, 0].LeftNode, Toroidal[0, header]);

            // Pleasing Column - Value constraint.
            header = 2 * square + side * (column - 1) + value;
            Toroidal[i, header] = new MatNode($"R{row}C{column}#{value}", Toroidal[0, header].UpNode, Toroidal[0, header], Toroidal[i, 0], Toroidal[i, 0].LeftNode, Toroidal[0, header]);

            // Pleasing InnerBox - Value constraint.
            header = 3 * square + side * (((row - 1) / root * root) + (column - 1) / root) + value;
            Toroidal[i, header] = new MatNode($"R{row}C{column}#{value}", Toroidal[0, header].UpNode, Toroidal[0, header], Toroidal[i, 0], Toroidal[i, 0].LeftNode, Toroidal[0, header]);
        }

        // Removing all existing position values in the puzzle board -> By removing its candidates, 
        // and candidates bound to the same constraints:
        for (row = 0; row < side; row++)
        {
            for (column = 0; column < side; column++)
            {
                indexValue = puzzelMatrix[row, column];
                if (indexValue != 0)
                {
                    // Finding candidate by (row/column/value) on sparse matrix's framework matrix.
                    header = indexValue + side * column + square * row;
                    ptr = Toroidal[header, 0];

                    // Disconnect (Cover) row's col node.
                    ptr.UpNode.DownNode = ptr.DownNode;
                    ptr.DownNode.UpNode = ptr.UpNode;

                    for (var pos = ptr.RightNode; pos != ptr; pos = pos.RightNode)
                    {
                        // Covering each constraint please by candidate
                        Cover(pos);
                    }
                }
            }
        }

        // MatNode head, for a newly created sparse double linked matrix with included candidates
        return head;
    }

    // Static method: GetMinCol (Private).
    // RECEIVES: MatNode head (head for a sparse double linked matrix).
    // RETURNS: Returns MatNode of a column head with the least amount of MatNodes in its column.
    private static MatNode GetMinCol(MatNode head)
    {
        // Get The first Node to the right of the head of the sparse matrix.
        MatNode min = head.RightNode;

        // While moving on all of the column heads.
        for (var pos = min.RightNode; pos != head; pos = pos.RightNode)
        {
            // If column head pos's column lenght (MatNode count) is smaller then min's -> Update min to pos.
            if (pos.ColumnLength < min.ColumnLength)
            {
                min = pos;
            }
        }

        // Returns MatNode of a column head with the least amount of MatNodes in its column.
        return min;
    }

    // Static method: Cover (Private).
    // RECEIVES: MatNode node.
    // RETURNS: void ** unlinks node's column, and each row in the column from the sparse matrix its in **.
    private static void Cover(MatNode node)
    {
        // Get the pointer to the header of column to which this node belong.
        MatNode top = node.TopNode;

        // Unlink column header from it's neighbors.
        top.RightNode.LeftNode = top.LeftNode;
        top.LeftNode.RightNode = top.RightNode;

        // Move down the column and remove each row by traversing right.
        for (var row = top.DownNode; row != top; row = row.DownNode)
        {
            for (var ptr = row.RightNode; ptr != row; ptr = ptr.RightNode)
            {
                ptr.UpNode.DownNode = ptr.DownNode;
                ptr.DownNode.UpNode = ptr.UpNode;

                // after unlinking row node, decrement the node count in column header.
                ptr.TopNode.ColumnLength--;
            }
        }
    }

        // Static method: Uncover (Private).
        // RECEIVES: MatNode node.
        // RETURNS: void ** links node's column, and each row in the column from the sparse matrix its in **.
        private static void Uncover(MatNode node)
        {
            // Get the pointer to the header of column to which this node belong.
            MatNode top = node.TopNode;

            // Move down the column and link back each row by traversing left.
            for (var row = top.UpNode; row != top; row = row.UpNode)
            {
                for (var ptr = row.LeftNode; ptr != row; ptr = ptr.LeftNode)
                {
                    ptr.UpNode.DownNode = ptr;
                    ptr.DownNode.UpNode = ptr;

                    // After linking row node, increment the node count in column header.
                    ptr.TopNode.ColumnLength++;
                }
            }

            // Link the column header from it's neighbors.
            top.RightNode.LeftNode = top;
            top.LeftNode.RightNode = top;
        }

    // Static method: Search (Private).
    // RECEIVES: MatNode head (head for a sparse double linked matrix), Stack<string> solution (contains 
    //           MatNode Tag strings as puzzle positions for solution).
    // RETURNS: true if a solution has been found, else false ** Acts as a recursive method and calls
    //          itself until sparse matrix has no columns left (not constraints it need to fill, true case)
    //          or until the column with the minimum amount of MatNodes under it counts 0 (can't fill 
    //          left constraints, false case).
    private static bool Search(MatNode head, Stack<string> solution)
    {
        // True exit condition -> sparse matrix has no columns left (not constraints it need to fill).
        if (head.RightNode == head)
        {
            return true;
        }

        // Choose column deterministically -> calls GetMinCol to find head column MatNode with the minimum
        //                                    amount of MatNodes under it.
        MatNode column = GetMinCol(head);

        // True exit condition -> the column with the minimum amount of MatNodes under it counts 0 (can't fill 
        //                        left constraints).
        if (column.ColumnLength == 0)
        {
            return false;
        }

        // Cover chosen column.
        Cover(column);

        // Moving on all rows belonging to column.
        for (MatNode row = column.DownNode; row != column; row = row.DownNode)
        {
            // Adds position value (row tag as "candidate") to solution stack.
            solution.Push(row.Tag);

            // Coving row nodes in column -> in case position value leads to a result.
            for (MatNode pos = row.RightNode; pos != row; pos = pos.RightNode)
            {
                if (pos.TopNode != head)
                {
                    Cover(pos);
                }
            }

            // If solution was found from position value -> return all the way back to the original other Search call
            if (Search(head, solution))
            {
                return true;
            }

            // If position value didn't lead to result:

            // Uncoving row nodes in column -> as it is found position value didn't lead to result.
            for (MatNode pos = row.LeftNode; pos != row; pos = pos.LeftNode)
            {
                if (pos.TopNode != head)
                {
                    Uncover(pos);
                }
            }

            // Pops position value (row tag as "candidate") from solution stack (to remove it).
            solution.Pop();
        }

        // If column didn't uncover the solution, uncover the chosen column the return false (solution not found).
        Uncover(column);

        return false;
    }

    // Static method: Search (Private) -> Calls the other Search static method.
    // RECEIVES: MatNode head (head for a sparse double linked matrix).
    // RETURNS: Stack<string> (which contains MatNode Tag strings as puzzle positions for solution).
    private static Stack<string> Search(MatNode head)
    {
        // Constructs new Stack<string> instance -> solutions.
        var solution = new Stack<string>();

        // Call the other Search class with solutions passed to it, for it to be filled with positions values
        // (row tags) if a solution is available for the passed board.
        Search(head, solution);

        // Returns solution stack.
        return solution;
    }

    // Static method: UpdatePuzzle (Private).
    // RECEIVES: SudokuPuzzle sp (SudokuPuzzle class reference, with included puzzle details), 
    //           Stack<string> (which contains MatNode Tag strings as puzzle positions for solution).
    // RETURNS: true if solution stack had tag position values, else false ** Updates SudokuPuzzle sp
    //          to solution if a solution is found **.
    private static bool UpdatePuzzle(SudokuPuzzle sp, Stack<string> solution)
    {
        // String positions -> sliced to find cell position and its value on Sudoku puzzle board.
        string pos;

        // String[] cordinations -> positions sliced into cordinations -> index: 1-Row 2-Column 3-Value.
        string[] cordinations;

        // Positions amount for values store in the solution stack.
        var positions = solution.Count;

        // If solution stack is empty -> return false (exit without updating anything).
        if (positions == 0)
        {
            return false;
        }

        // While there are still position values in the solution stack.
        while (positions > 0)
        {
            // Pop position value from solution stack.
            pos = solution.Pop();

            // Split (slice) position string to its values of Row / Column / Value.
            cordinations = pos.Split('R', 'C', '#');

            // Update Sudoku matrix board accordingly.
            sp.SudokuMatrix[int.Parse(cordinations[1]) - 1, int.Parse(cordinations[2]) - 1] = int.Parse(cordinations[3]);

            positions--;
        }

        // Receiving the new Sudoku puzzle string after the board has been solved:

        // Object for appending a string in loops.
        var newSTR = new StringBuilder();

        // While moving on the Sudoku matrix board.
        for (var i = 0; i < sp.Side; i++)
        {
            for (var j = 0; j < sp.Side; j++)
            {
                // Append each characer value from matrix board to StringBuilder instance.
                newSTR.Append((char)(sp.SudokuMatrix[i, j] + '0'));
            }
        }

        // Set updated Sudoku puzzle string to the new found solved one.
        sp.SudokuSTR = newSTR.ToString();

        // The Sudoku puzzle has been solved -> returns true.
        return true;
    }

    // Static method: Solve.
    // RECEIVES: SudokuPuzzle sp (SudokuPuzzle class reference, with included puzzle details).
    // RETURNS: true if solution stack had tag position values, else false (from UpdatePuzzle call).
    public static bool Solve(SudokuPuzzle sp)
    {
        // Inits sparse double linked matrix to all availalbe candidates to complete the Sudoku puzzle.
        MatNode head = FillDoubleLinkedListMatrix(sp);

        // Exit condition -> sparse matrix has no columns left (not constraints it need to fill).
        // Puzzle was entered solved!
        if (head.RightNode == head)
        {
            return true;
        }

        // Finds tag (strings) row solutions and returns them in a stack.
        Stack<string> solution = Search(head);

        // If a valid solution exists (solution stack is not empty) -> update SudokuPuzzle object accordingly.
        return UpdatePuzzle(sp, solution);
    }
}

