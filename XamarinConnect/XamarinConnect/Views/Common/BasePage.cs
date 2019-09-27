using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ATFaceME.Xamarin.Core.Utils;
using ATFaceME.Xamarin.Core.ViewModels.Common;
using Plugin.Connectivity;

namespace ATFaceME.Xamarin.Core.Views.Common
{
    public abstract class BasePage : ContentPage, IDisposable
    {
        #region Private properties

        private bool disposed;

        #endregion

        #region Protected properties

        protected PageState state;
        protected BaseViewModel ViewModel { get; set; }

        protected enum PageState
        {
            INITIAL, PROCESSING, LOADING, FINAL
        }

        #endregion

        #region Constructor

        protected BasePage()
        {
            state = PageState.INITIAL;
            PrepareStyle();

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Methods

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel != null)
                ViewModel.IsNavigation = false;
        }

        protected void InitalizeViewModel(BaseViewModel _viewModel)
        {
            BindingContext = ViewModel = _viewModel;
            ViewModel.Navigation = Navigation;
        }

        protected void PrepareStyle()
        {
            BackgroundColor = Color.White;
        }

        protected void AssignToNavigationCommand(Button control, bool isModal, Page page = null)
        {
            control.Command = ViewModel.NavigationCommand;
            control.CommandParameter = new CommandNavigationParameter
            {
                IsModal = isModal,
                Page = page
            };
        }

        protected void AssignToNavigationCommand(ToolbarItem control, bool isModal, Page page = null)
        {
            control.Command = ViewModel.NavigationCommand;
            control.CommandParameter = new CommandNavigationParameter
            {
                IsModal = isModal,
                Page = page
            };
        }

        protected virtual void OnSizeChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Make a request checking if the connectivity is ok and indicating the result in an alert.
        /// Change the state of the page to LOADING during the request.
        /// </summary>
        /// <param name="function">The function which corresponds to the request</param>
        /// <param name="successfulMessage">The message to alert if the result is OK</param>
        protected async Task DoRequest(Func<Task<string>> function, string successfulMessage = null)
        {
            await ChangePageState(PageState.LOADING);

            var message = "No hay conexión";
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    message = await function();
                }
                catch (Exception e)
                {
                    message = "Se ha producido un error en la petición";
                }

                if (message.Equals("OK"))
                {
                    message = successfulMessage;
                }
            }

            if (message != null)
            {
                Alert(message);
            }

            await ChangePageState(PageState.FINAL);
        }

        /// <summary>
        /// Make a request checking if the connectivity is ok and returning the result
        /// </summary>
        /// <param name="function">The function which corresponds to the request</param>
        protected async Task<string> DoRequestWithResponse(Func<Task<string>> function)
        {
            var result = "No hay conexión";
            if (CrossConnectivity.Current.IsConnected)
            {
                result = await function();
            }

            return result;
        }

        protected async Task<T> DoRequestWithResponse<T>(Func<Task<T>> function)
        {
            T result = default(T);
            if (CrossConnectivity.Current.IsConnected)
            {
                result = await function();
            }

            return result;
        }

        /// <summary>
        /// Change the state of the page, and control the page if it has to load.
        /// This method has to be override if the page need to change to any state.
        /// </summary>
        /// <param name="state">The new state of the page</param>
        /// <param name="message">The message to alert if it has content</param>
        protected virtual async Task ChangePageState(PageState state, string message = null)
        {
            if (state.Equals(PageState.LOADING))
            {
                await Navigation.PushAsync(new LoadingPage(), true);
            }
            else if (this.state.Equals(PageState.LOADING))
            {
                await Navigation.PopAsync(true);
            }

            this.state = state;

            if (message != null)
            {
                Alert(message);
            }
        }

        /// <summary>
        /// Default alert for the pages.
        /// </summary>
        protected async void Alert(string message)
        {
            await DisplayAlert("Aviso", message, "Aceptar");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                ViewModel = null;
            }

            disposed = true;
        }

        #endregion
    }
}
