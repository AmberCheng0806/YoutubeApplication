using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Youtube.Converters
{
    internal class TimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (!DateTime.TryParse(value.ToString(), out DateTime publishedAt))
                return string.Empty;

            var now = DateTime.Now;
            var diff = now - publishedAt;

            if (diff.TotalMinutes < 1)
                return "剛剛";

            if (diff.TotalHours < 1)
                return $"{(int)diff.TotalMinutes} 分鐘前";

            if (diff.TotalDays < 1)
                return $"{(int)diff.TotalHours} 小時前";

            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays} 天前";

            if (diff.TotalDays < 30)
                return $"{(int)(diff.TotalDays / 7)} 週前";

            if (diff.TotalDays < 365)
                return $"{(int)(diff.TotalDays / 30)} 個月前";

            return $"{(int)(diff.TotalDays / 365)} 年前";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
