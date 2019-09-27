using ATFaceME.Xamarin.Core.ViewModels.Common;

namespace ATFaceME.Xamarin.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        #region Properties

        private bool disposed;

        public bool Visible
        {
            get
            {
                return AuthenticationHelper.IsAdmin;
            }
        }

        #endregion

        #region Constructor & Destructor

        public MenuViewModel() : base()
        {
        }

        ~MenuViewModel()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

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

        #endregion
    }
}
