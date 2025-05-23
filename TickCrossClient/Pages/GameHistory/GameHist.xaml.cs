using System.Windows;
using System.Windows.Controls;
using TickCrossClient.Services;
using TickCrossLib.Models.NonePlayable;

namespace TickCrossClient.Pages.GameHistory
{
    /// <summary>
    /// Логика взаимодействия для GameHist.xaml
    /// </summary>
    public partial class GameHist : Page
    {
        private TickCrossLib.Models.User _user;
        private Frame _frame;
        public GameHist(TickCrossLib.Models.User user, Frame frame)
        {
            _user = user;
            _frame = frame;

            InitializeComponent();

            FillHistory();
        }

        private async void FillHistory()
        {
            HistList.Items.Clear();

            List<GameResult> hist = await ApiService.GetGameHistory(_user.Id);

            for (int i = hist.Count - 1; i >= 0; i--)
            {
                GameHistElement elem = new GameHistElement(hist[i]);
                HistList.Items.Add(elem);
            }
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetContentToMainFrame(new MainPage(_frame, _user));
        }
    }
}
