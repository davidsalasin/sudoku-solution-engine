using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Core;
using SudokuSolver.Core.Exceptions;

namespace SudokuSolver.Core.Tests;

[TestClass]
public class SudokuTests
{
    private static IEnumerable<object[]> GetValidInputTestCases()
    {
        yield return new object[] { new List<int> { 0 }, "Empty1x1Board_InitializesSuccessfully" };
        yield return new object[] { new List<int> { 1 }, "Completed1x1Board_InitializesSuccessfully" };
        yield return new object[] { new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Empty4x4Board_InitializesSuccessfully" };
        yield return new object[] { new List<int> { 1, 3, 2, 4, 2, 4, 1, 3, 3, 1, 4, 2, 4, 2, 3, 1 }, "Completed4x4Board_InitializesSuccessfully" };
        yield return new object[] { new List<int>(new int[81]), "Empty9x9Board_InitializesSuccessfully" };

        var completed9x9Puzzle = new List<int>(81);
        foreach (char c in "123456789" + "789123456" + "456789123" + "312845967" + "697312845" + "845697312" + "231574698" + "968231574" + "574968231")
            if (c >= '0') completed9x9Puzzle.Add(c - '0');
        yield return new object[] { completed9x9Puzzle, "Completed9x9Board_InitializesSuccessfully" };

        yield return new object[] { new List<int>(new int[256]), "Empty16x16Board_InitializesSuccessfully" };
    }

    private static IEnumerable<object[]> GetInvalidValuesTestCases()
    {
        // 4x4 with value 5 (invalid, max is 4)
        var puzzle4x4WithInvalidValue = new List<int>(16);
        foreach (char c in "0000" + "0050" + "0000" + "0000")
            if (c >= '0') puzzle4x4WithInvalidValue.Add(c - '0');
        yield return new object[] { puzzle4x4WithInvalidValue, "4x4BoardWithValueExceedingMax_ThrowsInvalidSudokuValueException" };

        // 9x9 with invalid characters '?' and 'A' converted to integers
        var puzzle9x9WithInvalidCharacters = new List<int>(81);
        foreach (char c in "123456789" + "789123456" + "456789123" + "31284?967" + "697312845" + "845697312" + "231574698" + "9A8231574" + "574968231")
            if (c >= '0') puzzle9x9WithInvalidCharacters.Add(c - '0');
        yield return new object[] { puzzle9x9WithInvalidCharacters, "9x9BoardWithInvalidCharacters_ThrowsInvalidSudokuValueException" };
    }

    private static IEnumerable<object[]> GetInvalidBoardSizeTestCases()
    {
        // 4x3 (12 elements, invalid size)
        var puzzle4x3InvalidSize = new List<int>(12);
        foreach (char c in "0000" + "0000" + "0000")
            if (c >= '0') puzzle4x3InvalidSize.Add(c - '0');
        yield return new object[] { puzzle4x3InvalidSize, "4x3BoardWithNonSquareSize_ThrowsInvalidSudokuBoardSizeException" };

        // 82 characters (invalid size)
        var puzzleWith82Elements = new List<int>(82);
        foreach (char c in "123456789" + "789123456" + "456789123" + "312845967" + "697312845" + "845697312" + "231574698" + "968231574" + "574968231" + "42069nice")
            if (c >= '0') puzzleWith82Elements.Add(c - '0');
        yield return new object[] { puzzleWith82Elements, "BoardWith82Elements_ThrowsInvalidSudokuBoardSizeException" };
    }

    [DataTestMethod]
    [DynamicData(nameof(GetValidInputTestCases), DynamicDataSourceType.Method)]
    public void Constructor_GivenValidInput_InitializesCorrectly(List<int> boardData, string testCaseName)
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
    public void Constructor_GivenInvalidValues_ThrowsInvalidSudokuValueException(List<int> boardData, string testCaseName)
    {
        // Act & Assert:
        Assert.ThrowsException<InvalidSudokuValueException>(() => new Sudoku(boardData));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetInvalidBoardSizeTestCases), DynamicDataSourceType.Method)]
    public void Constructor_GivenInvalidBoardSize_ThrowsInvalidSudokuBoardSizeException(List<int> boardData, string testCaseName)
    {
        // Act & Assert:
        Assert.ThrowsException<InvalidSudokuBoardSizeException>(() => new Sudoku(boardData));
    }
}
