using ATFaceME.Xamarin.Core.Views.Common;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Views
{
    public partial class LoadingPage : BasePage
    {
        private bool disposed;

        public LoadingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        ~LoadingPage()
        {
            Dispose(false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // In the future, you'll have to dispose the attributes
            }

            disposed = true;
            base.Dispose(disposing);
        }
    }
}