using GameOfLife.Models;

namespace GameOfLife.Repositories
{
    public interface IGameOfLifeRepository
    {
        void SaveBoard(GameOfLifeBoard board);
        GameOfLifeBoard? LoadBoard(string id);
        void DeleteBoard(string id);
    }
}
