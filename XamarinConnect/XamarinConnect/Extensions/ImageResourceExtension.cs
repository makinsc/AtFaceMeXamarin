using ATFaceME.Xamarin.Core.Utils;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ATFaceME.Xamarin.Core.Extensions
{
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageUtils.GetSourceImage(Source);

            return imageSource;
        }
    }
}
