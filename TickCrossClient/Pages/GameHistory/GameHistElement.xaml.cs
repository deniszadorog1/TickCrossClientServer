using System.Windows.Controls;
using TickCrossClient.Services;
using TickCrossLib.Models.NonePlayable;
using TickCrossLib.Services;

namespace TickCrossClient.Pages.GameHistory
{
    /// <summary>
    /// Логика взаимодействия для GameHistElement.xaml
    /// </summary>
    public partial class GameHistElement : UserControl
    {
        private GameResult _gameRes;
        public GameHistElement(GameResult result)
        {
            _gameRes = result;
            InitializeComponent();

            FillParams();
        }

        public void FillParams()
        {
            FirstPlayerLogin.Text = _gameRes.FirstPlayer.Login;
            SecondPlayerLogin.Text = _gameRes.SecondPlayer.Login;

            StartTime.Text = CheckTime(_gameRes.StartTime);
            EndTime.Text = CheckTime(_gameRes.EndTime);

            GameResultBlock.Text = GetGameResult();
        }

        public string GetGameResult()
        {
            string winner = JsonService.GetStringByName("GameHistElemWinnerString");
            string draw = JsonService.GetStringByName("GameHistElemDrawString");
            string canceled = JsonService.GetStringByName("GameHistElemCanceledString");

            return _gameRes.Winner is not null ? $"{winner}{_gameRes.Winner.Login}" :
                _gameRes.IsDraw is not null && (bool)_gameRes.IsDraw ? draw :
                canceled;
        }

        public string CheckTime(DateTime? time)
        {
            string defaultVal = JsonService.GetStringByName("DefaultNullValString");
            return time is null ? defaultVal :
                $"{time.Value.Day}.{time.Value.Month}.{time.Value.Year}";
        }
    }
}
