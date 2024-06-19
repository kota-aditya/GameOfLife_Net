using GameOfLife.Controllers;
using GameOfLife.Models;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameOfLifeTests
{
    [TestClass]
    public class GameOfLifeControllerTests
    {
        private GameOfLifeController _controller;
        private Mock<IGameOfLifeService> _mockService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockService = new Mock<IGameOfLifeService>();
            _controller = new GameOfLifeController(_mockService.Object);
        }

        [TestMethod]
        public void Test_UploadBoard()
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

            _mockService.Setup(s => s.AddBoard(It.IsAny<GameOfLifeBoard>())).Returns("test-board");

            var result = _controller.UploadBoard(board) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Test_GetNextState()
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

            _mockService.Setup(s => s.GetBoard("test-board")).Returns(board);
            _mockService.Setup(s => s.GetNextState(board)).Returns(new GameOfLifeBoard
            {
                State = new int[][]
                {
                    new int[] { 0, 0, 0 },
                    new int[] { 1, 1, 1 },
                    new int[] { 0, 0, 0 }
                },
                Rows = 3,
                Columns = 3
            });

            var result = _controller.GetNextState("test-board") as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
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

            _mockService.Setup(s => s.GetBoard("test-board")).Returns(board);
            _mockService.Setup(s => s.GetNthState(board, 2)).Returns(new GameOfLifeBoard
            {
                State = new int[][]
                {
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 },
                    new int[] { 0, 1, 0 }
                },
                Rows = 3,
                Columns = 3
            });

            var result = _controller.GetNthState("test-board", 2) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Test_GetFinalState()
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

            _mockService.Setup(s => s.GetBoard("test-board")).Returns(board);
            _mockService.Setup(s => s.GetFinalState(board, 10)).Returns(new GameOfLifeBoard
            {
                State = new int[][]
                {
                    new int[] { 0, 0, 0 },
                    new int[] { 1, 1, 1 },
                    new int[] { 0, 0, 0 }
                },
                Rows = 3,
                Columns = 3
            });

            var result = _controller.GetFinalState("test-board", 10) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
