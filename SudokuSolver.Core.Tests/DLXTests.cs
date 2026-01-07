using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SudokuSolver.Core.Tests;

[TestClass]
public class DLXTests
{
    private readonly ISudokuSolver sut = new DLX();

    private static IEnumerable<object[]> GetSolveTestCases()
    {
        yield return new object[] { new List<int> { 0 }, true, "Empty1x1Board_ReturnsTrue" };
        yield return new object[] { new List<int> { 1 }, true, "Completed1x1Board_ReturnsTrue" };
        yield return new object[] { new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, true, "Empty4x4Board_ReturnsTrue" };
        yield return new object[] { new List<int> { 1, 3, 2, 4, 2, 4, 1, 3, 3, 1, 4, 2, 4, 2, 3, 1 }, true, "Completed4x4Board_ReturnsTrue" };
        yield return new object[] { new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, true, "Empty9x9Board_ReturnsTrue" };

        var solvable9x9Puzzle2 = new List<int>(81);
        foreach (char c in "000801000" + "000000043" + "500000000" + "000070800" + "020030000" + "000000100" + "600000075" + "003400000" + "000200600")
            if (c >= '0') solvable9x9Puzzle2.Add(c - '0');
        yield return new object[] { solvable9x9Puzzle2, true, "Solvable9x9Puzzle2_ReturnsTrue" };

        var solvable9x9Puzzle3 = new List<int>(81);
        foreach (char c in "000060200" + "001050000" + "040000000" + "600000500" + "000100020" + "003700000" + "000401007" + "800000300" + "020000000")
            if (c >= '0') solvable9x9Puzzle3.Add(c - '0');
        yield return new object[] { solvable9x9Puzzle3, true, "Solvable9x9Puzzle3_ReturnsTrue" };

        var solvable9x9Puzzle4 = new List<int>(81);
        foreach (char c in "800000000" + "003600000" + "070090200" + "050007000" + "000045700" + "000100030" + "001000068" + "008500010" + "090000400")
            if (c >= '0') solvable9x9Puzzle4.Add(c - '0');
        yield return new object[] { solvable9x9Puzzle4, true, "Solvable9x9Puzzle4_ReturnsTrue" };

        var unsolvable9x9Puzzle1 = new List<int>(81);
        foreach (char c in "837050000" + "246173985" + "951020000" + "328597460" + "674030100" + "195060000" + "509080073" + "402010000" + "703040009")
            if (c >= '0') unsolvable9x9Puzzle1.Add(c - '0');
        yield return new object[] { unsolvable9x9Puzzle1, false, "Unsolvable9x9Puzzle1_ReturnsFalse" };

        var unsolvable9x9Puzzle2 = new List<int>(81);
        foreach (char c in "516849732" + "307605000" + "809700065" + "135060907" + "472591006" + "968370050" + "253186074" + "684207500" + "791050608")
            if (c >= '0') unsolvable9x9Puzzle2.Add(c - '0');
        yield return new object[] { unsolvable9x9Puzzle2, false, "Unsolvable9x9Puzzle2_ReturnsFalse" };

        yield return new object[] { new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 7, 8, 9, 1, 2, 3, 4, 5, 6, 4, 5, 6, 7, 8, 9, 1, 2, 3, 3, 1, 2, 8, 4, 5, 9, 6, 7, 6, 9, 7, 3, 1, 2, 8, 4, 5, 8, 4, 5, 6, 9, 7, 3, 1, 2, 2, 3, 1, 5, 7, 4, 6, 9, 8, 9, 6, 8, 2, 3, 1, 5, 7, 4, 5, 7, 4, 9, 6, 8, 2, 3, 1 }, true, "Completed9x9Board_ReturnsTrue" };

        // 16x16 puzzles - special characters convert to: ':'=10, ';'=11, '<'=12, '='=13, '>'=14, '?'=15, '@'=16
        var empty16x16Puzzle = new List<int>(new int[256]);
        yield return new object[] { empty16x16Puzzle, true, "Empty16x16Board_ReturnsTrue" };

        var solvable16x16Puzzle = new List<int>(256);
        var solvable16x16PuzzleString = "5000000000000002" + "000000:0>0000000" + "0080000000000000" + "0000000000006000" + "00=0000000000200" + "00000000000000?0" + ";000000000000000" + "0003000000@80000" + "0000000000000800" + "0000000>00000000" + "0000000000000700" + "0200000003000000" + "0000000000000080" + "3000000000000000" + "0000000000000040" + "000?0@=000000000";
        foreach (char c in solvable16x16PuzzleString) if (c >= '0') solvable16x16Puzzle.Add(c - '0');
        yield return new object[] { solvable16x16Puzzle, true, "Solvable16x16Puzzle_ReturnsTrue" };

        var unsolvable16x16Puzzle = new List<int>(256);
        var unsolvable16x16PuzzleString = "17062<;:3080=00?" + "0000@703=01000<5" + "050@0806<0004000" + ":00;0000000700>0" + "@1030000?>0800;0" + ";8:4500000003>70" + "000=;04000090080" + "00701000004000=0" + "5>0070000:26000@" + "00000:0000042901" + "00<?000000316000" + "9=08<00000000000" + "00174300:00?0560" + "0090005>;00000@4" + "00000?<020000=00" + "20000@0000007000";
        foreach (char c in unsolvable16x16PuzzleString) if (c >= '0') unsolvable16x16Puzzle.Add(c - '0');
        yield return new object[] { unsolvable16x16Puzzle, false, "Unsolvable16x16Puzzle_ReturnsFalse" };

        var empty25x25Puzzle = new List<int>(new int[625]); // all zeros
        yield return new object[] { empty25x25Puzzle, true, "Empty25x25Board_ReturnsTrue" };

        var empty36x36Puzzle = new List<int>(new int[1296]); // all zeros
        yield return new object[] { empty36x36Puzzle, true, "Empty36x36Board_ReturnsTrue" };
    }

    [DataTestMethod]
    [DynamicData(nameof(GetSolveTestCases), DynamicDataSourceType.Method)]
    public void Solve_GivenPuzzle_ReturnsExpectedResult(List<int> boardData, bool expectedIsSolvable, string testCaseName)
    {
        // Arrange:
        var sudoku = new Sudoku(boardData);

        // Act:
        bool actualIsSolvable = sut.Solve(sudoku);

        // Assert:
        Assert.AreEqual(expectedIsSolvable, actualIsSolvable);
    }
}
