using MaterialDesignThemes.Wpf.Converters;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TickCrossClient.Services;
using TickCrossLib.Enums;

namespace TickCrossClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {

        public TickCrossLib.Models.Game _game;
        Frame _frame;
        private TickCrossLib.Models.User _user;

        public GamePage(TickCrossLib.Models.Game game, Frame frame, 
            TickCrossLib.Models.User user)
        {
            _game = game;
            _frame = frame;
            _user = user;

            InitializeComponent();

            SetBasicThings();
        }

        public void SetBasicThings()
        {
            SetBasicParams();
            SetGameBlocksInList();
            ClearGameBlocks();

            SetGameBlocksEvent();
        }

        public void SetGameBlocksEvent()
        {
            for (int i = 0; i < _gameBlocks.GetLength(0); i++)
            {
                for (int j = 0; j < _gameBlocks.GetLength(1); j++)
                {
                    Point point = new Point(i, j);
                    _gameBlocks[i, j].PreviewMouseDown += async (sender, e) =>
                    {
                        if (!_game.IsUserIsStepper(_user)) return;
                        bool setMove = await IsMoveIsSet(point);

                        //Change in db
                        if (setMove) SetMoveInDB(point);
                    };
                }
            }
        }

        public async Task<bool> IsMoveIsSet(Point point)
        {
            if (_gameBlocks[(int)point.X, (int)point.Y].Text != string.Empty) return false;

            _gameBlocks[(int)point.X, (int)point.Y].Text =
            _game.GetSign().ToString();

            _game.SetSign((int)point.X, (int)point.Y);

/*            TickCrossLib.Enums.GameEnded res = _game.GeGameResult();
            if (res != TickCrossLib.Enums.GameEnded.InProgress)
            {
                await ApiService.SetStatusForTempGame(res, _game.Id);

                GameEnded(res);
                return true;
            }*/

            _game.ChangeStepper();
            SetTurnVisibility();
            return true;
        }

        public async void SetMoveInDB(Point point)
        {
            await ApiService.SetMovePointInTempGame(((int)point.X, (int)point.Y), _game.Id);
            await ApiService.SetStepperForTempGame(_game.Id, _game.GetStepperId());
        }

        private void GameEnded(TickCrossLib.Enums.GameEnded res)
        {
            if (res == TickCrossLib.Enums.GameEnded.Won) MessageBox.Show("Game ended! Stepper is won");
            else if (res == TickCrossLib.Enums.GameEnded.Draw) MessageBox.Show("Game ended! Its draw.");
        }

        public void ClearGameBlocks()
        {
            for (int i = 0; i < _gameBlocks.GetLength(0); i++)
            {
                for (int j = 0; j < _gameBlocks.GetLength(1); j++)
                {
                    _gameBlocks[i, j].Text = string.Empty;
                }
            }
        }

        private TextBlock[,] _gameBlocks;
        public void SetGameBlocksInList()
        {
            _gameBlocks = new TextBlock[,]
            {
                {ZeroZeroCell, ZeroOneCell, ZeroTwoCell },
                {OneZeroCell, OneOneCell, OneTwoCell },
                {TwoZeroCell, TwoOneCell, TwoTwoCell }
            };
        }

        public void SetBasicParams()
        {
            FirstLoginText.Text = _game.FirstPlayer.Login;
            SecondLoginText.Text = _game.SecondPlayer.Login;

            SetTurnVisibility();
        }

        public void SetTurnVisibility()
        {
            if (_game.StepperIndex == 0) SetFirstPlayerStepperVis();
            else SetSecondPlayerStepperVis();
        }

        public void SetFirstPlayerStepperVis()
        {
            FirstPlayerTurnText.Visibility = Visibility.Visible;
            SecondPlayerTurnText.Visibility = Visibility.Hidden;
        }

        public void SetSecondPlayerStepperVis()
        {
            FirstPlayerTurnText.Visibility = Visibility.Hidden;
            SecondPlayerTurnText.Visibility = Visibility.Visible;
        }

        private DispatcherTimer _moveTimer;
        private TickCrossLib.Models.GameRequest _tempGameReq;

        public void SetGameMoveTimer(TickCrossLib.Models.GameRequest req)
        {
            _tempGameReq = req;

            ApiService.AddTempGameInDB(_game.Id);

            _moveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _moveTimer.Tick += async (sender, e) =>
            {
                //
                SetStepperVis();
                
                /*if(!SetGameStart().Result)
                {
                    //One of the players logged out
                    //_moveTimer.Stop();
                }*/
                 SetEnemiesSign();
            };
            _moveTimer.Start();
        }

        public async void SetEnemiesSign()
        {
            (int?, int?) cord = await ApiService.GetMoveCord(_game.Id);
            if (cord == (null, null)) return;

            if (_gameBlocks[(int)cord.Item1, (int)cord.Item2].Text != string.Empty) return;

            _gameBlocks[(int)cord.Item1, (int)cord.Item2].Text =
            _game.GetEnemySign(_user).ToString();

            _game.SetSign((int)cord.Item1, (int)cord.Item2);

            /*            TickCrossLib.Enums.GameEnded res = _game.GeGameResult();
                        if (res != TickCrossLib.Enums.GameEnded.InProgress)
                        {
                            await ApiService.SetStatusForTempGame(res, _game.Id);

                            GameEnded(res);
                            return true;
                        }*/

        }

        public async void SetStepperVis()
        {
            int? newStepperId = await ApiService.GetTempGameStepperId(_game.Id);
            if (newStepperId is null) return;

            _game.SetStepperId((int)newStepperId);

            SetTurnVisibility();
        }

        public async Task<bool> SetGameStart()
        {
            if (_tempGameReq is null) return false;

            bool isReceiverLogged = await ApiService.IsUserIsLoggedById((int)_tempGameReq.Receiver.Id); //temp user
            bool isSenderLogged = await ApiService.IsUserIsLoggedById((int)_tempGameReq.Sender.Id); //future enemy

            return isReceiverLogged && isSenderLogged;
        }

        private async Task MakeMove(int gameId)
        {
            //Get move (from columns x, y in temp game table)
            (int?, int?) cord = await ApiService.GetMoveCord(gameId);
            if (cord == (null, null) || cord == (-1, -1)) return;

            //Set this move for each player
            SetMoveToPlayer(((int, int))cord);

            //Check game status(from db table column)
            //if game ended write it + set result for each player
            //show end game message
            //close game page

            return;
            TickCrossLib.Enums.GameEnded? status = await CheckGameStatus();
            if (status is null) return;

            ((MainWindow)Window.GetWindow(_frame)).SetContentToMainFrame(new MainPage(_frame, _user));
            //_moveTimer.Stop();
        }

        public void SetGameResultForPlayerStatistic(GameEnded status)
        {
            switch (status)
            {
                case TickCrossLib.Enums.GameEnded.Won:
                    {
                        break;
                    }
                case TickCrossLib.Enums.GameEnded.Draw:
                    {
                        break;
                    }
                case TickCrossLib.Enums.GameEnded.InProgress:
                    {
                        break;
                    }
            };
        }


        public async Task<TickCrossLib.Enums.GameEnded?> CheckGameStatus()
        {
            string status = await ApiService.GetTempGameStatus(_game.Id);
            if (status is null) return null;

            TickCrossLib.Enums.GameEnded? gameStat = GetStatusByString(status);
            return gameStat;
        }


        public async void SetMoveToPlayer((int, int) moveCord)
        {
            await IsMoveIsSet(new Point(moveCord.Item1, moveCord.Item2));

            //Got move from db is enemies move !strict!

            //Set move in view + in game class

        }

        public GameEnded? GetStatusByString(string status)
        {
            for (int i = (int)TickCrossLib.Enums.GameEnded.Won;
                i <= (int)TickCrossLib.Enums.GameEnded.InProgress; i++)
            {
                if (((TickCrossLib.Enums.GameEnded)i).ToString() == status)
                {
                    return (TickCrossLib.Enums.GameEnded)i;
                }
            }
            return null;
        }
    }
}
