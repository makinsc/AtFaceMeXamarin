using ATFaceME.Xamarin.Core.Utils;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Converters
{
    public class OfficeLocationToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return OfficeLocationUtils.GetRGBColorByOfficeLocation(value.ToString());
            }
            return OfficeLocationUtils.DEFAULT_RGB_COLOR;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
