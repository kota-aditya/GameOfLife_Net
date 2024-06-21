using GameOfLife.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Validators
{
    public static class BoardValidator
    {
        /// <summary>
        /// Validates the GameOfLifeBoard.
        /// </summary>
        /// <param name="board">The board to validate.</param>
        /// <returns>A list of validation results.</returns>
        public static List<ValidationResult> Validate(GameOfLifeBoard board)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(board);

            Validator.TryValidateObject(board, context, results, true);

            // Custom validations

            // Check if the number of rows in the state matches the Rows property
            if (board.State.Length != board.Rows)
            {
                results.Add(new ValidationResult("The number of rows in the state does not match the Rows property."));
            }

            // Check if all rows in the state match the Columns property and contain only 0s and 1s
            foreach (var row in board.State)
            {
                if (row.Length != board.Columns)
                {
                    results.Add(new ValidationResult("One or more rows in the state do not match the Columns property."));
                    break; // If one row fails the check, we can break early
                }

                foreach (var cell in row)
                {
                    if (cell != 0 && cell != 1)
                    {
                        results.Add(new ValidationResult("The board can only contain 0s and 1s."));
                        break; // If one cell fails the check, we can break early
                    }
                }
            }

            return results;
        }
    }
}
