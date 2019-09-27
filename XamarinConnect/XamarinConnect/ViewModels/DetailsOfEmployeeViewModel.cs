using Apibackend.Trasversal.DTOs;
using ATFaceME.Xamarin.Core.Models;
using ATFaceME.Xamarin.Core.ViewModels.Common;

namespace ATFaceME.Xamarin.Core.ViewModels
{
    public class DetailsOfEmployeeViewModel : BaseViewModel
    {
        #region Private properties

        private UserDetailModel details;
        private bool disposed;

        #endregion

        #region Public properties

        public string Id
        {
            get { return details.id; }
        }

        public string FullName
        {
            get { return Check(details.displayName); }
        }

        public string JobTitle
        {
            get { return Check(details.jobTitle); }
        }

        public string OfficeLocation
        {
            get { return Check(details.officeLocation); }
        }

        public string Mail
        {
            get { return Check(details.mail); }
        }

        public string Phone
        {
            get { return (HasPhone) ? details.businessPhones[0] : "No especificado"; }
        }

        public bool HasPhone
        {
            get { return details.businessPhones.Count > 0; }
        }

        public string DefaultPhoto
        {
            get
            {
                return details.PhotoSource.ToString();
            }
        }

        #endregion

        #region Constructor & Destructor

        public DetailsOfEmployeeViewModel(UserDetailModel _details) : base()
        {
            details = _details;
            details.PhotoSource = details.hasPhoto ? PhotoSourceType.URL : PhotoSourceType.DUMMY_DETAILS;
        }

        ~DetailsOfEmployeeViewModel()
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
                details = null;
            }

            disposed = true;
            base.Dispose(disposing);
        }

        private string Check(string data)
        {
            return (!string.IsNullOrEmpty(data)) ? data : "No especificado";
        }

        #endregion
    }
}
