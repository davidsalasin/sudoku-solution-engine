namespace SudokuSolutionEngine.Core;

/// <summary>
/// Sudoku solver using DLX (Dancing Links) algorithm - an implementation of Knuth's Algorithm X.
/// Originally named Algorithm X (and given the name "Dancing links" by Donald Knuth) 
/// is a recursive, nondeterministic, depth-first, backtracking algorithm that finds all 
/// solutions to the exact cover problem. Some of the better-known exact cover problems
/// include tiling, the n queens problem, and Sudoku. 
/// dancing links is a technique for reverting the operation of deleting a node from 
/// a circular double linked list (sparse Matrix). It is particularly useful for efficiently
/// implementing backtracking algorithms. 
/// See: https://en.wikipedia.org/wiki/Knuth%27s_Algorithm_X
/// See: https://en.wikipedia.org/wiki/Dancing_Links
/// See: https://en.wikipedia.org/wiki/Exact_cover
/// </summary>
public class DLX : ISudokuSolver
{
    /// <summary>
    /// Builds a sparse doubly-linked matrix representing all possible Sudoku placements and constraints.
    /// </summary>
    private static DancingLinksNode BuildMatrix(Sudoku sudoku)
    {
        int side = sudoku.Side;
        int root = sudoku.RootSquareSide;
        int square = side * side;
        int totalRows = side * square + 1;
        int totalCols = square * 4 + 1;

        DancingLinksNode[,] matrix = new DancingLinksNode[totalRows, totalCols];
        DancingLinksNode head = new DancingLinksNode("Head");
        matrix[0, 0] = head;
        head.Size = side * square;

        CreateConstraintColumns(matrix, head, side, square);
        CreateCandidateRows(matrix, head, sudoku.Board, side, root, square);
        RemovePreFilledCells(matrix, sudoku.Board, side, square);

        return head;
    }

    /// <summary>
    /// Creates all constraint column headers: Row-Column, Row-Value, Column-Value, and Box-Value.
    /// </summary>
    private static void CreateConstraintColumns(DancingLinksNode[,] matrix, DancingLinksNode head, int side, int square)
    {
        CreateRowColumnConstraints(matrix, head, side, square);
        CreateRowValueConstraints(matrix, head, side, square);
        CreateColumnValueConstraints(matrix, head, side, square);
        CreateBoxValueConstraints(matrix, head, side, square);
    }

    /// <summary>
    /// Creates Row-Column constraint columns (each cell must contain exactly one value).
    /// </summary>
    private static void CreateRowColumnConstraints(DancingLinksNode[,] matrix, DancingLinksNode head, int side, int square)
    {
        int row = 1;
        int column = 0;

        for (int i = 1; i <= square; i++)
        {
            column++;
            if (column > side)
            {
                column = 1;
                row++;
            }

            DancingLinksNode columnHeader = new DancingLinksNode($"R{row}C{column}");
            matrix[0, i] = columnHeader;
            columnHeader.Size = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Creates Row-Value constraint columns (each row must contain each value exactly once).
    /// </summary>
    private static void CreateRowValueConstraints(DancingLinksNode[,] matrix, DancingLinksNode head, int side, int square)
    {
        int row = 1;
        int value = 0;

        for (int i = 1; i <= square; i++)
        {
            value++;
            if (value > side)
            {
                value = 1;
                row++;
            }

            DancingLinksNode columnHeader = new DancingLinksNode($"R{row}#{value}");
            matrix[0, square + i] = columnHeader;
            columnHeader.Size = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Creates Column-Value constraint columns (each column must contain each value exactly once).
    /// </summary>
    private static void CreateColumnValueConstraints(DancingLinksNode[,] matrix, DancingLinksNode head, int side, int square)
    {
        int column = 1;
        int value = 0;

        for (int i = 1; i <= square; i++)
        {
            value++;
            if (value > side)
            {
                value = 1;
                column++;
            }

            DancingLinksNode columnHeader = new DancingLinksNode($"C{column}#{value}");
            matrix[0, 2 * square + i] = columnHeader;
            columnHeader.Size = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Creates Box-Value constraint columns (each box must contain each value exactly once).
    /// </summary>
    private static void CreateBoxValueConstraints(DancingLinksNode[,] matrix, DancingLinksNode head, int side, int square)
    {
        int box = 1;
        int value = 0;

        for (int i = 1; i <= square; i++)
        {
            value++;
            if (value > side)
            {
                value = 1;
                box++;
            }

            DancingLinksNode columnHeader = new DancingLinksNode($"B{box}#{value}");
            matrix[0, 3 * square + i] = columnHeader;
            columnHeader.Size = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Links a column header to the head node in the horizontal linked list.
    /// </summary>
    private static void LinkColumnToHead(DancingLinksNode columnHeader, DancingLinksNode head)
    {
        columnHeader.Left = head.Left;
        columnHeader.Right = head;
        head.Left.Right = columnHeader;
        head.Left = columnHeader;
    }

    /// <summary>
    /// Creates all candidate rows representing possible placements (row, column, value).
    /// </summary>
    private static void CreateCandidateRows(DancingLinksNode[,] matrix, DancingLinksNode head, byte[,] puzzleBoard, int side, int root, int square)
    {
        int row = 1;
        int column = 1;
        int value = 0;

        for (int i = 1; i <= square * side; i++)
        {
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

            DancingLinksNode rowHeader = new DancingLinksNode($"R{row}C{column}#{value}");
            matrix[i, 0] = rowHeader;

            LinkRowToHead(rowHeader, head);
            CreateConstraintNodesForCandidate(matrix, i, rowHeader, row, column, value, side, root, square);
        }
    }

    /// <summary>
    /// Links a row header to the head node in the vertical linked list.
    /// </summary>
    private static void LinkRowToHead(DancingLinksNode rowHeader, DancingLinksNode head)
    {
        rowHeader.Up = head.Up;
        rowHeader.Down = head;
        head.Up.Down = rowHeader;
        head.Up = rowHeader;
        rowHeader.ColumnHeader = head;
    }

    /// <summary>
    /// Creates the four constraint nodes for a candidate (Row-Column, Row-Value, Column-Value, Box-Value).
    /// </summary>
    private static void CreateConstraintNodesForCandidate(DancingLinksNode[,] matrix, int rowIndex, DancingLinksNode rowHeader, int row, int column, int value, int side, int root, int square)
    {
        string label = $"R{row}C{column}#{value}";

        // Row-Column constraint
        int headerIndex = side * (row - 1) + column;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, label);

        // Row-Value constraint
        headerIndex = square + side * (row - 1) + value;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, label);

        // Column-Value constraint
        headerIndex = 2 * square + side * (column - 1) + value;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, label);

        // Box-Value constraint
        int boxIndex = ((row - 1) / root * root) + (column - 1) / root;
        headerIndex = 3 * square + side * boxIndex + value;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, label);
    }

    /// <summary>
    /// Creates a single constraint node and links it into the matrix.
    /// </summary>
    private static void CreateConstraintNode(DancingLinksNode[,] matrix, int rowIndex, int colIndex, DancingLinksNode rowHeader, string label)
    {
        DancingLinksNode columnHeader = matrix[0, colIndex];
        matrix[rowIndex, colIndex] = new DancingLinksNode(
            label,
            columnHeader.Up,
            columnHeader,
            rowHeader,
            rowHeader.Left,
            columnHeader
        );
    }

    /// <summary>
    /// Removes pre-filled cells from the matrix by covering their constraints.
    /// </summary>
    private static void RemovePreFilledCells(DancingLinksNode[,] matrix, byte[,] puzzleBoard, int side, int square)
    {
        for (int row = 0; row < side; row++)
        {
            for (int column = 0; column < side; column++)
            {
                byte cellValue = puzzleBoard[row, column];
                if (cellValue != 0)
                {
                    int rowIndex = cellValue + side * column + square * row;
                    DancingLinksNode candidateRow = matrix[rowIndex, 0];

                    // Remove the candidate row from vertical list
                    candidateRow.Up.Down = candidateRow.Down;
                    candidateRow.Down.Up = candidateRow.Up;

                    // Cover all constraints satisfied by this candidate
                    for (DancingLinksNode constraint = candidateRow.Right; constraint != candidateRow; constraint = constraint.Right)
                    {
                        Cover(constraint);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Finds the column with the minimum number of nodes (optimization for Algorithm X).
    /// </summary>
    private static DancingLinksNode FindMinimumColumn(DancingLinksNode head)
    {
        DancingLinksNode minColumn = head.Right;

        for (DancingLinksNode column = minColumn.Right; column != head; column = column.Right)
        {
            if (column.Size < minColumn.Size)
            {
                minColumn = column;
            }
        }

        return minColumn;
    }

    /// <summary>
    /// Covers a column and all rows that satisfy its constraint.
    /// </summary>
    private static void Cover(DancingLinksNode node)
    {
        DancingLinksNode columnHeader = node.ColumnHeader;

        // Unlink column header
        columnHeader.Right.Left = columnHeader.Left;
        columnHeader.Left.Right = columnHeader.Right;

        // Remove all rows in this column
        for (DancingLinksNode row = columnHeader.Down; row != columnHeader; row = row.Down)
        {
            for (DancingLinksNode constraint = row.Right; constraint != row; constraint = constraint.Right)
            {
                constraint.Up.Down = constraint.Down;
                constraint.Down.Up = constraint.Up;
                constraint.ColumnHeader.Size--;
            }
        }
    }

    /// <summary>
    /// Uncovers a column and restores all rows that satisfy its constraint.
    /// </summary>
    private static void Uncover(DancingLinksNode node)
    {
        DancingLinksNode columnHeader = node.ColumnHeader;

        // Restore all rows in this column (in reverse order)
        for (DancingLinksNode row = columnHeader.Up; row != columnHeader; row = row.Up)
        {
            for (DancingLinksNode constraint = row.Left; constraint != row; constraint = constraint.Left)
            {
                constraint.Up.Down = constraint;
                constraint.Down.Up = constraint;
                constraint.ColumnHeader.Size++;
            }
        }

        // Relink column header
        columnHeader.Right.Left = columnHeader;
        columnHeader.Left.Right = columnHeader;
    }

    /// <summary>
    /// Recursive search for a solution using Algorithm X.
    /// </summary>
    private static bool Search(DancingLinksNode head, Stack<string> solution)
    {
        // Solution found: no constraints remaining
        if (head.Right == head)
        {
            return true;
        }

        DancingLinksNode column = FindMinimumColumn(head);

        // No solution possible: column has no candidates
        if (column.Size == 0)
        {
            return false;
        }

        Cover(column);

        // Try each candidate row in this column
        for (DancingLinksNode row = column.Down; row != column; row = row.Down)
        {
            solution.Push(row.Label);

            // Cover all constraints satisfied by this row
            for (DancingLinksNode constraint = row.Right; constraint != row; constraint = constraint.Right)
            {
                if (constraint.ColumnHeader != head)
                {
                    Cover(constraint);
                }
            }

            // Recursively search for solution
            if (Search(head, solution))
            {
                return true;
            }

            // Backtrack: uncover constraints
            for (DancingLinksNode constraint = row.Left; constraint != row; constraint = constraint.Left)
            {
                if (constraint.ColumnHeader != head)
                {
                    Uncover(constraint);
                }
            }

            solution.Pop();
        }

        Uncover(column);
        return false;
    }

    /// <summary>
    /// Initiates the search and returns the solution stack.
    /// </summary>
    private static Stack<string> Search(DancingLinksNode head)
    {
        var solution = new Stack<string>();
        Search(head, solution);
        return solution;
    }

    /// <summary>
    /// Applies the solution to the Sudoku board.
    /// </summary>
    private static bool ApplySolution(Sudoku sudoku, Stack<string> solution)
    {
        if (solution.Count == 0)
        {
            return false;
        }

        while (solution.Count > 0)
        {
            string position = solution.Pop();
            string[] parts = position.Split('R', 'C', '#');

            int row = int.Parse(parts[1]) - 1;
            int column = int.Parse(parts[2]) - 1;
            byte value = (byte)int.Parse(parts[3]);

            sudoku.Board[row, column] = value;
        }

        return true;
    }

    /// <summary>
    /// Solves the Sudoku puzzle using the DLX algorithm.
    /// </summary>
    public bool Solve(Sudoku sudoku)
    {
        DancingLinksNode head = BuildMatrix(sudoku);

        // Puzzle already solved
        if (head.Right == head)
        {
            return true;
        }

        Stack<string> solution = Search(head);
        return ApplySolution(sudoku, solution);
    }
}

/// <summary>
/// Represents a node in a sparse doubly-linked matrix for the DLX (Dancing Links) algorithm.
/// </summary>
internal class DancingLinksNode
{
    public string Label { get; set; }
    public int Size { get; set; }
    public DancingLinksNode ColumnHeader { get; set; }
    public DancingLinksNode Up { get; set; }
    public DancingLinksNode Down { get; set; }
    public DancingLinksNode Right { get; set; }
    public DancingLinksNode Left { get; set; }

    /// <summary>
    /// Creates an isolated node with all links pointing to itself.
    /// </summary>
    public DancingLinksNode(string label)
    {
        Label = label;
        Up = this;
        Down = this;
        Right = this;
        Left = this;
        ColumnHeader = this;
    }

    /// <summary>
    /// Creates a node and links it into the sparse matrix structure.
    /// </summary>
    public DancingLinksNode(string label, DancingLinksNode up, DancingLinksNode down, DancingLinksNode right, DancingLinksNode left, DancingLinksNode columnHeader)
    {
        Label = label;

        Up = up;
        up.Down = this;

        Down = down;
        down.Up = this;

        Right = right;
        right.Left = this;

        Left = left;
        left.Right = this;

        ColumnHeader = columnHeader;
        columnHeader.Size++;
    }
}