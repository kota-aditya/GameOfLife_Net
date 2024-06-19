using GameOfLife.Models;

namespace GameOfLife.Services
{
    public interface IGameOfLifeService
    {
        string AddBoard(GameOfLifeBoard board);
        GameOfLifeBoard? GetBoard(string id);
        GameOfLifeBoard GetNextState(GameOfLifeBoard board);
        GameOfLifeBoard GetNthState(GameOfLifeBoard board, int n);
        GameOfLifeBoard GetFinalState(GameOfLifeBoard board, int maxSteps);
    }
}
