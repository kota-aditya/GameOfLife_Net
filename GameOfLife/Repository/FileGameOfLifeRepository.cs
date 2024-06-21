using GameOfLife.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;

namespace GameOfLife.Repositories
{
    /// <summary>
    /// Repository class for managing Game of Life boards stored in the file system.
    /// </summary>
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

        /// <summary>
        /// Saves a board to the file system.
        /// </summary>
        /// <param name="board">The board to save.</param>
        public void SaveBoard(GameOfLifeBoard board)
        {
            var filePath = Path.Combine(_storagePath, $"{board.Id}.json");
            var json = JsonSerializer.Serialize(board);
            File.WriteAllText(filePath, json);
            _logger.LogInformation("Board with ID {BoardId} saved to {FilePath}.", board.Id, filePath);
        }

        /// <summary>
        /// Loads a board from the file system.
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <returns>The board if found; otherwise, null.</returns>
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

        /// <summary>
        /// Deletes a board from the file system.
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
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
