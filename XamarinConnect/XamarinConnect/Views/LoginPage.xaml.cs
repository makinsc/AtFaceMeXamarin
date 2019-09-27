using ATFaceME.Xamarin.Core.Views.Common;
using Microsoft.Identity.Client;
using System;

namespace ATFaceME.Xamarin.Core.Views
{
    public partial class LoginPage : BasePage
    {
        public IPlatformParameters PlatformParameters { get; set; }
        private bool disposed;
        private bool isLoging;
        private ListOfEmployeesPage listOfEmployeesPage;

        public LoginPage() : base()
        {
            InitializeComponent();
            isLoging = false;
        }

        ~LoginPage()
        {
            Dispose(false);
        }

        protected async override void OnAppearing() 
        {
            base.OnAppearing();

            switch (state)
            {
                case PageState.INITIAL:
                    App.IdentityClientApp.PlatformParameters = PlatformParameters;
                    if (!AuthenticationHelper.IsAuthenticated())
                    {
                        TryLogin(LoginButton, new EventArgs());
                    }
                    break;

                case PageState.FINAL:
                    if (!isLoging)
                    {
                        bool doLogout = await DisplayAlert("Aviso", "La aplicación va a desloguearse", "Aceptar", "Cancelar");
                        if (doLogout)
                        {
                            AuthenticationHelper.SignOut();
                            Alert("Se ha deslogueado de la aplicación");
                        }
                        else
                        {
                            await Navigation.PushAsync(listOfEmployeesPage);
                        }
                    }                    
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                PlatformParameters = null;
            }

            disposed = true;
            base.Dispose(disposing);
        }

        private async void TryLogin(object sender, EventArgs e)
        {
            isLoging = true;
            await DoRequest(() => AuthenticationHelper.Authenticate(), "Autenticación realizada");

            if (AuthenticationHelper.IsAuthenticated())
            {
                listOfEmployeesPage = new ListOfEmployeesPage();
                await Navigation.PushAsync(listOfEmployeesPage);
            }

            isLoging = false;
        }
    }
}
