using System.Diagnostics;
using SudokuSolver.Core;

namespace SudokuSolver.CLI;

/// <summary>
/// Static class that allows easy inputting of Sudoku puzzle strings from the user to SudokuPuzzle class (and solving them).
/// </summary>
public static class SudokuPannel
{
    /// <summary>
    /// Prints user instructions.
    /// </summary>
    private static void LaunchInstructions()
    {
        // User instructions:
        Console.WriteLine("#-----------------------------------------------/ Sudoku Puzzle Solver /-----------------------------------------------#\n");

        Console.WriteLine("-> Enter the puzzle format type:");
        Console.WriteLine("   (1) 'Text' (Sudoku puzzle string) -OR- (2) 'File' (path to .txt file contraining a singular Sudoku puzzle string)\n");

        Console.WriteLine("-> Enter the puzzle format:");
        Console.WriteLine("  -> if the puzzle format is valid: ");
        Console.WriteLine("     **The algorithm solves the sudoku format entered, and releases its solution if it finds one**");
        Console.WriteLine("  -> if the puzzle format isn't valid: ");
        Console.WriteLine("     **Prints to console puzzle entry error**\n");

        Console.WriteLine("-> Choose if to enter another puzzle format ");
        Console.WriteLine("   -> If so, continues until user is statisfied with the puzzel solutions\n");

        Console.WriteLine("#----------------------------------------------------------------------------------------------------------------------#");
    }

    /// <summary>
    /// Asks user for format input and redirects the request to TextFormatEntry/FileFormatEntry,
    /// then asks the user if they would like to enter another board format.
    /// </summary>
    public static void LaunchInput()
    {
        // While user wants to enter another board format flag.
        bool loopFlag = true;

        // Prints user instructions.
        LaunchInstructions();

        // Resizing the buffer size of Console for larger inputs (for future long puzzle string inputs).
        int bufSize = 4096;
        Stream inStream = Console.OpenStandardInput(bufSize);
        Console.SetIn(new StreamReader(inStream, Console.InputEncoding, false, bufSize));

        // While user wants to enter another board format:
        while (loopFlag)
        {
            // Asks user for the Sudoku puzzle format.
            Console.Write("\n> Enter a puzzle format type ('Text' / 'File'): ");
            var format = Console.ReadLine();

            // If input is wrong, ask user for a new input.
            if (format == null || (format != "Text") && (format != "File"))
            {
                Console.WriteLine("\n! ERROR: Wrong puzzle format input !");
                continue;
            }

            // Print selected input.
            Console.WriteLine($"\n> '{format}' format is chosen!");

            // Choose FormatMethod according to user's format.
            FormatEntry formatMethod = (format == "Text") ? new FormatEntry(TextFormatEntry) : new FormatEntry(FileFormatEntry);

            // Receive boolean value from running the FormatMethod.
            bool uncrashed = formatMethod();

            // Ask user if he would like to insert another format according to if the last request crashed or not.
            Console.Write("> Do you want to enter {0} puzzle? Enter ('Yes'/ 'No'): ", (uncrashed) ? "another" : "a different");
            var again = Console.ReadLine();

            // If input is wrong, ask user for a new input.
            while (again == null || (again != "Yes") && (again != "No"))
            {
                Console.WriteLine("\n! ERROR: Wrong answer input !\n");
                Console.Write("> Do you want to enter {0} puzzle? Enter ('Yes'/ 'No'): ", (uncrashed) ? "another" : "a different");
                again = Console.ReadLine();
            }

            // If the user answer is no -> exit request loop, else loop.
            if (again == "No")
            {
                loopFlag = false;
            }
        }

        Console.WriteLine("\n> Exited\n");
    }

    /// <summary>
    /// Delegate for format entry methods that return a boolean indicating success.
    /// </summary>
    public delegate bool FormatEntry();

    /// <summary>
    /// Handles file format entry for Sudoku puzzles.
    /// </summary>
    /// <returns>True if the method call was successful (didn't crash), otherwise false.</returns>
    public static bool FileFormatEntry()
    {
        // Asks user for the Sudoku puzzle file path.
        Console.Write("\n>> Enter puzzle format's file path: ");
        var path = Console.ReadLine();

        // If input is wrong, ask user for a new input.
        if (path == null)
        {
            Console.WriteLine("\n! ERROR: Wrong puzzle file path input !");
            return false;
        }

        // Next method calls may throw these exceptions:
        // (1) FileNotFoundException
        // (2) Exception - "Couldn't create SudokuPuzzle Class: (Puzzle string's length)^0.25 isn't a natural number: *(Puzzle string's length)^0.25*"
        // (3) Exception - "Couldn't create SudokuPuzzle Class: Puzzle string contains invalid characters for the Length of the Sudoku puzzle string: *char* (as (int)*char*)"
        try
        {
            // Opens path file on read to get user's puzzle string.
            string puzzleString = File.ReadAllText(@path);
            // Create SudokuPuzzle instance from user's puzzle string from file path.
            var sp = new SudokuPuzzle(puzzleString);
            // Solve the SudokuPuzzle instance.
            path = SolvePuzzle(sp, path);
        }
        catch (Exception e)
        {
            // Print error's message accordingly to what had occurred.
            Console.WriteLine($"\n! ERROR: {e.Message} !\n");
            // Return "crashed" value.
            return false;
        }

        // Print solution file's path.
        var pathParts = path.Split('\\');
        var header = pathParts[^1];
        Console.WriteLine($"\n>> Sudoku checked. Wrote possible solution to \"{header}\" at the same directory as the original .txt file\n");

        // Return "uncrashed" value.
        return true;
    }

    /// <summary>
    /// Handles text format entry for Sudoku puzzles.
    /// </summary>
    /// <returns>True if the method call was successful (didn't crash), otherwise false.</returns>
    public static bool TextFormatEntry()
    {
        // Asks user for the Sudoku puzzle string.
        Console.Write("\n>> Enter text string puzzle format:\n");
        var puzzleString = Console.ReadLine();

        if (puzzleString == null)
        {
            Console.WriteLine("\n! ERROR: null puzzle string input !");
            return false;
        }

        // Next method calls may throw these exceptions:
        // (1) Exception - "Couldn't create SudokuPuzzle Class: (Puzzle string's length)^0.25 isn't a natural number: *(Puzzle string's length)^0.25*"
        // (2) Exception - "Couldn't create SudokuPuzzle Class: Puzzle string contains invalid characters for the Length of the Sudoku puzzle string: *char* (as (int)*char*)"
        try
        {
            // Create SudokuPuzzle instance from user's puzzle string input.
            var sp = new SudokuPuzzle(puzzleString);
            // Solve the SudokuPuzzle instance.
            SolvePuzzle(sp);
        }
        catch (Exception e)
        {
            // Print error's message accordingly to what had occurred.
            Console.WriteLine($"\n! ERROR: {e.Message} !\n");
            // Return "crashed" value.
            return false;
        }

        Console.WriteLine($">> Sudoku checked.\n");

        // Return "uncrashed" value.
        return true;
    }

    /// <summary>
    /// Solves the puzzle and writes the solution to a file. Redirects console output to the solution file path.
    /// </summary>
    /// <param name="sp">SudokuPuzzle class reference with included puzzle details.</param>
    /// <param name="path">Path to the original puzzle file.</param>
    /// <returns>Path string of the solution file.</returns>
    private static string SolvePuzzle(SudokuPuzzle sp, string path)
    {
        // Creates new path to solution file of the original file -> new .txt file with an addition
        // "-SOLUTION-" at the end.
        path = path.Substring(0, path.Length - 4) + " -SOLUTION-.txt";

        // Save old console's output box.
        TextWriter output = Console.Out;

        // Create a new solution file based on the created path, and open it:
        using (StreamWriter file = new StreamWriter(@path))
        {
            // Redirect console to write in the new solution file.
            Console.SetOut(file);
            // Call SolvePuzzle to save its print values in solution file.
            SolvePuzzle(sp);
            // Redirect console back to its old output box.
            Console.SetOut(output);
        }

        // Return solution file path.
        return path;
    }

    /// <summary>
    /// Solves the puzzle and prints SudokuPuzzle's info, and the changed info if it has been solved.
    /// </summary>
    /// <param name="sp">SudokuPuzzle class reference with included puzzle details.</param>
    private static void SolvePuzzle(SudokuPuzzle sp)
    {
        // Print sudoku board GUI.
        Console.WriteLine(">>> Sudoku puzzle GUI:");
        sp.Print();

        // Starts stopwatch to count time for solution.
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        // Calls on to solve SudokuPuzzle, solved is set to true or false according to being solved or not.
        bool solved = sp.Solve();

        // Stops stopwatch and creates a TimeSpan instance out of the time recorded.
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        // If Sudoku was solved:
        if (solved)
        {
            // Print solved sudoku board GUI.
            Console.WriteLine(">>> Sudoku puzzle GUI [SOLVED]:");
            sp.Print();
            // Inform of operation's successful run (print).
            Console.Write(">>> Solved the Board!");
        }
        // If Sudoku wasn't solved:
        else
        {
            // Inform of operation's unsuccessful run (print).
            Console.Write(">>> Couldn't solve the Board.");
        }

        // Print operations time : Seconds.MilliSeconds.
        Console.WriteLine(" Operation runtime: {0}.{1:000}\n", ts.Seconds, ts.Milliseconds);
    }

    // Example of a file: C:\Users\David\source\repos\OMEGA 21-12-2020 Sudoku\OMEGA 21-12-2020 Sudoku\Example1.txt
}

