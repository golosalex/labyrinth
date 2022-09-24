using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labyrinth.Common
{
    public class ReadOnly2DArray<T> : IEnumerable<T>
    {
        private readonly T[,] array;
        public int Rows { get; }
        public int Columns { get; }

        public ReadOnly2DArray(T[,] array)
        {
            this.array = array;
            Rows = array.GetLength(0);
            Columns = array.GetLength(1);
        }

        public T this[int row, int column] => array[row, column];

        public IEnumerator<T> GetEnumerator()
        {
            int rows = Rows;
            int columns = Columns;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    yield return array[i, j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
