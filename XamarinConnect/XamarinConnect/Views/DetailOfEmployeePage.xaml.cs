using Apibackend.Trasversal.DTOs;
using ATFaceME.Xamarin.Core.Models;
using ATFaceME.Xamarin.Core.ViewModels;
using ATFaceME.Xamarin.Core.Views.Common;

namespace ATFaceME.Xamarin.Core.Views
{
    public partial class DetailOfEmployeePage : BasePage
    {
        private bool disposed;

        public DetailOfEmployeePage(UserDetailModel employee)
        {
            InitalizeViewModel(new DetailsOfEmployeeViewModel(employee));
            InitializeComponent();
        }

        ~DetailOfEmployeePage()
        {
            Dispose(false);
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            Dispose(true);
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

        private DetailsOfEmployeeViewModel GetViewModel()
        {
            return (DetailsOfEmployeeViewModel)ViewModel;
        }
    }
}