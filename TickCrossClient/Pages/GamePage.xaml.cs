using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

        private Brush _previewColor = Brushes.Gray;
        public void SetGameBlocksEvent()
        {
            for (int i = 0; i < _gameBlocks.GetLength(0); i++)
            {
                for (int j = 0; j < _gameBlocks.GetLength(1); j++)
                {
                    Point point = new Point(i, j);
                    _gameBlocks[i, j].PreviewMouseDown += async (sender, e) =>
                    {
                        int x = (int)point.X;
                        int y = (int)point.Y;
                        if (_gameBlocks[x, y].Foreground == _previewColor)
                        {
                            ClearCellFromPreview(x, y);
                        }

                        if (!_game.IsUserIsStepper(_user)) return;
                        bool setMove = await IsMoveIsSet(point);

                        //Change in db
                        if (setMove) SetMoveInDB(point);
                    };

                    _gameBlocks[i, j].MouseEnter += (sender, e) =>
                    {
                        if (!_game.IsUserIsStepper(_user)) return;

                        int x = (int)point.X;
                        int y = (int)point.Y;

                        if (_gameBlocks[x, y].Text != string.Empty) return;

                        _gameBlocks[x, y].Foreground = _previewColor;
                        _gameBlocks[x, y].Text = _game.GetSign().ToString();
                    };

                    _gameBlocks[i, j].MouseLeave += (sender, e) =>
                    {
                        int x = (int)point.X;
                        int y = (int)point.Y;

                        if (_gameBlocks[x, y].Foreground != _previewColor) return;

                        ClearCellFromPreview(x, y);
                    };
                }
            }
        }

        public void ClearCellFromPreview(int x, int y)
        {
            _gameBlocks[x, y].Text = string.Empty;
            _gameBlocks[x, y].Foreground = Brushes.Black;
        }

        public async Task<bool> IsMoveIsSet(Point point)
        {
            if (_gameBlocks[(int)point.X, (int)point.Y].Text != string.Empty) return false;

            _gameBlocks[(int)point.X, (int)point.Y].Text =
            _game.GetSign().ToString();

            _game.SetSign((int)point.X, (int)point.Y);

            GameEnded res = _game.GeGameResult();
            if (res != TickCrossLib.Enums.GameEnded.InProgress)
            {
                await ApiService.SetStatusForTempGame(res, _game.Id);

                //Set end game result in db

                int? winnerId = null;
                if (res is TickCrossLib.Enums.GameEnded.Won)
                {
                    winnerId = _user.Id;
                }
                SetGameResultForPlayerStatistic(res, winnerId);

                _moveTimer.Stop();
                GameEnded(res);

                return true;
            }

            _game.ChangeStepper();
            SetTurnVisibility();
            return true;
        }

        public async void SetMoveInDB(Point point)
        {
            await ApiService.SetMovePointInTempGame(((int)point.X, (int)point.Y), _game.Id);
            await ApiService.SetStepperForTempGame(_game.Id, _game.GetStepperId());
        }

        private async void GameEnded(TickCrossLib.Enums.GameEnded res)
        {
            if (res == TickCrossLib.Enums.GameEnded.Won) MessageBox.Show("Game ended! Stepper is won");
            else if (res == TickCrossLib.Enums.GameEnded.Draw) MessageBox.Show("Game ended! Its draw.");
            else if (res == TickCrossLib.Enums.GameEnded.Canceled) MessageBox.Show("Game is been canceled!"); ;

            _moveTimer.Stop();
            ((MainWindow)Window.GetWindow(_frame)).SetContentToMainFrame(new MainPage(_frame, _user));
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();

            //Remove requests for logged player
            await ApiService.RemoveUserRequests(_user.Id);
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
                //Is enemy is closed game
                if(await IsGameIsClosed())
                {
                    await ApiService.RemoveTempGame(_game.Id);
                    GameEnded(TickCrossLib.Enums.GameEnded.Canceled);
                    return;
                }


                SetStepperVis();
                SetEnemiesSign();
            };
            _moveTimer.Start();
        }

        public async Task<bool> IsGameIsClosed()
        {
            bool? isCanceled = await ApiService.IsGameBeenCanceled(_game.Id);
            if (isCanceled is null) return false;
            return (bool)isCanceled;
        }

        public async void CheckTempGameStatus()
        {
            string? status = await ApiService.GetTempGameStatus(_game.Id);
            if (status is null) return;

            GameEnded? stat = GetStatusByString(status);
            if (stat is null) return;
            if (stat != TickCrossLib.Enums.GameEnded.InProgress) _moveTimer.Stop();
        }

        public async void SetGameResultForPlayerStatistic(GameEnded status, int? winnerId)
        {
            switch (status)
            {
                case TickCrossLib.Enums.GameEnded.Won:
                    {
                        await ApiService.SetGameResult(_game.Id, winnerId, null);
                        break;
                    }
                case TickCrossLib.Enums.GameEnded.Draw:
                    {
                        await ApiService.SetGameResult(_game.Id, null, true);
                        break;
                    }
            };
        }

        public async void SetEnemiesSign()
        {
            (int?, int?) cord = await ApiService.GetMoveCord(_game.Id);
            if (cord == (null, null)) return;

            if (_gameBlocks[(int)cord.Item1, (int)cord.Item2].Text != string.Empty) return;

            _gameBlocks[(int)cord.Item1, (int)cord.Item2].Text =
            _game.GetEnemySign(_user).ToString();

            _game.SetEnemySign((int)cord.Item1, (int)cord.Item2, _user);

            GameEnded status = _game.GeGameResult();
            if (status != TickCrossLib.Enums.GameEnded.InProgress)
            {
                await ApiService.SetStatusForTempGame(status, _game.Id);

                GameEnded(status);

                //Remove temp games;
                await ApiService.RemoveTempGame(_game.Id);
            }
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
        }

        public async Task<GameEnded?> CheckGameStatus()
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
                if (((GameEnded)i).ToString() == status)
                {
                    return (GameEnded)i;
                }
            }
            return null;
        }
    }
}
