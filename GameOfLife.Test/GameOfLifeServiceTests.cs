using GameOfLife.Controllers;
using GameOfLife.Models;
using GameOfLife.Repositories;
using GameOfLife.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GameOfLifeTests
{
    [TestClass]
    public class GameOfLifeServiceTests
    {
        private GameOfLifeService _service;
        private Mock<IGameOfLifeRepository> _mockRepository;
        private Mock<ILogger<GameOfLifeService>> _mockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IGameOfLifeRepository>();
            _mockLogger = new Mock<ILogger<GameOfLifeService>>();
            _service = new GameOfLifeService(_mockRepository.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void Test_AddBoard()
        {
            var board = new GameOfLifeBoard
            {
                State = new int[][]
                {
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 }
                },
                Rows = 3,
                Columns = 3
            };

            var id = _service.AddBoard(board);
            Assert.IsNotNull(id);
            _mockRepository.Verify(r => r.SaveBoard(It.Is<GameOfLifeBoard>(b => b.Id == id)), Times.Once);
        }

        [TestMethod]
        public void Test_GetNextState()
        {
            var board = new GameOfLifeBoard
            {
                Id = "test-board",
                State =
                [
                    [0, 1, 0],
                    [0, 1, 0],
                    [0, 1, 0]
                ],
                Rows = 3,
                Columns = 3
            };

            var expectedState = new int[][]
            {
                [0, 0, 0],
                [1, 1, 1],
                [0, 0, 0]
            };

            _mockRepository.Setup(r => r.LoadBoard(board.Id)).Returns(board);

            var nextState = _service.GetNextState(board);

            for (int i = 0; i < expectedState.Length; i++)
            {
                CollectionAssert.AreEqual(expectedState[i], nextState.State[i]);
            }
        }

        [TestMethod]
        public void Test_GetNthState()
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

            var expectedState = new int[][]
            {
                new int[] { 0, 1, 0 },
                new int[] { 0, 1, 0 },
                new int[] { 0, 1, 0 }
            };

            _mockRepository.Setup(r => r.LoadBoard(board.Id)).Returns(board);

            var nthState = _service.GetNthState(board, 2);

            for (int i = 0; i < expectedState.Length; i++)
            {
                CollectionAssert.AreEqual(expectedState[i], nthState.State[i]);
            }
        }

        [TestMethod]
        public void Test_GetFinalState_UnstableState()
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

            _mockRepository.Setup(r => r.LoadBoard(board.Id)).Returns(board);

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _service.GetFinalState(board, 2);
            });
        }

        [TestMethod]
        public void Test_GetFinalState_StableState()
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

            _mockRepository.Setup(r => r.LoadBoard(board.Id)).Returns(board);

            var finalState = _service.GetFinalState(board, 10);
            Assert.IsNotNull(finalState);
        }
    }
}