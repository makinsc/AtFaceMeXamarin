using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Converters
{
    public class ArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //byte[] bmp = value as byte[];
            //if (bmp == null)
            //    return null;

            //return ImageSource.FromStream(() => new MemoryStream(bmp));
            return ImageSource.FromUri(new Uri("https://atoffice365.blob.core.windows.net/profilephotoatsistemas/30edb800-073a-4ded-b2ea-7cea40b8e55d"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

    }
}
