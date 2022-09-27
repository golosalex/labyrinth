using labyrinth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labyrinth.Model
{
    public class LabyrinthModel
    {
        //Поля
        private ReadOnly2DArray<Cell> _labyrinthData;
        private CellWatcher _cellWatcher;

        //Свойства
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
        public LabyrinthModel(int rows = 10, int colomns = 10)
        {
            
            Cell[,] cells = CreateArrayOfCell(rows, colomns);

            _labyrinthData = new ReadOnly2DArray<Cell>(cells);
            _cellWatcher = new CellWatcher(LabyrinthData);
            CellWatcher.SomeCellChanched += CellWatcher_SomeCellChanched;
            TimeSpan = 25;
        }
        public void TestMethodWithCell()
        {
            LabyrinthData[2, 2].LeftWall = false;
            LabyrinthData[2,1].RightWall= false;
            LabyrinthData[2, 2].Status = StatusEnum.InStack;
            LabyrinthData[2, 1].Status = StatusEnum.InLabirinth;
        }
        public void GenerateLabyrinth()
        {
            LabyrinthGenerator generator = new LabyrinthGenerator(this);
            Task.Run(() => generator.GenerateLabyrinth());
        }
        private void CellWatcher_SomeCellChanched(object? sender, Cell e)
        {
            SomeCellChenged?.Invoke(this, e);
        }

        //публичные методы

        public void ChangeSize(int newRows, int newColoms)
        {
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
