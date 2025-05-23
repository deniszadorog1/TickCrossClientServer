namespace TickCrossServer.Timers
{
    public class MoveTimer
    {
        private Timer _timer;
        private DateTime _startTime;
        public TimeSpan TimePerMove { get; private set; }
        public TimeSpan TimeLeft { get; private set; }
        public bool IsExpired { get; private set; }

        private readonly object _lock = new();

        public int GameId { get; }

        public event Action<int> OnTimeExpired; 

        public MoveTimer(int gameId, TimeSpan timePerMove)
        {
            GameId = gameId;
            TimePerMove = timePerMove;
            StartNewTurn();
        }

        public void StartNewTurn()
        {
            const int inter = 1000;
            lock (_lock)
            {
                _startTime = DateTime.UtcNow;
                TimeLeft = TimePerMove;
                IsExpired = false;

                _timer?.Dispose();
                _timer = new Timer(Update, null, 0, inter);
            }
        }

        private void Update(object state)
        {
            lock (_lock)
            {
                var elapsed = DateTime.UtcNow - _startTime;
                TimeLeft = TimePerMove - elapsed;

                if (TimeLeft <= TimeSpan.Zero)
                {
                    TimeLeft = TimeSpan.Zero;
                    IsExpired = true;
                    _timer?.Dispose();
                    OnTimeExpired?.Invoke(GameId); 
                }
            }
        }

        public TimeSpan GetTimeLeft()
        {
            lock (_lock)
            {
                return TimeLeft;
            }
        }
    }
}
