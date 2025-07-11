﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using TickCrossClient.Pages;
using TickCrossClient.Pages.FriendPages;
using TickCrossClient.Pages.GameReqs;
using TickCrossClient.Services;
using TickCrossLib.Enums;

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
        //private TickCrossLib.Models.GameRequest _tempGameReq;

        public TickCrossLib.Models.GameRequest? _req;
        public void SetGameRequestTimer()
        {
            const int timeInterval = 1;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(timeInterval)
            };
            _timer.Tick += async (sender, e) =>
            {
                if (_loggedUser is null) return;
                bool isPlay = await SetReceiversGamePage();
                if (isPlay) return;

                if (!(_req is null) || _loggedUser is null) return;
                _req = await ApiService.GetAcceptedGameRequest(_loggedUser.Login);
                //_tempGameReq = _req;

                if (_req is null) return;

                //if (_req.Sender.Id != _loggedUser.Id) SetPageForSecondFrame(new GotGameRequest(MainFrame, _req, _loggedUser));

                //If game accepted + two players are logged in - start game
                //if (_tempGameReq is null) return;
                //if (!SetGameStart().Result) return;
                //await SetGamePage();

            };
            _timer.Start();
        }

        public async Task<bool> SetGameStart()
        {
            if (_req is null) return false;

            bool isReceiverLogged = await ApiService.IsUserIsLoggedById((int)_req.Receiver.Id); //temp user
            bool isSenderLogged = await ApiService.IsUserIsLoggedById((int)_req.Sender.Id); //future enemy

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

                page.SetMoveTimer();
                return true;
            }
            return false;
        }

        public RequestStatus GetRequestStatByString(string request)
        {
            for (int i = 0; i <= (int)RequestStatus.InProgress; i++)
            {
                if (request == ((RequestStatus)i).ToString())
                {
                    return (RequestStatus)i;
                }
            }
            return RequestStatus.InProgress;
        }

        public async void SetLoggedUser(TickCrossLib.Models.User user)
        {
            _loggedUser = user;
            await ApiService.RemoveClosedGames(user.Id);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!MainFrame.IsEnabled) ClearSecondaryFrame();
            if (MainFrame.Content is Registration || MainFrame.Content is MainPage ||
                MainFrame.Content is UserOptions)
            {
                MainFrame.Content = new Login(MainFrame);

                if (_loggedUser is not null)
                {
                    ApiService.SetUserLoginStatus(_loggedUser.Id, UserStat.Offline);
                    //ApiService.RemoveUserRequests(_loggedUser.Id);
                    ApiService.RemoveTempGame(_loggedUser.Id);
                }

                e.Cancel = true;
                //if(!(_timer is null))  _timer.Stop();
                _loggedUser = null;
            }
            else if (MainFrame.Content is GamePage || MainFrame.Content is FriendsPage ||
                MainFrame.Content is FriendAcceptance || MainFrame.Content is GameRequests)

            {
                if (MainFrame.Content is GamePage gamePage)
                {
                    gamePage.SetGameEndStatForPlayers();

                    //Set end game for oponent!
                    SetGameClosed(gamePage);
                }

                MainFrame.Content = new MainPage(MainFrame, _loggedUser);
                e.Cancel = true;
            }

            SetWindowSize();
        }

        public async void SetGameClosed(GamePage page)
        {
            //Is game is Still exist
            bool isExist = await ApiService.IsTempGameIsExist(page._game.Id);

            //Set game Canceled status
            if (isExist)
            {
                await ApiService.SetGameCanceledStatus(page._game.Id);
            }

        }

        public void ClearSecondaryFrame()
        {
            const int maxZIndex = 100;

            SecondaryFrame.Content = null;
            Canvas.SetZIndex(SecondaryFrame, -1);
            Canvas.SetZIndex(MainFrame, maxZIndex);

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
            const int radius = 30;
            const int zSecond = 10;
            MainFrame.Effect = new BlurEffect()
            {
                Radius = radius
            };
            MainFrame.IsEnabled = false;

            Canvas.SetZIndex(SecondaryFrame, zSecond);
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

                //_req = null;
                _timer.Start();
            }

            if (_loggedUser is not null && page is Login)
            {
                ApiService.SetUserLoginStatus(_loggedUser.Id, UserStat.Offline);
            }

            SetWindowSize();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetWindowSizeParams();
        }

        public void Window_StateChanged(object sender, EventArgs e)
        {
            SetWindowSize();
        }

        public void SetWindowSize()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                SetWindowSizeParams();
            }), DispatcherPriority.Render);
        }

        public void SetWindowSizeParams()
        {
            Size windowSize = new Size(this.ActualWidth, this.ActualHeight);

            if (MainFrame.Content is Login login)
            {
                login.ChangeCardSize(windowSize);
            }
            else if (MainFrame.Content is Registration reg)
            {
                reg.ChangeCardSize(windowSize);
            }
            else if (MainFrame.Content is MainPage main)
            {
                main.ChangeCardSize(windowSize);
            }
            else if (MainFrame.Content is FriendsPage friends)
            {
                friends.OptionsParamsSize(windowSize);
            }
            else if (MainFrame.Content is FriendAcceptance friendAccept)
            {
                friendAccept.ChangeCardSize(windowSize);
            }
            else if (MainFrame.Content is GameRequests gameReqs)
            {
                gameReqs.ChangeCardSize(windowSize);
            }

            if (SecondaryFrame.Content is UserOptions option)
            {
                option.ChangeCardSize(windowSize);
            }

        }
    }
}