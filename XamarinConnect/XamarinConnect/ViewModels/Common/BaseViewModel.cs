using ATFaceME.Xamarin.Core.Agents;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.ViewModels.Common
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Private properties

        private bool isNavigation;
        private BackendClient backendClient;
        private bool disposed;

        #endregion

        #region Public properties

        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation;
        public Command NavigationCommand { get; private set; }

        public bool IsNavigation
        {
            get { return isNavigation; }
            set
            {
                isNavigation = value;
                OnPropertyChanged("IsNavigation");
                NavigationCommand.ChangeCanExecute();
            }
        }

        public BackendClient BackendClient
        {
            get
            {
                if (backendClient == null)
                {
                    backendClient = new BackendClient();
                }
                return backendClient;
            }
        }

        #endregion

        #region Constructor

        public BaseViewModel()
        {
            NavigationCommand = new Command(
                (parameter) =>
                {
                    IsNavigation = true;
                    NavigationToPage((CommandNavigationParameter)parameter);
                },
                (o) =>
                {
                    return !IsNavigation;
                });
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                backendClient = null;
                PropertyChanged = null;
            }

            disposed = true;
        }

        private void NavigationToPage(CommandNavigationParameter parameter)
        {
            if (parameter.IsModal)
            {
                if (parameter.Page != null)
                {
                    Navigation.PushModalAsync(parameter.Page);
                }
                else
                {
                    Navigation.PopModalAsync();
                }
            }
            else
            {
                if (parameter.Page != null)
                {
                    Navigation.PushAsync(parameter.Page);
                }
                else
                {
                    Navigation.PopAsync();
                }
            }
        }

        #endregion
    }

    public class CommandNavigationParameter
    {
        public bool IsModal;
        public Page Page;
    }
}
