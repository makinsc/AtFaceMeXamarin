using Android.App;
using Android.Content;
using ATFaceME.Xamarin.Core.Views;
using ATFaceME.Xamarin.Droid;
using Microsoft.Identity.Client;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace ATFaceME.Xamarin.Droid
{
    public class LoginPageRenderer : PageRenderer
    {
        public LoginPageRenderer(Context context) : base(context) { }
        private LoginPage _page;
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            _page = e.NewElement as LoginPage;
            var activity = this.Context as Activity;
            _page.PlatformParameters = new PlatformParameters(activity);
        }
    }
}