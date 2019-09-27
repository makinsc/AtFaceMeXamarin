using ATFaceME.Xamarin.Core.Models;
using ATFaceME.Xamarin.Core.Utils;
using ATFaceME.Xamarin.Core.ViewModels;
using ATFaceME.Xamarin.Core.Views.Common;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core.Views
{
    public partial class ListOfEmployeesPage : BasePage
    {
        #region Private properties

        private bool isInitialized;
        private bool disposed;
        private bool isFilterShowed;
        private IdentifyPage identifyPage;
        private AdminPage adminPage;
        private ToolbarItem filterPageToolbar;
        private ToolbarItem identifyPageToolbar;
        private ToolbarItem groupPersonPageToolbar;        

        #endregion

        #region Constructor & Destructor

        public ListOfEmployeesPage()
        {
            identifyPage = new IdentifyPage();
            adminPage = new AdminPage();

            InitalizeViewModel(new ListOfEmployeesViewModel());
            InitializeComponent();
            
            EmployeesList.ItemAppearing += (object sender, ItemVisibilityEventArgs scrollArgs) =>
            {
                if (scrollArgs.Item is UserDetailModel user)
                {
                    user.PhotoSource = user.hasPhoto ? PhotoSourceType.URL : PhotoSourceType.DUMMY_LIST;
                    if (GetViewModel().IsTheLastItem(scrollArgs.Item) && !String.IsNullOrEmpty(GetViewModel().LinkToNext))
                    {
                        LoadMore();
                    }
                }

            };

            EmployeesList.ItemDisappearing += (object sender, ItemVisibilityEventArgs scrollArgs) =>
            {
                if (scrollArgs.Item is UserDetailModel user)
                {
                    user.PhotoSource = PhotoSourceType.NO_PHOTO;
                }

            };

            isFilterShowed = false;
        }

        ~ListOfEmployeesPage()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods

        public async void LoadMore()
        {
            LoadingIndicator.IsVisible = true;
            
            await DoRequestWithResponse(() => (GetViewModel().LoadViewModel(false)));
           
            LoadingIndicator.IsVisible = false;
        }

        public ListOfEmployeesViewModel GetViewModel()
        {
            return (ListOfEmployeesViewModel)ViewModel;
        }

        #endregion

        #region Protected methods

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!isInitialized)
            {
                isInitialized = true;
                await DoRequest(() => (GetViewModel().LoadViewModel()));
                PrepareToolbar();
            }
            else
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    foreach (var employee in GetViewModel().ListOfEmployees)
                    {
                        employee.PhotoSource = employee.hasPhoto ? PhotoSourceType.URL : PhotoSourceType.DUMMY_LIST;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                identifyPage = null;
            }

            disposed = true;
            base.Dispose(disposing);
        }

        protected override bool OnBackButtonPressed()
        {
            if (isFilterShowed)
            {
                HideFilter();
                return true;
            }
            else
            {
                GetViewModel().Restore();
                return base.OnBackButtonPressed();
            }          
        }

        protected override void OnSizeChanged(object sender, EventArgs e)
        {
            if (isFilterShowed)
            {
                FilterLayout.TranslateTo(0, FilterLayout.Height);
            }            
        }

        protected async override Task ChangePageState(PageState state, string message = null)
        {
            if (state.Equals(PageState.LOADING))
            {
                ListLayout.IsVisible = false;
                LoadingIndicator.IsVisible = true;
            }
            else if (this.state.Equals(PageState.LOADING))
            {
                ListLayout.IsVisible = true;
                LoadingIndicator.IsVisible = false;
            }

            this.state = state;
        }

        #endregion

        #region Private methods

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && e.Item is UserDetailModel)
            {
                Navigation.PushAsync(new DetailOfEmployeePage(((UserDetailModel)e.Item)), false);
            }
        }

        private void PrepareToolbar()
        {
            filterPageToolbar = new ToolbarItem();
            filterPageToolbar.Text = "Filtro";
            filterPageToolbar.Icon = "filter.png";
            ToolbarItems.Add(filterPageToolbar);

            identifyPageToolbar = new ToolbarItem();
            identifyPageToolbar.Text = "Cámara";
            identifyPageToolbar.Icon = "photo.png";
            ToolbarItems.Add(identifyPageToolbar);

            filterPageToolbar.Clicked += ChangeFilterPosition;
            AssignToNavigationCommand(identifyPageToolbar,
                                      false,
                                      identifyPage);

            if (AuthenticationHelper.IsAdmin)
            {
                groupPersonPageToolbar = new ToolbarItem();
                groupPersonPageToolbar.Text = "Administración";
                groupPersonPageToolbar.Icon = "admin.png";
                ToolbarItems.Add(groupPersonPageToolbar);

                AssignToNavigationCommand(groupPersonPageToolbar,
                                          false,
                                          adminPage);
            }
        }

        private void ChangeFilterPosition(object sender, EventArgs e)
        {
            if (!isFilterShowed)
            {
                ShowFilter();
            }
            else
            {
                HideFilter();
            }
        }

        private void ShowFilter()
        {
            if (!isFilterShowed)
            {
                FilterLayout.TranslateTo(0, FilterLayout.Height);
                isFilterShowed = true;
            }
        }

        private void HideFilter()
        {
            if (isFilterShowed)
            {
                FilterLayout.TranslateTo(0, -FilterLayout.Height);
                isFilterShowed = false;
            }            
        }

        private void OnOfficeLocationSelected(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                GetViewModel().SelectedIndexOfficeLocation = selectedIndex;
            }
            
            OfficeLabel.Text = OfficePicker.Items[OfficePicker.SelectedIndex];
            OfficeLabel.TextColor = Color.FromHex(OfficeLocationUtils.GetRGBColorByOfficeLocation(OfficeLabel.Text));
        }

        private void OnOfficeLabelTapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => OfficePicker.Focus());
        }

        private async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            HideFilter();

            GetViewModel().CheckIfHasToResetEmployees();

            if (GetViewModel().HasToApplyFilter())
            {
                await DoRequest(() => (GetViewModel().LoadViewModel()));
            }
            else
            {
                await DoRequest(() => (GetViewModel().LoadViewModel(true)));
            }
            
            if (!GetViewModel().HasResult())
            {
                Alert("No se encontraron resultados");
            }
        }

        #endregion
    }
}