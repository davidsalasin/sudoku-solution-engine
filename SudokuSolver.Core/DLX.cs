namespace SudokuSolver.Core;

/// <summary>
/// Sudoku solver using DLX (Dancing Links) algorithm - an implementation of Knuth's Algorithm X.
/// Originally named Algorithm X (and given the name "Dancing links" by Donald Knuth) 
/// is a recursive, nondeterministic, depth-first, backtracking algorithm that finds all 
/// solutions to the exact cover problem. Some of the better-known exact cover problems
/// include tiling, the n queens problem, and (The one I used the algorithm for) Sudoku. 
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
    private static MatNode BuildMatrix(Sudoku sudoku)
    {
        int side = sudoku.Side;
        int root = sudoku.RootSquareSide;
        int square = side * side;
        int totalRows = side * square + 1;
        int totalCols = square * 4 + 1;

        MatNode[,] matrix = new MatNode[totalRows, totalCols];
        MatNode head = new MatNode("Head");
        matrix[0, 0] = head;
        head.ColumnLength = side * square;

        CreateConstraintColumns(matrix, head, side, square);
        CreateCandidateRows(matrix, head, sudoku.Board, side, root, square);
        RemovePreFilledCells(matrix, sudoku.Board, side, square);

        return head;
    }

    /// <summary>
    /// Creates all constraint column headers: Row-Column, Row-Value, Column-Value, and Box-Value.
    /// </summary>
    private static void CreateConstraintColumns(MatNode[,] matrix, MatNode head, int side, int square)
    {
        CreateRowColumnConstraints(matrix, head, side, square);
        CreateRowValueConstraints(matrix, head, side, square);
        CreateColumnValueConstraints(matrix, head, side, square);
        CreateBoxValueConstraints(matrix, head, side, square);
    }

    /// <summary>
    /// Creates Row-Column constraint columns (each cell must contain exactly one value).
    /// </summary>
    private static void CreateRowColumnConstraints(MatNode[,] matrix, MatNode head, int side, int square)
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

            MatNode columnHeader = new MatNode($"R{row}C{column}");
            matrix[0, i] = columnHeader;
            columnHeader.ColumnLength = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Creates Row-Value constraint columns (each row must contain each value exactly once).
    /// </summary>
    private static void CreateRowValueConstraints(MatNode[,] matrix, MatNode head, int side, int square)
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

            MatNode columnHeader = new MatNode($"R{row}#{value}");
            matrix[0, square + i] = columnHeader;
            columnHeader.ColumnLength = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Creates Column-Value constraint columns (each column must contain each value exactly once).
    /// </summary>
    private static void CreateColumnValueConstraints(MatNode[,] matrix, MatNode head, int side, int square)
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

            MatNode columnHeader = new MatNode($"C{column}#{value}");
            matrix[0, 2 * square + i] = columnHeader;
            columnHeader.ColumnLength = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Creates Box-Value constraint columns (each box must contain each value exactly once).
    /// </summary>
    private static void CreateBoxValueConstraints(MatNode[,] matrix, MatNode head, int side, int square)
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

            MatNode columnHeader = new MatNode($"B{box}#{value}");
            matrix[0, 3 * square + i] = columnHeader;
            columnHeader.ColumnLength = 0;
            LinkColumnToHead(columnHeader, head);
        }
    }

    /// <summary>
    /// Links a column header to the head node in the horizontal linked list.
    /// </summary>
    private static void LinkColumnToHead(MatNode columnHeader, MatNode head)
    {
        columnHeader.LeftNode = head.LeftNode;
        columnHeader.RightNode = head;
        head.LeftNode.RightNode = columnHeader;
        head.LeftNode = columnHeader;
    }

    /// <summary>
    /// Creates all candidate rows representing possible placements (row, column, value).
    /// </summary>
    private static void CreateCandidateRows(MatNode[,] matrix, MatNode head, byte[,] puzzleBoard, int side, int root, int square)
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

            MatNode rowHeader = new MatNode($"R{row}C{column}#{value}");
            matrix[i, 0] = rowHeader;

            LinkRowToHead(rowHeader, head);
            CreateConstraintNodesForCandidate(matrix, i, rowHeader, row, column, value, side, root, square);
        }
    }

    /// <summary>
    /// Links a row header to the head node in the vertical linked list.
    /// </summary>
    private static void LinkRowToHead(MatNode rowHeader, MatNode head)
    {
        rowHeader.UpNode = head.UpNode;
        rowHeader.DownNode = head;
        head.UpNode.DownNode = rowHeader;
        head.UpNode = rowHeader;
        rowHeader.TopNode = head;
    }

    /// <summary>
    /// Creates the four constraint nodes for a candidate (Row-Column, Row-Value, Column-Value, Box-Value).
    /// </summary>
    private static void CreateConstraintNodesForCandidate(MatNode[,] matrix, int rowIndex, MatNode rowHeader, int row, int column, int value, int side, int root, int square)
    {
        string tag = $"R{row}C{column}#{value}";

        // Row-Column constraint
        int headerIndex = side * (row - 1) + column;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, tag);

        // Row-Value constraint
        headerIndex = square + side * (row - 1) + value;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, tag);

        // Column-Value constraint
        headerIndex = 2 * square + side * (column - 1) + value;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, tag);

        // Box-Value constraint
        int boxIndex = ((row - 1) / root * root) + (column - 1) / root;
        headerIndex = 3 * square + side * boxIndex + value;
        CreateConstraintNode(matrix, rowIndex, headerIndex, rowHeader, tag);
    }

    /// <summary>
    /// Creates a single constraint node and links it into the matrix.
    /// </summary>
    private static void CreateConstraintNode(MatNode[,] matrix, int rowIndex, int colIndex, MatNode rowHeader, string tag)
    {
        MatNode columnHeader = matrix[0, colIndex];
        matrix[rowIndex, colIndex] = new MatNode(
            tag,
            columnHeader.UpNode,
            columnHeader,
            rowHeader,
            rowHeader.LeftNode,
            columnHeader
        );
    }

    /// <summary>
    /// Removes pre-filled cells from the matrix by covering their constraints.
    /// </summary>
    private static void RemovePreFilledCells(MatNode[,] matrix, byte[,] puzzleBoard, int side, int square)
    {
        for (int row = 0; row < side; row++)
        {
            for (int column = 0; column < side; column++)
            {
                byte cellValue = puzzleBoard[row, column];
                if (cellValue != 0)
                {
                    int rowIndex = cellValue + side * column + square * row;
                    MatNode candidateRow = matrix[rowIndex, 0];

                    // Remove the candidate row from vertical list
                    candidateRow.UpNode.DownNode = candidateRow.DownNode;
                    candidateRow.DownNode.UpNode = candidateRow.UpNode;

                    // Cover all constraints satisfied by this candidate
                    for (MatNode constraint = candidateRow.RightNode; constraint != candidateRow; constraint = constraint.RightNode)
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
    private static MatNode FindMinimumColumn(MatNode head)
    {
        MatNode minColumn = head.RightNode;

        for (MatNode column = minColumn.RightNode; column != head; column = column.RightNode)
        {
            if (column.ColumnLength < minColumn.ColumnLength)
            {
                minColumn = column;
            }
        }

        return minColumn;
    }

    /// <summary>
    /// Covers a column and all rows that satisfy its constraint.
    /// </summary>
    private static void Cover(MatNode node)
    {
        MatNode columnHeader = node.TopNode;

        // Unlink column header
        columnHeader.RightNode.LeftNode = columnHeader.LeftNode;
        columnHeader.LeftNode.RightNode = columnHeader.RightNode;

        // Remove all rows in this column
        for (MatNode row = columnHeader.DownNode; row != columnHeader; row = row.DownNode)
        {
            for (MatNode constraint = row.RightNode; constraint != row; constraint = constraint.RightNode)
            {
                constraint.UpNode.DownNode = constraint.DownNode;
                constraint.DownNode.UpNode = constraint.UpNode;
                constraint.TopNode.ColumnLength--;
            }
        }
    }

    /// <summary>
    /// Uncovers a column and restores all rows that satisfy its constraint.
    /// </summary>
    private static void Uncover(MatNode node)
    {
        MatNode columnHeader = node.TopNode;

        // Restore all rows in this column (in reverse order)
        for (MatNode row = columnHeader.UpNode; row != columnHeader; row = row.UpNode)
        {
            for (MatNode constraint = row.LeftNode; constraint != row; constraint = constraint.LeftNode)
            {
                constraint.UpNode.DownNode = constraint;
                constraint.DownNode.UpNode = constraint;
                constraint.TopNode.ColumnLength++;
            }
        }

        // Relink column header
        columnHeader.RightNode.LeftNode = columnHeader;
        columnHeader.LeftNode.RightNode = columnHeader;
    }

    /// <summary>
    /// Recursive search for a solution using Algorithm X.
    /// </summary>
    private static bool Search(MatNode head, Stack<string> solution)
    {
        // Solution found: no constraints remaining
        if (head.RightNode == head)
        {
            return true;
        }

        MatNode column = FindMinimumColumn(head);

        // No solution possible: column has no candidates
        if (column.ColumnLength == 0)
        {
            return false;
        }

        Cover(column);

        // Try each candidate row in this column
        for (MatNode row = column.DownNode; row != column; row = row.DownNode)
        {
            solution.Push(row.Tag);

            // Cover all constraints satisfied by this row
            for (MatNode constraint = row.RightNode; constraint != row; constraint = constraint.RightNode)
            {
                if (constraint.TopNode != head)
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
            for (MatNode constraint = row.LeftNode; constraint != row; constraint = constraint.LeftNode)
            {
                if (constraint.TopNode != head)
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
    private static Stack<string> Search(MatNode head)
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
        MatNode head = BuildMatrix(sudoku);

        // Puzzle already solved
        if (head.RightNode == head)
        {
            return true;
        }

        Stack<string> solution = Search(head);
        return ApplySolution(sudoku, solution);
    }
}
