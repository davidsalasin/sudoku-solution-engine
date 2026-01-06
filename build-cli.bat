@echo off
echo Building SudokuSolver.CLI...

dotnet publish SudokuSolver.CLI\SudokuSolver.CLI.csproj -c Release --self-contained -r win-x64 -p:PublishSingleFile=true -p:DebugType=None -o .

echo.
echo Done! SudokuSolver.CLI.exe is now in the root directory.

