using GameOfLife.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;

namespace GameOfLife.Repositories
{
    public class FileGameOfLifeRepository : IGameOfLifeRepository
    {
        private readonly string _storagePath;
        private readonly ILogger<FileGameOfLifeRepository> _logger;

        public FileGameOfLifeRepository(string storagePath, ILogger<FileGameOfLifeRepository> logger)
        {
            _storagePath = storagePath;
            _logger = logger;
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
            _logger.LogInformation("Board with ID {BoardId} saved to {FilePath}.", board.Id, filePath);
        }

        public GameOfLifeBoard? LoadBoard(string id)
        {
            var filePath = Path.Combine(_storagePath, $"{id}.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                _logger.LogInformation("Board with ID {BoardId} loaded from {FilePath}.", id, filePath);
                return JsonSerializer.Deserialize<GameOfLifeBoard>(json);
            }
            _logger.LogWarning("Board with ID {BoardId} not found in {FilePath}.", id, filePath);
            return null;
        }

        public void DeleteBoard(string id)
        {
            var filePath = Path.Combine(_storagePath, $"{id}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("Board with ID {BoardId} deleted from {FilePath}.", id, filePath);
            }
            else
            {
                _logger.LogWarning("Board with ID {BoardId} not found for deletion in {FilePath}.", id, filePath);
            }
        }
    }
}
