﻿using labyrinth.Common;
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

        public ReadOnly2DArray<Cell> LabyrinthData
        {
            get => _labyrinthData;
            private set
            {
                _labyrinthData = value;
            }
        }
        public int Rows { get; private set; }
        public int Coloms { get; private set; }
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="coloms"></param>
        public LabyrinthModel(int rows = 10, int coloms = 10)
        {
            Rows = rows;
            Coloms = coloms;
            Cell[,] cells = CreateArrayOfCell(rows, coloms);

            _labyrinthData = new ReadOnly2DArray<Cell>(cells);
            _cellWatcher = new CellWatcher(LabyrinthData);
            CellWatcher.SomeCellChanched += CellWatcher_SomeCellChanched;
        }

        private void CellWatcher_SomeCellChanched(object? sender, Cell e)
        {
            SomeCellChenged?.Invoke(this, e);
        }

        //публичные методы

        public void ChangeSize(int newRows, int newColoms)
        {
            Rows = newRows;
            Coloms = newColoms;
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
