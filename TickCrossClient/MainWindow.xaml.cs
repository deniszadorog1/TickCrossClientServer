using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using TickCrossClient.Pages;
using TickCrossClient.Pages.FriendPages;
using TickCrossClient.Pages.GameReqs;
using TickCrossClient.Services;

namespace TickCrossClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TickCrossLib.Models.User _loggedUser;
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Content = new Login(MainFrame);
        }

        public DispatcherTimer _timer;
        private TickCrossLib.Models.GameRequest _tempGameReq;

        private TickCrossLib.Models.GameRequest? _req;
        public void SetGameRequestTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += async (sender, e) =>
            {
                bool isPlay = await SetReceiversGamePage();
                if (isPlay) return;

                if (!(_req is null)) return;
                _req = await ApiService.GetFirstGameRequest(_loggedUser.Login);
                if (_req is null) return;

                if (_req.Sender.Id != _loggedUser.Id) SetPageForSecondFrame(new GotGameRequest(MainFrame, _req, _loggedUser));

                //If game accepted + two players are logged in - start game
                _tempGameReq = _req;
                if (_tempGameReq is null) return;
                // if (!SetGameStart().Result) return;
                //await SetGamePage();

            };
            _timer.Start();
        }

        public async Task<bool> IsGameRequestAccepted()
        {
            string? reqStatus = await ApiService.GetReqStatus(_req.Id);
            if (reqStatus is null) return false;

            TickCrossLib.Enums.RequestStatus stat = GetRequestStatByString(reqStatus);
            return stat == TickCrossLib.Enums.RequestStatus.Accepted;
        }

        /* public async Task SetGamePage()
         {
             ClearSecondaryFrame();

             List<char>? signs = await ApiService.GetSigns();
             if (signs is null || _req is null) return;

             //TickCrossLib.Models.Game game = new TickCrossLib.Models.Game((TickCrossLib.Models.GameRequest)_req, signs);

             //GamePage gamePage = new GamePage(game, MainFrame, _loggedUser);
             //SetContentToMainFrame(gamePage);

            // gamePage.SetGameMoveTimer(_req);
         }*/

        public async Task<bool> SetGameStart()
        {
            if (_tempGameReq is null) return false;

            bool isReceiverLogged = await ApiService.IsUserIsLoggedById((int)_tempGameReq.Receiver.Id); //temp user
            bool isSenderLogged = await ApiService.IsUserIsLoggedById((int)_tempGameReq.Sender.Id); //future enemy

            return isReceiverLogged && isSenderLogged;
        }

        public async Task<bool> SetReceiversGamePage()
        {
            if (_req is null) return false;
            else if (MainFrame.Content is GamePage) return true;


            string? status = await ApiService.GetReqStatus(_req.Id);
            TickCrossLib.Enums.RequestStatus stat = GetRequestStatByString((string)status);


            if (!(_req is null) && !(status is null) && _req.Sender.Id == _loggedUser.Id &&
               stat == TickCrossLib.Enums.RequestStatus.Accepted)
            {
                ClearSecondaryFrame();

                _timer.Stop();
                List<char>? signs = await ApiService.GetSigns();
                if (_req is null || signs is null) return false;

                TickCrossLib.Models.Game game = new TickCrossLib.Models.Game(_req, signs);
                game.AddGameId(await ApiService.GetLastGameId());
                GamePage page = new GamePage(game, MainFrame, _loggedUser);

                SetContentToMainFrame(page);
                page.SetGameMoveTimer(_req);
                return true;
            }
            return false;
        }

        public TickCrossLib.Enums.RequestStatus GetRequestStatByString(string request)
        {
            for (int i = 0; i <= (int)TickCrossLib.Enums.RequestStatus.InProgress; i++)
            {
                if (request == ((TickCrossLib.Enums.RequestStatus)i).ToString())
                {
                    return (TickCrossLib.Enums.RequestStatus)i;
                }
            }
            return TickCrossLib.Enums.RequestStatus.InProgress;
        }


        public void SetLoggedUser(TickCrossLib.Models.User user)
        {
            _loggedUser = user;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!MainFrame.IsEnabled) ClearSecondaryFrame();

            if (MainFrame.Content is Registration || MainFrame.Content is MainPage ||
                MainFrame.Content is UserOptions)
            {
                MainFrame.Content = new Login(MainFrame);
                e.Cancel = true;
                _timer.Stop();
                _loggedUser = null;
            }
            else if (MainFrame.Content is GamePage || MainFrame.Content is FriendsPage)
            {
                MainFrame.Content = new MainPage(MainFrame, _loggedUser);
                e.Cancel = true;
            }

        }

        public void ClearSecondaryFrame()
        {
            SecondaryFrame.Content = null;
            Canvas.SetZIndex(SecondaryFrame, -1);
            Canvas.SetZIndex(MainFrame, 100);

            MainFrame.Effect = null;
            MainFrame.IsEnabled = true;
        }


        private void MainFrame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainFrame.IsEnabled) return;

            MainFrame.Effect = null;
            MainFrame.IsEnabled = true;
            SecondaryFrame.Content = null;
        }

        private void SecondaryFrame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as FrameworkElement;
            if (clicked is Border bor && bor.Background == Brushes.Transparent)
            {
                ClearSecondaryFrame();
            }
        }

        public void SetPageForSecondFrame(Page page)
        {
            MainFrame.Effect = new BlurEffect()
            {
                Radius = 30
            };
            MainFrame.IsEnabled = false;

            Canvas.SetZIndex(SecondaryFrame, 10);
            Canvas.SetZIndex(MainFrame, -1);

            SecondaryFrame.Background = Brushes.Transparent;

            SecondaryFrame.Content = page;
        }

        public void SetContentToMainFrame(Page page)
        {
            MainFrame.Content = page;

            //Set user game params

            if (page is MainPage newPage)
            {
                newPage.SetUserGameParams();

                _req = null;
                _timer.Start();
            }
        }


    }
}