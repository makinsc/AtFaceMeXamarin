using YourNameSpace.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ATFaceME.Xamarin.Core.Controls;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]
namespace YourNameSpace.iOS
{
    public class HyperlinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            int number = 0;
            int.TryParse(e.NewElement.Text, out number);

            if(number != 0)
            {
                Control.UserInteractionEnabled = true;

                Control.TextColor = UIColor.Blue;

                var gesture = new UITapGestureRecognizer();

                gesture.AddTarget(() =>
                {
                    var url = new NSUrl("tel:" + Control.Text);

                    if (UIApplication.SharedApplication.CanOpenUrl(url))
                        UIApplication.SharedApplication.OpenUrl(url);
                });

                Control.AddGestureRecognizer(gesture);
            }
            else
            {
                Control.UserInteractionEnabled = false;

                Control.TextColor = UIColor.Black;
            }

        }
    }
}