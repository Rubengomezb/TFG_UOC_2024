using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Converters
{
    public class BoolToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool isFavourite = (bool)value;
                if (isFavourite)
                    return "favorite_24dp.png";
                else
                    return "favorite_border_24dp.png";
        }
            return "favorite_border_24dp.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
