using System.Diagnostics;

namespace SudokuSolver;

// Static Class DLX: Allows easy inputing of Sudoku puzzle strings of the user to SudokuPuzzle class (and solving them)
public static class SudokuPannel
{
    // Static method: LaunchInstructions (Private).
    // RETURNS: void ** Prints user instructions **.
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

    // Static method: LaunchInput.
    // RETURNS: void ** Asks user for format input and relocates his request to TextFormatEntry/FileFormatEntry 
    //          -> later asking the user if he would like to enter another board format **.
    public static void LaunchInput()
    {
        // While user wants to enter another board format flag.
        bool loopFlag = true;

        // Prints user instructions.
        LaunchInstructions();

        // Resizing the buffersize of Console for larger inputs.
        // ** For future long puzzle string inputs **.
        int bufSize = 4096;
        Stream inStream = Console.OpenStandardInput(bufSize);
        Console.SetIn(new StreamReader(inStream, Console.InputEncoding, false, bufSize));

        // While user wants to enter another board format:
        while (loopFlag)
        {
            // Asks user for the Sudoku puzzle format.
            Console.Write("\n> Enter a puzzle format type ('Text' / 'File'): ");
            string format = Console.ReadLine();

            // If input is wrong, ask user for a new input.
            if ((format != "Text") && (format != "File"))
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
            string again = Console.ReadLine();

            // If input is wrong, ask user for a new input.
            while ((again != "Yes") && (again != "No"))
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

    // Delegate method: FormatEntry.
    // SIGNATURES: RETURN -> BOOL / ENTRY (no params).
    public delegate bool FormatEntry();

    // Static method: FileFormatEntry.
    // RETURNS: true if the method call was successful (didn't crash), else false.
    public static bool FileFormatEntry()
    {
        // Asks user for the Sudoku puzzle file path.
        Console.Write("\n>> Enter puzzle format's file path: ");
        string path = Console.ReadLine();

        // Next method calls may throw these exceptions:
        // (1) FileNotFoundException.
        // (2) Exception - "Couldn't create SudokuPuzzle Class: (Puzzle string's lenght)^0.25 
        //                  isn't a natural number: *(Puzzle string's lenght)^0.25*".
        // (3) Exception - "Couldn't create SudokuPuzzle Class: Puzzle string contains invalid
        //                  charactrs for the Lenght of the Soduko puzzle string: *char* (as (int)*char*)".
        try
        {
            // Opens path file on read to get user's puzzle string.
            string puzzleString = File.ReadAllText(@path);
            // Create SudokuPuzzle instance from user's puzzle string from file path.
            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);
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
        string header = path.Split('\\')[path.Split('\\').Length - 1];
        Console.WriteLine($"\n>> Sudoku checked. Wrote possible solution to \"{header}\" at the same directory as the original .txt file\n");

        // Return "uncrashed" value.
        return true;
    }

    // Static method: TextFormatEntry.
    // RETURNS: true if the method call was successful (didn't crash), else false.
    public static bool TextFormatEntry()
    {
        // Asks user for the Sudoku puzzle string.
        Console.Write("\n>> Enter text string puzzle format:\n");
        string puzzleString = Console.ReadLine();

        // Next method calls may throw these exceptions:
        // (1) Exception - "Couldn't create SudokuPuzzle Class: (Puzzle string's lenght)^0.25 
        //                  isn't a natural number: *(Puzzle string's lenght)^0.25*".
        // (2) Exception - "Couldn't create SudokuPuzzle Class: Puzzle string contains invalid
        //                  charactrs for the Lenght of the Soduku puzzle string: *char* (as (int)*char*)".
        try
        {
            // Create SudokuPuzzle instance from user's puzzle string input.
            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);
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

    // Static method: SolvePuzzle (Private) -> Calls the other SolvePuzzle static method.
    // RECEIVES: SudokuPuzzle sp (SudokuPuzzle class reference, with included puzzle details).
    // RETURNS: returns path string of file solution ** Gets called upon to redirect console's output to
    //          solution's file path (writes solution to solution file) **.
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

    // Static method: SolvePuzzle (Private).
    // RECEIVES: SudokuPuzzle sp (SudokuPuzzle class reference, with included puzzle details).
    // RETURNS: void ** print SudokuPuzzle's info, and the changed info if it has been solved **.
    private static void SolvePuzzle(SudokuPuzzle sp)
    {
        // Print sudoku puzzle string.
        Console.WriteLine($"\n>>> Sudoku puzzle string input:\n{sp.SudokuSTR}\n");

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
            // Print solved sudoku puzzle string.
            Console.WriteLine($">>> Sudoku puzzle string input [SOLVED]:\n{sp.SudokuSTR}\n");
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

