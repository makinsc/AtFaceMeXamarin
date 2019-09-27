using Xamarin.Forms;
using ATFaceME.Xamarin.Core.Views;
using Xamarin.Forms.Platform.iOS;
using Microsoft.Identity.Client;
using ATFaceME.Xamarin.iOS;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace ATFaceME.Xamarin.iOS
{
    class LoginPageRenderer : PageRenderer
    {
        LoginPage _page;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            _page = e.NewElement as LoginPage;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _page.PlatformParameters = new PlatformParameters(this);
        }
    }
}