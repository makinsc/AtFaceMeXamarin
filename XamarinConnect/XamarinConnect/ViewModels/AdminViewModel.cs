using ATFaceME.Xamarin.Core.ViewModels.Common;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace ATFaceME.Xamarin.Core.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        #region Properties

        public ICommand LaunchTrain { get; private set; }
        private bool disposed;

        #endregion

        #region Constructor & Destructor

        public AdminViewModel() : base()
        {
            LaunchTrain = new Command(async () => await SendToken(), () => CanSend());
        }

        ~AdminViewModel()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        private bool CanSend()
        {
            return (AuthenticationHelper.IsAuthenticated() && AuthenticationHelper.IsAdmin);
        }

        private Task SendToken()
        {
            return Task.Run(()=> sendTokenForTrain());
        }

        private async void sendTokenForTrain()
        {
            //Implementamos el envio del mensaje a la cola.

            var result = await ServiceBus.ServiceBusManager.PostTokenToQueue(AuthenticationHelper.TokenForUser, "AtFaceMeBus", "colatokens");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                LaunchTrain = null;
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}
