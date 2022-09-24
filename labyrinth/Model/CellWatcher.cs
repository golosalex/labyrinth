using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labyrinth.Model
{
    public class CellWatcher
    {
        private IEnumerable<Cell> _cells;

        public IEnumerable<Cell> Cells
        {
            get => _cells; 
            set
            {
                MassUnsubscribe();
                _cells = value;
                MassSubscribe();
            }
        }
        public CellWatcher(IEnumerable<Cell> cells)
                {
                    _cells = cells;
                    MassSubscribe();
                }
        private void MassUnsubscribe()
        {
            foreach (var cell in Cells)
            {
                cell.CellChanged -= Cell_CellChanged;
            }
        }

        

        private void MassSubscribe()
        {
            foreach (var cell in Cells)
            {
                cell.CellChanged += Cell_CellChanged;
            }
        }


        private void Cell_CellChanged(object? sender, Cell e)
        {
            SomeCellChanched?.Invoke(this, e);
        }

        public event EventHandler<Cell>? SomeCellChanched;
    }
}
