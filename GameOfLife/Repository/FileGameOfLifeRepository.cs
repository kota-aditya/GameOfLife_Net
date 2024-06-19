using GameOfLife.Models;
using System.Text.Json;

namespace GameOfLife.Repositories
{
    public class FileGameOfLifeRepository : IGameOfLifeRepository
    {
        private readonly string _storagePath;

        public FileGameOfLifeRepository(string storagePath)
        {
            _storagePath = storagePath;
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public void SaveBoard(GameOfLifeBoard board)
        {
            var filePath = Path.Combine(_storagePath, $"{board.Id}.json");
            var json = JsonSerializer.Serialize(board);
            File.WriteAllText(filePath, json);
        }

        public GameOfLifeBoard? LoadBoard(string id)
        {
            var filePath = Path.Combine(_storagePath, $"{id}.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<GameOfLifeBoard>(json);
            }
            return null;
        }

        public void DeleteBoard(string id)
        {
            var filePath = Path.Combine(_storagePath, $"{id}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
