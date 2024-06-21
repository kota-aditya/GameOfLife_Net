using GameOfLife.Models;
using GameOfLife.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Services
{
    /// <summary>
    /// Service class for managing Game of Life boards.
    /// </summary>
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IGameOfLifeRepository _repository;
        private readonly ILogger<GameOfLifeService> _logger;

        public GameOfLifeService(IGameOfLifeRepository repository, ILogger<GameOfLifeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new board and returns its unique identifier.
        /// </summary>
        /// <param name="board">The board to add.</param>
        /// <returns>The unique identifier of the board.</returns>
        public string AddBoard(GameOfLifeBoard board)
        {
            board.Id = Guid.NewGuid().ToString();
            _repository.SaveBoard(board);
            _logger.LogInformation("Board with ID {BoardId} added.", board.Id);
            return board.Id;
        }

        /// <summary>
        /// Retrieves a board by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <returns>The board if found; otherwise, null.</returns>
        public GameOfLifeBoard? GetBoard(string id)
        {
            return _repository.LoadBoard(id);
        }

        /// <summary>
        /// Retrieves the next state of the board based on the current state.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <returns>The next state of the board.</returns>
        public GameOfLifeBoard GetNextState(GameOfLifeBoard board)
        {
            var newState = new int[board.Rows][];
            for (int i = 0; i < board.Rows; i++)
            {
                newState[i] = new int[board.Columns];
                for (int j = 0; j < board.Columns; j++)
                {
                    int aliveNeighbors = CountAliveNeighbors(board, i, j);
                    if (board.State[i][j] == 1)
                    {
                        // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                        // Any live cell with more than three live neighbours dies, as if by overpopulation.
                        // Any live cell with two or three live neighbours lives on to the next generation.
                        newState[i][j] = (aliveNeighbors < 2 || aliveNeighbors > 3) ? 0 : 1;
                    }
                    else
                    {
                        // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                        newState[i][j] = (aliveNeighbors == 3) ? 1 : 0;
                    }
                }
            }
            var nextBoard = new GameOfLifeBoard { Id = board.Id, State = newState, Rows = board.Rows, Columns = board.Columns };
            _repository.SaveBoard(nextBoard);
            _logger.LogInformation("Next state calculated for board ID {BoardId}.", board.Id);
            return nextBoard;
        }

        /// <summary>
        /// Retrieves the state of the board after a specified number of steps.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <param name="n">The number of steps to calculate.</param>
        /// <returns>The state of the board after the specified number of steps.</returns>
        public GameOfLifeBoard GetNthState(GameOfLifeBoard board, int n)
        {
            var currentState = board;
            for (int i = 0; i < n; i++)
            {
                currentState = GetNextState(currentState);
            }
            _logger.LogInformation("{NthState}th state calculated for board ID {BoardId}.", n, board.Id);
            return currentState;
        }

        /// <summary>
        /// Retrieves the final state of the board, which is a stable state or a repeating loop.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <param name="maxSteps">The maximum number of steps to calculate.</param>
        /// <returns>The final state of the board.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the board does not reach a stable state within the specified number of steps.</exception>
        public GameOfLifeBoard GetFinalState(GameOfLifeBoard board, int maxSteps)
        {
            var currentState = board;
            var seenStates = new HashSet<string>();

            for (int i = 0; i < maxSteps; i++)
            {
                // Serialize the current state to a string for loop detection
                var stateString = string.Join(",", currentState.State.Select(row => string.Join(",", row)));

                // Check if the current state has been seen before (loop detection)
                if (seenStates.Contains(stateString))
                {
                    _logger.LogInformation("Loop detected for board ID {BoardId} at step {Step}.", board.Id, i);
                    return currentState; // Loop detected, return the state before loop
                }

                // Add the current state to the set of seen states
                seenStates.Add(stateString);

                // Get the next state of the board
                currentState = GetNextState(currentState);
            }

            // If the loop completes without finding a stable state or loop, throw an exception
            _logger.LogWarning("Board ID {BoardId} did not reach a stable state within {MaxSteps} steps.", board.Id, maxSteps);
            throw new InvalidOperationException("Board does not reach a stable state within the specified max steps.");
        }

        /// <summary>
        /// Counts the number of alive neighbors for a cell at the specified position.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <param name="row">The row position of the cell.</param>
        /// <param name="col">The column position of the cell.</param>
        /// <returns>The number of alive neighbors.</returns>
        private int CountAliveNeighbors(GameOfLifeBoard board, int row, int col)
        {
            int count = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i == row && j == col) continue;
                    if (i >= 0 && i < board.Rows && j >= 0 && j < board.Columns)
                    {
                        count += board.State[i][j];
                    }
                }
            }
            return count;
        }
    }
}
