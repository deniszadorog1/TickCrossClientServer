namespace TickCrossServer.Timers
{
    public class TimerStorage
    {
        private static readonly Dictionary<int, MoveTimer> _timers = new();
        private static readonly object _lock = new();

        public static void StartTimerForGame(int gameId, TimeSpan timePerMove)
        {
            lock (_lock)
            {
                if (!_timers.ContainsKey(gameId))
                {
                    var timer = new MoveTimer(gameId, timePerMove);
                    _timers.Add(gameId, timer);
                }
                else
                {
                    _timers[gameId].StartNewTurn();
                }
            }
        }

        public static void StopTimer(int gameId)
        {
            lock (_lock)
            {
                if (_timers.ContainsKey(gameId))
                {
                    MoveTimer timer = _timers.Where(x => x.Key == gameId).FirstOrDefault().Value;
                    if (timer is null) return;
                }
            }
        }

        public static TimeSpan? GetTimeLeft(int gameId)
        {
            lock (_lock)
            {
                if (_timers.TryGetValue(gameId, out var timer)) return timer.GetTimeLeft();
                return null;
            }
        }
    }
}
