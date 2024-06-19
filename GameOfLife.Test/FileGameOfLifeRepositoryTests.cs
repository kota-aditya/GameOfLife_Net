using GameOfLife.Models;
using GameOfLife.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.Json;

namespace GameOfLifeTests
{
    [TestClass]
    public class FileGameOfLifeRepositoryTests
    {
        private string _storagePath;
        private FileGameOfLifeRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            if (Directory.Exists(_storagePath))
            {
                Directory.Delete(_storagePath, true);
            }
            Directory.CreateDirectory(_storagePath);

            _repository = new FileGameOfLifeRepository(_storagePath);
        }

        [TestMethod]
        public void Test_SaveAndLoadBoard()
        {
            var board = new GameOfLifeBoard
            {
                Id = "test-board",
                State = new int[][]
                {
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 }
                },
                Rows = 3,
                Columns = 3
            };

            _repository.SaveBoard(board);

            var loadedBoard = _repository.LoadBoard("test-board");
            Assert.IsNotNull(loadedBoard);
            Assert.AreEqual(board.Id, loadedBoard.Id);
            for (int i = 0; i < board.Rows; i++)
            {
                CollectionAssert.AreEqual(board.State[i], loadedBoard.State[i]);
            }
        }

        [TestMethod]
        public void Test_DeleteBoard()
        {
            var board = new GameOfLifeBoard
            {
                Id = "test-board",
                State = new int[][]
                {
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 }
                },
                Rows = 3,
                Columns = 3
            };

            _repository.SaveBoard(board);
            _repository.DeleteBoard(board.Id);

            var loadedBoard = _repository.LoadBoard(board.Id);
            Assert.IsNull(loadedBoard);
        }
    }
}
