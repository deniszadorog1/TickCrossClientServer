using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using TickCrossLib.Enums;

namespace TickCrossLib.Models
{
    public class Game
    {
        public User FirstPlayer { get; set; }
        public User SecondPlayer { get; set; }
        private Field GameField { get; set; }

        public int StepperIndex { get; set; }

        private char _firstPlayerSign = 'x';
        private char _secondPlayerSign = 'o';

        private List<(int, int)> _movesHistory = new List<(int, int)>();

        public Game(User first, User second, int stepperIndex, char firstSign, char secondSign)
        {
            FirstPlayer = first;
            SecondPlayer = second;

            StepperIndex = stepperIndex;

            _firstPlayerSign = firstSign;
            _secondPlayerSign = secondSign;

            GameField = new Field();

            SetPlayersInArray();
        }


        public void SetSign(int x, int y)
        {
            GameField.SetSignInCell(x, y, StepperIndex == 0 ? _firstPlayerSign : _secondPlayerSign);
        }

        public GameEnded GeGameResult()
        {
            return GameField.GetGameResult();
        }

        public char GetSign()
        {
            char sign = StepperIndex == 0 ? _firstPlayerSign : _secondPlayerSign;
            return sign;
        }

        public void ChangeStepper()
        {
            StepperIndex = StepperIndex == 0 ? 1 : 0;
        }

        private const int amountOfPlayers = 2;
        private User[] Players = new User[amountOfPlayers];

        public void SetPlayersInArray()
        {
            Players = new User[amountOfPlayers]
            {
                FirstPlayer,
                SecondPlayer
            };
        }

        private const int _firstPlayerIndex = 0;
        private const int _secondPlayerIndex = 1;
        public void ChangeStepperIndex()
        {
            StepperIndex = StepperIndex == _firstPlayerIndex ? _secondPlayerIndex : _firstPlayerIndex;
        }

        public void SetSignInField(int xCord, int yCord)
        {
            GameField.SetSignInCell(xCord, yCord, GetStepperSign());
        }

        public char GetStepperSign()
        {
            return Players[StepperIndex] == FirstPlayer ? _firstPlayerSign : _secondPlayerSign;
        }

        public void AddMoveInHistory(int x, int y)
        {
            _movesHistory.Add((x, y));
        }


    }
}
