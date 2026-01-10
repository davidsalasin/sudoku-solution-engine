@echo off
echo Building sudoku-solver...

cd /d %~dp0..
dotnet publish SudokuSolutionEngine.CLI\SudokuSolutionEngine.CLI.csproj -c Release --self-contained -r win-x64 -p:PublishSingleFile=true -p:DebugType=None -o .

echo Done! sudoku-solver.exe is now in the root directory.
