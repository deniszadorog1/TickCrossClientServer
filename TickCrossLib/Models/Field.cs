using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TickCrossLib.Enums;

namespace TickCrossLib.Models
{
    public class Field
    {
        public Cell[,] Cells { get; set; }

        private const int _boardSize = 3;
        public Field()
        {
            SetField();
        }

        public void SetField()
        {
            Cells = new Cell[_boardSize, _boardSize];

            for(int i = 0; i < _boardSize; i++)
            {
                for(int j = 0; j < _boardSize; j++)
                {
                    Cells[i, j] = new Cell();
                }
            }

        }

        public void SetSignInCell(int xCord, int yCord, char sign)
        {
            if (!(Cells[xCord, yCord].Sign is null)) return;
            Cells[xCord, yCord].Sign = sign;
        }

        public GameEnded GetGameResult()
        {
            return IsDraw() ? GameEnded.Draw :
                IsSomeoneWon() ? GameEnded.Won :
                GameEnded.InProgress;
        }

        private bool IsGameEnded()
        {
            return IsSomeoneWon() || IsDraw();
        }

        public bool IsSomeoneWon()
        {
            return IsHorizontalWon() || IsVerticalWon() || IsCrossWon();
        }

        private const int _firstCellIndex = 0;
        private const int _secondCellIndex = 1;
        private const int _thirdCellIndex = 2;

        public bool IsHorizontalWon()
        {
            return ((Cells[_firstCellIndex, _firstCellIndex].Sign == Cells[_firstCellIndex, _secondCellIndex].Sign &&
                Cells[_firstCellIndex, _firstCellIndex].Sign == Cells[_firstCellIndex, _thirdCellIndex].Sign &&
                !IsCellSignIsEmpty(Cells[_firstCellIndex, _firstCellIndex])) ||

                (Cells[_secondCellIndex, _firstCellIndex].Sign == Cells[_secondCellIndex, _secondCellIndex].Sign &&
                Cells[_secondCellIndex, _firstCellIndex].Sign == Cells[_secondCellIndex, _thirdCellIndex].Sign &&
                !IsCellSignIsEmpty(Cells[_secondCellIndex, _firstCellIndex])) ||

                (Cells[_thirdCellIndex, _firstCellIndex].Sign == Cells[_thirdCellIndex, _secondCellIndex].Sign &&
                Cells[_thirdCellIndex, _firstCellIndex].Sign == Cells[_thirdCellIndex, _thirdCellIndex].Sign &&
                !IsCellSignIsEmpty(Cells[_thirdCellIndex, _firstCellIndex])));
        }


        private char _clearedValue = ' ';
        public bool IsVerticalWon()
        {
            char? sign = _clearedValue;
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                sign = _clearedValue;
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (j == 0) sign = Cells[j, i].Sign;
                    else if (sign == _clearedValue || sign is null || !IsCellIsEqualsSign(Cells[j, i], sign)) break;
                    if (j == Cells.GetLength(1) - 1) return true;
                }
            }
            return false;
        }

        public bool IsCellIsEqualsSign(Cell cell, char? sign)
        {
            return cell.Sign.Equals(sign);
        }

        public bool IsCrossWon()
        {
            int size = Cells.GetLength(0);

            char? sign = _clearedValue;
            for(int i = 0; i < size; i++)
            {
                if (i == 0) sign = Cells[i, i].Sign;
                if (sign is null || 
                    Cells[i, i].Sign == _clearedValue || !IsCellIsEqualsSign(Cells[i, i], sign)) break;


                if (i == size - 1) return true;


            }
            sign = _clearedValue;
            int count = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                if (i == size - 1) sign = Cells[count, i].Sign;
                if (sign is null || Cells[count, i].Sign == _clearedValue || !IsCellIsEqualsSign(Cells[count, i], sign)) break;

                if (i == 0) return true;
                count++;
            }
            return false;
        }

        public bool IsDraw()
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (IsCellSignIsEmpty(Cells[i, j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsCellSignIsEmpty(Cell cell)
        {
            if (cell.Sign is null) return true;
            return cell.Sign.ToString() == string.Empty;
        }
    }
}
