using GameOfLife.Models;
using GameOfLife.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Services
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IGameOfLifeRepository _repository;
        private readonly ILogger<GameOfLifeService> _logger;

        public GameOfLifeService(IGameOfLifeRepository repository, ILogger<GameOfLifeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public string AddBoard(GameOfLifeBoard board)
        {
            board.Id = Guid.NewGuid().ToString();
            _repository.SaveBoard(board);
            _logger.LogInformation("Board with ID {BoardId} added.", board.Id);
            return board.Id;
        }

        public GameOfLifeBoard? GetBoard(string id)
        {
            return _repository.LoadBoard(id);
        }

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
                        newState[i][j] = (aliveNeighbors < 2 || aliveNeighbors > 3) ? 0 : 1;
                    }
                    else
                    {
                        newState[i][j] = (aliveNeighbors == 3) ? 1 : 0;
                    }
                }
            }
            var nextBoard = new GameOfLifeBoard { Id = board.Id, State = newState, Rows = board.Rows, Columns = board.Columns };
            _repository.SaveBoard(nextBoard);
            _logger.LogInformation("Next state calculated for board ID {BoardId}.", board.Id);
            return nextBoard;
        }

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

        public GameOfLifeBoard GetFinalState(GameOfLifeBoard board, int maxSteps)
        {
            var currentState = board;
            var seenStates = new HashSet<string>();
            for (int i = 0; i < maxSteps; i++)
            {
                var stateString = string.Join(",", currentState.State.Select(row => string.Join(",", row)));
                if (seenStates.Contains(stateString))
                {
                    _logger.LogInformation("Loop detected for board ID {BoardId} at step {Step}.", board.Id, i);
                    return currentState; // Loop detected, return the state before loop
                }
                seenStates.Add(stateString);
                var nextState = GetNextState(currentState);
                if (IsSameState(currentState, nextState))
                {
                    _logger.LogInformation("Stable state reached for board ID {BoardId} at step {Step}.", board.Id, i);
                    return currentState; // Stable state reached
                }
                currentState = nextState;
            }
            _logger.LogWarning("Board ID {BoardId} did not reach a stable state within {MaxSteps} steps.", board.Id, maxSteps);
            throw new InvalidOperationException("Board does not reach a stable state within the specified max steps.");
        }

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

        private bool IsSameState(GameOfLifeBoard a, GameOfLifeBoard b)
        {
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (a.State[i][j] != b.State[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
