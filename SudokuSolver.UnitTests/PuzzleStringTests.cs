using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SudokuSolver;

namespace SudokuSolver.UnitTests
{
    // Unit Tests for custom unit scenarios
    [TestClass]
    public class PuzzleStringTests
    {
        [TestMethod]
        public void Solve_1x1_EMPTY_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 1x1 Sudoku:
            var puzzleString = "0";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_1x1_FULL_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 1x1 Sudoku:
            var puzzleString = "1";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_4x4_Solvable1_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 4x4 Sudoku:
            var puzzleString = "0000" +
                               "0000" +
                               "0000" +
                               "0000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_4x4_UnvalidChar_ThrewException()
        {
            // Arrange:
            // Puzzle string is an unvalid 4x4 Sudoku:
            var puzzleString = "0000" +
                               "0050" +
                               "0000" +
                               "0000";

            // Act:
            try
            {
                // Expected to throw exception - 5 not a valid char for the specific Sudoku puzzle.
                var sp = new SudokuPuzzle(puzzleString);
                Assert.Fail();
            }
            catch
            {
                // Assert:
                // True expected.
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Solve_4x3_UnvalidLen_ThrewException()
        {
            // Arrange:
            // Puzzle string is an unvalid 4x4 Sudoku:
            var puzzleString = "0000" +
                               "0000" +
                               "0000";

            // Act:
            try
            {
                // Expected to throw exception - not a valid length for a Sudoku puzzle.
                var sp = new SudokuPuzzle(puzzleString);
                Assert.Fail();
            }
            catch
            {
                // Assert:
                // True expected.
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Solve_4x4_SOLVED_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 4x4 Sudoku:
            var puzzleString = "1324" +
                               "2413" +
                               "3142" +
                               "4231";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_9x9_Solvable1_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 9x9 Sudoku:
            var puzzleString = "000000000" +
                               "000000000" +
                               "000000000" +
                               "000000000" +
                               "000000000" +
                               "000000000" +
                               "000000000" +
                               "000000000" +
                               "000000000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_9x9_Solvable2_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 9x9 Sudoku:
            var puzzleString = "000801000" +
                               "000000043" +
                               "500000000" +
                               "000070800" +
                               "020030000" +
                               "000000100" +
                               "600000075" +
                               "003400000" +
                               "000200600";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_9x9_Solvable3_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 9x9 Sudoku:
            var puzzleString = "000060200" +
                               "001050000" +
                               "040000000" +
                               "600000500" +
                               "000100020" +
                               "003700000" +
                               "000401007" +
                               "800000300" +
                               "020000000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_9x9_Solvable4_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 9x9 Sudoku:
            var puzzleString = "800000000" +
                               "003600000" +
                               "070090200" +
                               "050007000" +
                               "000045700" +
                               "000100030" +
                               "001000068" +
                               "008500010" +
                               "090000400";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_9x9_Unsolvable1_ReturnFalse()
        {
            // Arrange:
            // Puzzle string is an unvalid 9x9 Sudoku:
            var puzzleString = "837050000" +
                               "246173985" +
                               "951020000" +
                               "328597460" +
                               "674030100" +
                               "195060000" +
                               "509080073" +
                               "402010000" +
                               "703040009";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // False expected.
            Assert.IsFalse(solved);
        }

        [TestMethod]
        public void Solve_9x9_Unsolvable2_ReturnFalse()
        {
            // Arrange:
            // Puzzle string is an unvalid 9x9 Sudoku:
            var puzzleString = "516849732" +
                               "307605000" +
                               "809700065" +
                               "135060907" +
                               "472591006" +
                               "968370050" +
                               "253186074" +
                               "684207500" +
                               "791050608";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // False expected.
            Assert.IsFalse(solved);
        }

        [TestMethod]
        public void Solve_9x9_SOLVED_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 9x9 Sudoku:
            var puzzleString = "123456789" +
                               "789123456" +
                               "456789123" +
                               "312845967" +
                               "697312845" +
                               "845697312" +
                               "231574698" +
                               "968231574" +
                               "574968231";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_9x9_UnvalidChar_ThrewException()
        {
            // Arrange:
            // Puzzle string is an unvalid 9x9 Sudoku:
            var puzzleString = "123456789" +
                               "789123456" +
                               "456789123" +
                               "31284?967" +
                               "697312845" +
                               "845697312" +
                               "231574698" +
                               "9A8231574" +
                               "574968231";

            // Act:
            try
            {
                // Expected to throw exception - ? not a valid char for the specific Sudoku puzzle.
                var sp = new SudokuPuzzle(puzzleString);
                Assert.Fail();
            }
            catch
            {
                // Assert:
                // True expected.
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Solve_82chars_UnvalidLen_ThrewException()
        {
            // Arrange:
            // Puzzle string is an unvalid 9x10 Sudoku:
            var puzzleString = "123456789" +
                               "789123456" +
                               "456789123" +
                               "312845967" +
                               "697312845" +
                               "845697312" +
                               "231574698" +
                               "968231574" +
                               "574968231" +
                               "42069nice";

            // Act:
            try
            {
                // Expected to throw exception - not a valid length for a Sudoku puzzle.
                var sp = new SudokuPuzzle(puzzleString);
                Assert.Fail();
            }
            catch
            {
                // Assert:
                // True expected.
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Solve_16x16_Solvable1_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 16x16 Sudoku:
            var puzzleString = "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000" +
                               "0000000000000000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_16x16_Solvable2_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 16x16 Sudoku:
            var puzzleString = "5000000000000002" +
                               "000000:0>0000000" +
                               "0080000000000000" +
                               "0000000000006000" +
                               "00=0000000000200" +
                               "00000000000000?0" +
                               ";000000000000000" +
                               "0003000000@80000" +
                               "0000000000000800" +
                               "0000000>00000000" +
                               "0000000000000700" +
                               "0200000003000000" +
                               "0000000000000080" +
                               "3000000000000000" +
                               "0000000000000040" +
                               "000?0@=000000000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_16x16_Unsolvable1_ReturnFalse()
        {
            // Arrange:
            // Puzzle string is an unvalid 16x16 Sudoku:
            var puzzleString = "17062<;:3080=00?" +
                               "0000@703=01000<5" +
                               "050@0806<0004000" +
                               ":00;0000000700>0" +
                               "@1030000?>0800;0" +
                               ";8:4500000003>70" +
                               "000=;04000090080" +
                               "00701000004000=0" +
                               "5>0070000:26000@" +
                               "00000:0000042901" +
                               "00<?000000316000" +
                               "9=08<00000000000" +
                               "00174300:00?0560" +
                               "0090005>;00000@4" +
                               "00000?<020000=00" +
                               "20000@0000007000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // False expected.
            Assert.IsFalse(solved);
        }

        [TestMethod]
        public void Solve_25x25_Solvable1_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 25x25 Sudoku:
            var puzzleString = "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000" +
                               "0000000000000000000000000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }

        [TestMethod]
        public void Solve_36x36_Solvable1_ReturnTrue()
        {
            // Arrange:
            // Puzzle string is a valid 36x36 Sudoku:
            var puzzleString = "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000" +
                               "000000000000000000000000000000000000";

            SudokuPuzzle sp = new SudokuPuzzle(puzzleString);

            // Act:
            bool solved = sp.Solve();

            // Assert:
            // True expected.
            Assert.IsTrue(solved);
        }
    }
}

