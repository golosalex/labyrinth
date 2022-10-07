using labyrinth.Common;
using labyrinth.Model;
using MazeProj.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace labyrinth.ViewModel
{
    public class LabyrinthViewModel : BasePropertyChanged
    {


        private int _rows;
        private int _coloms;
        private int _modelRows;
        private int _modelColomns;
        private ReadOnly2DArray<CellViewModel> data;
        private int _timeSpanViewModel;
        private readonly LabyrinthModel _model;
        private readonly Launcher _launcher;

        public ICommand RemakeLabirinth { get; }
        public ICommand TestCommand { get; }
        public ICommand GenerateCommand { get; }

        public ReadOnly2DArray<CellViewModel> Data { get => data; private set => Set<ReadOnly2DArray<CellViewModel>>(ref data, value); }
        public int Rows { get => _rows; set => Set<int>(ref _rows, value); }
        public int Coloms { get => _coloms; set => Set<int>(ref _coloms, value); }
        public int TimeSpanViewModel
        {
            get => _timeSpanViewModel; 
            set
            {
                Set<int>(ref _timeSpanViewModel, value);
                _model.TimeSpan=value;
            }
        }
        public int ModelsRows { get => _model.Rows; set => Set<int>(ref _modelRows, value); }
        public int ModelsColomns { get => _model.Colomns; set => Set<int>(ref _modelColomns, value); }

        /// <summary>
        /// это для дизайнера студии, не юзать!!!
        /// </summary>
        public LabyrinthViewModel() : this(new LabyrinthModel(), new Launcher())
        {

        }


        public LabyrinthViewModel(LabyrinthModel model, Launcher launcher)
        {
            _model = model;
            _modelColomns = model.Colomns;
            _modelRows = model.Rows;
            TimeSpanViewModel=model.TimeSpan;
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
            _launcher = launcher;
            RemakeLabirinth = new RelayCommand(_ => Remake());
            TestCommand = new RelayCommand(_ => _model.TestMethodWithCell());
            GenerateCommand = new RelayCommand(_ => Generate());
        }
        private void Generate()
        {
            if (ModelsRows != Rows || ModelsColomns != Coloms) Remake();

            _model.GenerateLabyrinth();
        }

        private void Remake()
        {
            _model.ChangeSize(Rows, Coloms);

        }

        private void Model_SomeCellChenged(object? sender, Cell e)
        {

            var dataCell = Data[e.Row, e.Colomn];
            dataCell.Update(e);
        }

        private void InitData(ReadOnly2DArray<Cell> newData)
        {
            ModelsColomns = newData.Colomns;
            ModelsRows = newData.Rows;
            var array = new CellViewModel[_model.Rows, _model.Colomns];
            foreach (var cell in newData)
            {
                array[cell.Row, cell.Colomn] = new CellViewModel(cell);
            }
            Data = new ReadOnly2DArray<CellViewModel>(array);
            Rows = _model.Rows;
            Coloms = _model.Colomns;

        }

        private void Model_CellsChanged(object? sender, ReadOnly2DArray<Cell> e)
        {
            InitData(e);
        }
    }
}
