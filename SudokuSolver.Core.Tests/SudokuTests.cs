using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Core;
using SudokuSolver.Core.Exceptions;

namespace SudokuSolver.Core.Tests;

[TestClass]
public class SudokuTests
{
    private static IEnumerable<object[]> GetValidInputTestCases()
    {
        yield return new object[] { new List<byte> { 0 }, "Empty1x1Board_InitializesSuccessfully" };
        yield return new object[] { new List<byte> { 1 }, "Completed1x1Board_InitializesSuccessfully" };
        yield return new object[] { new List<byte> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Empty4x4Board_InitializesSuccessfully" };
        yield return new object[] { new List<byte> { 1, 3, 2, 4, 2, 4, 1, 3, 3, 1, 4, 2, 4, 2, 3, 1 }, "Completed4x4Board_InitializesSuccessfully" };
        yield return new object[] { new List<byte>(new byte[81]), "Empty9x9Board_InitializesSuccessfully" };

        var completed9x9Puzzle = new List<byte>(81);
        foreach (char c in "123456789" + "789123456" + "456789123" + "312845967" + "697312845" + "845697312" + "231574698" + "968231574" + "574968231")
            if (c >= '0') completed9x9Puzzle.Add((byte)(c - '0'));
        yield return new object[] { completed9x9Puzzle, "Completed9x9Board_InitializesSuccessfully" };
    }

    private static IEnumerable<object[]> GetInvalidValuesTestCases()
    {
        // 4x4 with value 5 (invalid, max is 4)
        var puzzle4x4WithInvalidValue = new List<byte>(16);
        foreach (char c in "0000" + "0050" + "0000" + "0000")
            if (c >= '0') puzzle4x4WithInvalidValue.Add((byte)(c - '0'));
        yield return new object[] { puzzle4x4WithInvalidValue, "4x4BoardWithValueExceedingMax_ThrowsInvalidSudokuValueException" };

        // 9x9 with invalid characters '?' and 'A' converted to integers
        var puzzle9x9WithInvalidCharacters = new List<byte>(81);
        foreach (char c in "123456789" + "789123456" + "456789123" + "31284?967" + "697312845" + "845697312" + "231574698" + "9A8231574" + "574968231")
            if (c >= '0') puzzle9x9WithInvalidCharacters.Add((byte)(c - '0'));
        yield return new object[] { puzzle9x9WithInvalidCharacters, "9x9BoardWithInvalidCharacters_ThrowsInvalidSudokuValueException" };
    }

    private static IEnumerable<object[]> GetInvalidBoardSizeTestCases()
    {
        // 4x3 (12 elements, invalid size)
        var puzzle4x3InvalidSize = new List<byte>(12);
        foreach (char c in "0000" + "0000" + "0000")
            if (c >= '0') puzzle4x3InvalidSize.Add((byte)(c - '0'));
        yield return new object[] { puzzle4x3InvalidSize, "4x3BoardWithNonSquareSize_ThrowsInvalidSudokuDimensionsException" };

        // 82 characters (invalid size)
        var puzzleWith82Elements = new List<byte>(82);
        foreach (char c in "123456789" + "789123456" + "456789123" + "312845967" + "697312845" + "845697312" + "231574698" + "968231574" + "574968231" + "42069nice")
            if (c >= '0') puzzleWith82Elements.Add((byte)(c - '0'));
        yield return new object[] { puzzleWith82Elements, "BoardWith82Elements_ThrowsInvalidSudokuDimensionsException" };
    }

    private static IEnumerable<object[]> GetBoardSizeLimitExceededTestCases()
    {
        // 256x256 board (65536 elements, valid shape but exceeds max size)
        yield return new object[] { new List<byte>(new byte[65536]), "256x256Board_ThrowsSudokuBoardSizeLimitExceededException" };
    }

    private static IEnumerable<object[]> GetSolverLimitExceededTestCases()
    {
        // 49x49 board (2401 elements, RootSquareSide = 7, exceeds practical solver limit of 36x36)
        yield return new object[] { new List<byte>(new byte[2401]), "49x49Board_ThrowsSudokuSolverLimitExceededException" };
        
        // 64x64 board (4096 elements, RootSquareSide = 8, exceeds practical solver limit of 36x36)
        yield return new object[] { new List<byte>(new byte[4096]), "64x64Board_ThrowsSudokuSolverLimitExceededException" };
        
        // 100x100 board (10000 elements, RootSquareSide = 10, exceeds practical solver limit of 36x36)
        yield return new object[] { new List<byte>(new byte[10000]), "100x100Board_ThrowsSudokuSolverLimitExceededException" };
    }

    [DataTestMethod]
    [DynamicData(nameof(GetValidInputTestCases), DynamicDataSourceType.Method)]
    public void Constructor_GivenValidInput_InitializesCorrectly(List<byte> boardData, string testCaseName)
    {
        // Arrange & Act:
        var sut = new Sudoku(boardData);

        // Assert:
        Assert.IsNotNull(sut);
        Assert.IsNotNull(sut.Board);
        Assert.IsTrue(sut.Side > 0);
        Assert.IsTrue(sut.RootSquareSide > 0);
        Assert.AreEqual(sut.Side, sut.RootSquareSide * sut.RootSquareSide);
        Assert.AreEqual(boardData.Count, sut.Side * sut.Side);
    }

    [DataTestMethod]
    [DynamicData(nameof(GetInvalidValuesTestCases), DynamicDataSourceType.Method)]
    public void Constructor_GivenInvalidValues_ThrowsInvalidSudokuValueException(List<byte> boardData, string testCaseName)
    {
        // Act & Assert:
        Assert.ThrowsException<InvalidSudokuValueException>(() => new Sudoku(boardData));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetInvalidBoardSizeTestCases), DynamicDataSourceType.Method)]
    public void Constructor_GivenInvalidBoardSize_ThrowsInvalidSudokuDimensionsException(List<byte> boardData, string testCaseName)
    {
        // Act & Assert:
        Assert.ThrowsException<InvalidSudokuDimensionsException>(() => new Sudoku(boardData));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetBoardSizeLimitExceededTestCases), DynamicDataSourceType.Method)]
    public void Constructor_GivenBoardSizeExceedingLimit_ThrowsSudokuBoardSizeLimitExceededException(List<byte> boardData, string testCaseName)
    {
        // Act & Assert:
        Assert.ThrowsException<SudokuBoardSizeLimitExceededException>(() => new Sudoku(boardData));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetSolverLimitExceededTestCases), DynamicDataSourceType.Method)]
    public void Constructor_GivenBoardSizeExceedingSolverLimit_ThrowsSudokuSolverLimitExceededException(List<byte> boardData, string testCaseName)
    {
        // Act & Assert:
        Assert.ThrowsException<SudokuSolverLimitExceededException>(() => new Sudoku(boardData));
    }

    [TestMethod]
    public void Constructor_GivenValidNestedInput_InitializesCorrectly()
    {
        // Arrange:
        var nestedInput = new List<IList<byte>>
        {
            new List<byte> { 1, 3, 2, 4 },
            new List<byte> { 2, 4, 1, 3 },
            new List<byte> { 3, 1, 4, 2 },
            new List<byte> { 4, 2, 3, 1 }
        };

        // Act:
        var sut = new Sudoku(nestedInput);

        // Assert:
        Assert.IsNotNull(sut);
        Assert.IsNotNull(sut.Board);
        Assert.AreEqual(4, sut.Side);
        Assert.AreEqual(2, sut.RootSquareSide);
    }

    [TestMethod]
    public void Constructor_GivenNestedInput_ProducesSameResultAsFlatInput()
    {
        // Arrange:
        var flatInput = new List<byte> { 1, 3, 2, 4, 2, 4, 1, 3, 3, 1, 4, 2, 4, 2, 3, 1 };
        var nestedInput = new List<IList<byte>>
        {
            new List<byte> { 1, 3, 2, 4 },
            new List<byte> { 2, 4, 1, 3 },
            new List<byte> { 3, 1, 4, 2 },
            new List<byte> { 4, 2, 3, 1 }
        };

        // Act:
        var sudokuFromFlat = new Sudoku(flatInput);
        var sudokuFromNested = new Sudoku(nestedInput);

        // Assert:
        Assert.AreEqual(sudokuFromFlat.Side, sudokuFromNested.Side);
        Assert.AreEqual(sudokuFromFlat.RootSquareSide, sudokuFromNested.RootSquareSide);
        
        for (int i = 0; i < sudokuFromFlat.Side; i++)
        {
            for (int j = 0; j < sudokuFromFlat.Side; j++)
            {
                Assert.AreEqual(sudokuFromFlat.Board[i, j], sudokuFromNested.Board[i, j]);
            }
        }
    }
}
