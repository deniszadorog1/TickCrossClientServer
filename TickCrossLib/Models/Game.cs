using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using TickCrossLib.Enums;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace TickCrossLib.Models
{
    public class Game
    {
        public int Id { get; set; }
        public User FirstPlayer { get; set; }
        public User SecondPlayer { get; set; }
        private Field GameField { get; set; }

        public int StepperIndex { get; set; }

        public char FirstPlayerSign { get; set; }
        public char SecondPlayerSign { get; set; }


        public Game()
        {
            GameField = new Field();
        }

        public Game(User first, User second, int stepperIndex, char firstSign, char secondSign)
        {
            FirstPlayer = first;
            SecondPlayer = second;

            StepperIndex = stepperIndex;

            FirstPlayerSign = firstSign;
            SecondPlayerSign = secondSign;

            GameField = new Field();

            SetPlayersInArray();
        }

        public Game(User first, User second, int stepperIndex, char firstSign, char secondSign, int id)
        {
            FirstPlayer = first;
            SecondPlayer = second;

            StepperIndex = stepperIndex;

            FirstPlayerSign = firstSign;
            SecondPlayerSign = secondSign;

            Id = id;

            GameField = new Field();

            SetPlayersInArray();
        }


        public Game(GameRequest req, List<char> signs)
        {
            FirstPlayer = req.Sender;
            SecondPlayer = req.Receiver;

            FirstPlayerSign = req.ReceiverSign;
            SecondPlayerSign = req.SenderSign;

            StepperIndex = signs.First() == FirstPlayerSign ? 0 : 1;

            GameField = new Field();
            SetPlayersInArray();
        }

        public void SetSign(int x, int y)
        {
            GameField.SetSignInCell(x, y, StepperIndex == 0 ? FirstPlayerSign : SecondPlayerSign);
        }

        public void SetEnemySign(int x, int y, User logged)
        {
            GameField.SetSignInCell(x, y, Players.First().Id == logged.Id ? SecondPlayerSign: FirstPlayerSign);
        }

        public GameEnded GeGameResult()
        {
            return GameField.GetGameResult();
        }

        public char GetSign()
        {
            return StepperIndex == 0 ? FirstPlayerSign : SecondPlayerSign;
        }

        public char GetEnemySign(User user)
        {
            return FirstPlayer.Id == user.Id ? SecondPlayerSign : FirstPlayerSign; 
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
            return Players[StepperIndex] == FirstPlayer ? FirstPlayerSign : SecondPlayerSign;
        }


        public void AddGameId(int? newId)
        {
            Id = newId is null ? -1 : (int)newId;
        }

        public bool IsUserIsStepper(User user)
        {
            return Players[StepperIndex].Id == user.Id;
        }

        public int GetStepperId()
        {
            return Players[StepperIndex].Id;
        }

        public void SetStepperId(int stepperId)
        {
            StepperIndex = Players.First().Id == stepperId ? 0 : 1;
        }
    }
}
