using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TickCrossClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        TickCrossLib.Models.Game _game;

        public GamePage(TickCrossLib.Models.Game game)
        {
            _game = game;

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
            for(int i = 0; i < _gameBlocks.GetLength(0); i++)
            {
                for(int j = 0; j < _gameBlocks.GetLength(1); j++)
                {
                    _gameBlocks[i, j].PreviewMouseDown += (sender, e) =>
                    {

                    };
                }
            }
        }

        public void ClearGameBlocks()
        {
            for(int i = 0; i < _gameBlocks.GetLength(0); i++)
            {
                for(int j = 0; j < _gameBlocks.GetLength(1); j++)
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

    }
}
