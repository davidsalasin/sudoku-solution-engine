
using CommandLine;
using SudokuSolver.CLI;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options =>    
    {        
        var output = options.Capital ? options.Input.ToUpperInvariant() : options.Input;
        Console.WriteLine(output);
    });
