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
using TickCrossLib.Enums;

namespace TickCrossLib.Models
{
    public class Field
    {
        public Cell[,] Cells { get; set; }


        private const int _boardSize = 3;
        public Field()
        {
            Cells = new Cell[_boardSize, _boardSize];
        }


        public void SetSignInCell(int xCord, int yCord, char sign)
        {
            if (Cells[xCord, yCord].Sign.ToString() == string.Empty) return;

            Cells[xCord, yCord].Sign = sign;
        }

        private bool IsGameEnded()
        {
            return IsSomeOneOne() || IsDraw();
        }

        public bool IsSomeOneOne()
        {
            return IsHorizontalWon() || IsVerticalWon() || IsCrossWon();
        }

        private const int _firstCellIndex = 0;
        private const int _secondCellIndex = 1;
        private const int _thirdCellIndex = 2;

        public bool IsHorizontalWon()
        {
            return ((Cells[_firstCellIndex, _firstCellIndex].Sign == Cells[_secondCellIndex, _firstCellIndex].Sign &&
                Cells[_firstCellIndex, _firstCellIndex].Sign == Cells[_thirdCellIndex, _firstCellIndex].Sign &&
                !IsCellSignIsEmpty(Cells[_firstCellIndex, _firstCellIndex])) ||

                (Cells[_firstCellIndex, _secondCellIndex].Sign == Cells[_secondCellIndex, _secondCellIndex].Sign &&
                Cells[_firstCellIndex, _secondCellIndex].Sign == Cells[_thirdCellIndex, _secondCellIndex].Sign &&
                !IsCellSignIsEmpty(Cells[_firstCellIndex, _secondCellIndex])) ||

                (Cells[_firstCellIndex, _thirdCellIndex].Sign == Cells[_secondCellIndex, _thirdCellIndex].Sign &&
                Cells[_firstCellIndex, _thirdCellIndex].Sign == Cells[_thirdCellIndex, _thirdCellIndex].Sign &&
                !IsCellSignIsEmpty(Cells[_firstCellIndex, _thirdCellIndex])));
        }


        private char _clearedValue = ' ';
        public bool IsVerticalWon()
        {
            char sign = _clearedValue;
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                sign = _clearedValue;
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (i == 0) sign = Cells[j, i].Sign;
                    else if (sign == _clearedValue || !IsCellIsEqualsSign(Cells[j, i], sign)) break;
                    if (j == Cells.GetLength(1) - 1) return true;
                }
            }
            return true;
        }

        public bool IsCellIsEqualsSign(Cell cell, char sign)
        {
            return cell.Sign.Equals(sign);
        }

        public bool IsCrossWon()
        {
            int size = Cells.GetLength(0);

            char sign = _clearedValue;
            for(int i = 0; i < size; i++)
            {
                if (i == 0) sign = Cells[i, i].Sign;
                if (Cells[i, i].Sign == _clearedValue || !IsCellIsEqualsSign(Cells[i, i], sign)) break;


                if (i == size - 1) return true;

            }
            sign = _clearedValue;
            for(int i = size - 1; i >= 0; i--)
            {
                if (i == 0) sign = Cells[i, i].Sign;
                if (Cells[i, i].Sign == _clearedValue || !IsCellIsEqualsSign(Cells[i, i], sign)) break;

                if (i == size - 1) return true;
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
            return cell.Sign.ToString() == string.Empty;
        }
    }
}
