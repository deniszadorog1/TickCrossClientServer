using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace TickCrossLib.Models
{
    public class Field
    {
        public Cell[,] Cells { get; set; }

        public Field()
        {
            Cells = new Cell[3, 3];
        }

        public void SetSignInCell(int xCord, int yCord, char sign) 
        {
            if (Cells[xCord, yCord].Sign.ToString() == string.Empty) return;

            Cells[xCord, yCord].Sign = sign;
        } 

        public bool IsSomeOneOne()
        {
            return false;
        }

        private const int _firstCellIndex = 0;
        private const int _secondCellIndex = 1;
        private const int _thirdCellIndex = 2;

        public bool IsHorizontalWon()
        {
            return ((Cells[_firstCellIndex, _firstCellIndex].Sign == Cells[_secondCellIndex, _firstCellIndex].Sign &&
                Cells[_firstCellIndex, _firstCellIndex].Sign == Cells[_thirdCellIndex, _firstCellIndex].Sign &&
                Cells[_firstCellIndex, _firstCellIndex].Sign.ToString() != string.Empty) || 
                
                ());
        }

        public bool IsVerticalWon()
        {
            return false;
        }

        public bool IsCrossWon()
        { 
            return false;
        }

        public bool IsDraw()
        {
            return false;
        }
    }
}
