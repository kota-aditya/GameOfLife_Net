namespace GameOfLife.Models
{
    public class GameOfLifeBoard
    {
        public string Id { get; set; }
        public int[][] State { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
