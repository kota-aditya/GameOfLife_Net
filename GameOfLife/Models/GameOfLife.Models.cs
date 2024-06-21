using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Models
{
    /// <summary>
    /// Represents the state of the Game of Life board.
    /// </summary>
    public class GameOfLifeBoard
    {
        /// <summary>
        /// The current state of the board, represented as a 2D array.
        /// Each element can be either 0 (dead) or 1 (alive).
        /// </summary>
        [Required(ErrorMessage = "State is required.")]
        [MinLength(1, ErrorMessage = "The board must have at least one row.")]
        public int[][] State { get; set; }

        /// <summary>
        /// The number of rows in the board.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The number of rows must be at least 1.")]
        public int Rows { get; set; }

        /// <summary>
        /// The number of columns in the board.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The number of columns must be at least 1.")]
        public int Columns { get; set; }

        /// <summary>
        /// The unique identifier for the board.
        /// </summary>
        public string? Id { get; set; }
    }
}
