
using ATFaceME.Xamarin.Core.Models;
using ATFaceME.Xamarin.Core.Utils;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Converters
{
    public class IdToSourceImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            var label = parameter as Label;

            if (string.IsNullOrEmpty(path) || label == null)
                return value;

            PhotoSourceType sourceType = (PhotoSourceType) Enum.Parse(typeof(PhotoSourceType),label.Text);

            switch (sourceType)
            {
                case PhotoSourceType.URL:
                    return "https://atoffice365.blob.core.windows.net/profilephotoatsistemas/" + path + ".jpg";
                case PhotoSourceType.DUMMY_LIST:
                    return ImageUtils.DummyUserImageList;
                case PhotoSourceType.DUMMY_DETAILS:
                    return ImageUtils.DummyUserImageProfile;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
