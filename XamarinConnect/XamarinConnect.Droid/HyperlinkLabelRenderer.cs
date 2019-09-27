using YourNameSpace.Droid;
using Android.Text.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ATFaceME.Xamarin.Core.Controls;
using Android.Content;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]
namespace YourNameSpace.Droid
{
    public class HyperlinkLabelRenderer : LabelRenderer
    {
        public HyperlinkLabelRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Linkify.AddLinks(Control, MatchOptions.All);
        }
    }
}