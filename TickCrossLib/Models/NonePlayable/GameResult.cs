using System;

namespace TickCrossLib.Models.NonePlayable
{
    public class GameResult
    {
        public User FirstPlayer { get; set; }
        public User SecondPlayer { get; set; }
        public User Winner { get; set; }
        public bool? IsDraw { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public GameResult(User first, User second, User winner,
            bool? isDraw, DateTime? start, DateTime? end)
        {
            FirstPlayer = first;
            SecondPlayer = second;
            Winner = winner;
            IsDraw = isDraw;
            StartTime = start;
            EndTime = end;
        }
    }
}
