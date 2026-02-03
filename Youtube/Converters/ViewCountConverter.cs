using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Youtube.Converters
{
    internal class ViewCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (!long.TryParse(value.ToString(), out long count))
                return string.Empty;

            if (count < 10_000)
                return $"觀看次數：{count:N0}";

            if (count < 1_000_000)
                return $"觀看次數：{count / 10_000.0:F1} 萬";

            return $"觀看次數：{count / 1_000_000.0:F1} 百萬";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
