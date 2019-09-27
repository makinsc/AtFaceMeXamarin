using Apibackend.Trasversal.DTOs;
using ApiBackend.Transversal.DTOs.PLC;
using ApiBackend.Transversal.DTOs.PLC.RequestDTO;
using ApiBackend.Transversal.DTOs.PLC.ResultDTO;
using ATFaceME.Xamarin.Core.Models;
using ATFaceME.Xamarin.Core.ViewModels.Common;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.ViewModels
{
    public class IdentifyViewModel : BaseViewModel
    {
        #region Private properties

        private UserDetailModel user;
        private bool disposed;
        private string infoText;

        #endregion

        #region Public properties

        public UserDetailModel User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        public string InfoText
        {
            get { return infoText; }
            set
            {
                infoText = value;
                OnPropertyChanged("InfoText");
            }
        }

        public async Task<IdentifyResult> LoadViewModel(Stream photoStream)
        {
            var personData = await BackendClient.CallHttp<Stream, IdentifyResult>(Endpoints.Identify,
                                                                            HttpVerbs.POST,
                                                                            AuthenticationHelper.TokenForUser,
                                                                            photoStream,
                                                                            null);
            return personData;
        }

        #endregion

        #region Constructor & Destructor

        public IdentifyViewModel() : base()
        {
        }

        ~IdentifyViewModel()
        {
            Dispose(false);
        }

        #endregion        

        #region Protected & Private methods

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                user = null;
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}
