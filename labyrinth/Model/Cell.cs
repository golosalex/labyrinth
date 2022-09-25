using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace labyrinth.Model
{
    public class Cell
    {
        private bool leftWall;
        private bool topWall;
        private bool rightWall;
        private bool bottomWall;

        public bool LeftWall
        {
            get => leftWall;
            set
            {
                leftWall = value;
                CellChanged?.Invoke(this, this);
            }
        }
        public bool TopWall
        {
            get => topWall;
            set
            {
                topWall = value;
                CellChanged?.Invoke(this, this);
            }
        }
        public bool RightWall
        {
            get => rightWall; set
            {
                rightWall = value;
                CellChanged?.Invoke(this, this);
            }
        }
        public bool BottomWall
        {
            get => bottomWall; set
            {
                bottomWall = value;
                CellChanged?.Invoke(this, this);
            }
        }

        public int Row { get; }
        public int Colomn { get; }

        public Cell(int row, int col)
        {
            Row = row;

            Colomn = col;
            LeftWall = true;
            topWall = true;
            rightWall = true;
            bottomWall = true;

        }
        public event EventHandler<Cell>? CellChanged;
    }
}
