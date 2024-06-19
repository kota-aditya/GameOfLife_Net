using GameOfLife.Models;

namespace GameOfLife.Helper
{
    public static class BoardValidator
    {
        public static bool IsValid(GameOfLifeBoard board, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (board == null || board.State == null || board.State.Length == 0)
            {
                errorMessage = "Board state cannot be null or empty.";
                return false;
            }

            int rowLength = board.State[0].Length;
            foreach (var row in board.State)
            {
                if (row.Length != rowLength)
                {
                    errorMessage = "All rows must have the same number of columns.";
                    return false;
                }
                if (row.Any(cell => cell != 0 && cell != 1))
                {
                    errorMessage = "Board state can only contain 0s and 1s.";
                    return false;
                }
            }

            return true;
        }
    }

}
