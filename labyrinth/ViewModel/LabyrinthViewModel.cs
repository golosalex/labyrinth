using labyrinth.Common;
using labyrinth.Model;
using MazeProj.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labyrinth.ViewModel
{
    public class LabyrinthViewModel : BasePropertyChanged
    {
        private int _rows;
        private int _coloms;
        private readonly LabyrinthModel _model;
        public ReadOnly2DArray<CellViewModel> Data { get; private set; }
        public int Rows { get => _rows; set => Set<int>(ref _rows, value); }
        public int Coloms { get => _coloms; set => Set<int>(ref _coloms,value); }

        public int ModelsRows { get => _model.Rows; }
        public int ModelsColoms { get=> _model.Colomns; }

        public LabyrinthViewModel(LabyrinthModel model)
        {
            _model = model;
            var newData = model.LabyrinthData;
            var array = new CellViewModel[_model.Rows, _model.Colomns];
            foreach (var cell in newData)
            {
                array[cell.Row, cell.Colomn] = new CellViewModel(cell);
            }
            Data = new ReadOnly2DArray<CellViewModel>(array);
            Rows = _model.Rows;
            Coloms = _model.Colomns;

            model.CellsChanged += Model_CellsChanged;
            model.SomeCellChenged += Model_SomeCellChenged;
        }

        private void Model_SomeCellChenged(object? sender, Cell e)
        {

            var dataCell = Data[e.Row, e.Colomn];
            dataCell.Update(e);
        }

        private void InitData(ReadOnly2DArray<Cell> newData)
        {
            var array = new CellViewModel[_model.Rows, _model.Colomns];
            foreach (var cell in newData)
            {
                array[cell.Row, cell.Colomn] = new CellViewModel(cell);
            }
            Data = new ReadOnly2DArray<CellViewModel>(array);
            Rows =  _model.Rows;
            Coloms =  _model.Colomns;
        }

        private void Model_CellsChanged(object? sender, ReadOnly2DArray<Cell> e)
        {
            InitData(e);
        }
    }
}
