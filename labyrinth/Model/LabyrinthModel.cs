using labyrinth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace labyrinth.Model
{
    public class LabyrinthModel
    {
        //Поля
        private ReadOnly2DArray<Cell> _labyrinthData;
        private CellWatcher _cellWatcher;
      

        private CancellationTokenSource cancellationTokenSource;
        
        //Свойства
        public bool IsGeneratigActive { get; set; }
        private CellWatcher CellWatcher  
        {
            get => _cellWatcher;
            set
            {
                _cellWatcher = value;
            }
        }
        public int TimeSpan { get; set; }
        public ReadOnly2DArray<Cell> LabyrinthData
        {
            get => _labyrinthData;
            private set
            {
                _labyrinthData = value;
            }
        }
        public int Rows { get => LabyrinthData.Rows; }
        public int Colomns { get => LabyrinthData.Colomns; }
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="colomns"></param>
        public LabyrinthModel(int rows = 30, int colomns = 30)
        {
            
            Cell[,] cells = CreateArrayOfCell(rows, colomns);

            _labyrinthData = new ReadOnly2DArray<Cell>(cells);
            _cellWatcher = new CellWatcher(LabyrinthData);
            CellWatcher.SomeCellChanched += CellWatcher_SomeCellChanched;
            TimeSpan = 25;
            cancellationTokenSource = new CancellationTokenSource();
            IsGeneratigActive = false;
            
        }
        public void TestMethodWithCell()
        {
            LabyrinthData[2, 2].LeftWall = false;
            LabyrinthData[2,1].RightWall= false;
            LabyrinthData[2, 2].Status = StatusEnum.InStack;
            LabyrinthData[2, 1].Status = StatusEnum.InLabirinth;
        }
        public void GenerateLabyrinth(AlgorytmEnum algorytm)
        {
            var token = cancellationTokenSource.Token;
            if (token.CanBeCanceled) cancellationTokenSource.Cancel();
            
            cancellationTokenSource=new CancellationTokenSource();
            token = cancellationTokenSource.Token;
            LabyrinthGenerator generator = new LabyrinthGenerator(this);
            Task.Run(() => generator.GenerateLabyrinth(token, algorytm),token);
        }
        private void CellWatcher_SomeCellChanched(object? sender, Cell e)
        {
            SomeCellChenged?.Invoke(this, e);
        }

        //публичные методы

        public void ChangeSize(int newRows, int newColoms)
        {

            var token = cancellationTokenSource.Token;
            if (token.CanBeCanceled) cancellationTokenSource.Cancel();
            
            Cell[,] cells = CreateArrayOfCell(newRows, newColoms);

            LabyrinthData = new ReadOnly2DArray<Cell>(cells);
            CellsChanged?.Invoke(this, LabyrinthData);
            CellWatcher.Cells = LabyrinthData;

        }


        //приватные методы
        private static Cell[,] CreateArrayOfCell(int rows, int coloms)
        {
            Cell[,] cells = new Cell[rows, coloms];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < coloms; j++)
                {
                    cells[i, j] = new Cell(i, j);
                }
            }

            return cells;
        }

        //events
        public event EventHandler<Cell>? SomeCellChenged;
        public event EventHandler<ReadOnly2DArray<Cell>>? CellsChanged;

    }
}
