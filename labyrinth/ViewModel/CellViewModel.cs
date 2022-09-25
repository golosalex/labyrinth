using labyrinth.Model;
using MazeProj.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labyrinth.ViewModel
{
    public class CellViewModel:BasePropertyChanged
    {
        private bool _leftWall;
        private bool _topWall;
        private bool _rightWall;
        private bool _bottomWall;

        public bool LeftWall
        {
            get => _leftWall;
            set => Set<bool>(ref _leftWall, value);
            
        }
        public bool TopWall
        {
            get => _topWall;
            set => Set<bool>(ref _topWall, value);
        }
        public bool RightWall
        {
            get => _rightWall;
            set => Set<bool>(ref _rightWall, value);
        }
        public bool BottomWall
        {
            get => _bottomWall;
            set => Set<bool>(ref _bottomWall, value);
        }

        public int Row { get; }
        public int Colomn { get; }

        public CellViewModel(Cell cell)
        {
            Row = cell.Row;
            Colomn = cell.Colomn;

            LeftWall = cell.LeftWall;   
            RightWall= cell.RightWall;
            TopWall = cell.TopWall;
            BottomWall = cell.BottomWall;
        }

        public void Update (Cell cell)
        {
            LeftWall = cell.LeftWall;
            RightWall = cell.RightWall;
            TopWall = cell.TopWall;
            BottomWall = cell.BottomWall;
        }


    }
}
