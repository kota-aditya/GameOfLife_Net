using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Models
{
    public class GameOfLifeBoard
    {
        [Required]
        [MinLength(1, ErrorMessage = "The board must have at least one row.")]
        public int[][] State { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The number of rows must be at least 1.")]
        public int Rows { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The number of columns must be at least 1.")]
        public int Columns { get; set; }

        public string? Id { get; set; }
    }
}
