using System.Windows;
using System.Windows.Controls;
using TickCrossClient.Services;

namespace TickCrossClient.Pages.GameReqs
{
    /// <summary>
    /// Логика взаимодействия для GotGameRequest.xaml
    /// </summary>
    public partial class GotGameRequest : Page
    {
        private Frame _frame;
        TickCrossLib.Models.GameRequest _req;
        private TickCrossLib.Models.User _user;

        public GotGameRequest(Frame frame, TickCrossLib.Models.GameRequest req,
            TickCrossLib.Models.User user)
        {
            _req = req;
            _frame = frame;
            _user = user;

            InitializeComponent();

            SetBasicParams();
        }

        public async void SetBasicParams()
        {
            EnemyLogin.Text = _user.Login == _req.Sender.Login ? _req.Receiver.Login : _req.Sender.Login;
        }

        private void DeclineReq_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();
        }

        private async void AcceptReq_Click(object sender, RoutedEventArgs e)
        {
           
            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();

            List<char>? signs = await ApiService.GetSigns();
            if (signs is null || _req is null) return;

            TickCrossLib.Models.Game game = new TickCrossLib.Models.Game((TickCrossLib.Models.GameRequest)_req, signs);

            await ApiService.SetRequestStatus(_req, TickCrossLib.Enums.RequestStatus.Accepted);

            await ApiService.AddGameInDB(game);

            game.AddGameId(await ApiService.GetLastGameId());


            GamePage page = new GamePage(game, _frame, _user);

            ((MainWindow)Window.GetWindow(_frame)).SetContentToMainFrame(page);
            ((MainWindow)Window.GetWindow(_frame))._timer.Stop();
            page.SetGameMoveTimer(_req);

            ApiService.AddTempGameInDB(game.Id);
        }


    }
}
