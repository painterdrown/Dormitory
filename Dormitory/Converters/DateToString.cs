using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory.Converters
{
    class DateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime time = (DateTime)value;
            return time.ToString("yyyy/MM/dd");
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTime.ParseExact(value.ToString(), "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
