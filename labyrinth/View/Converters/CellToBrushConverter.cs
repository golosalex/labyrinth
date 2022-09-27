using labyrinth.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace labyrinth.View.Converters
{
    internal class CellToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is StatusEnum status)
            {
                Brush result = Brushes.Crimson;
                switch (status)
                {
                    case StatusEnum.Isolated:
                        result= Brushes.Orange;
                        break;
                    case StatusEnum.InStack:
                        result = Brushes.Brown;
                        break;
                    case StatusEnum.InLabirinth:
                        result = Brushes.LightGray;
                    break;

                }
                return result;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
