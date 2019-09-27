using ATFaceME.Xamarin.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace ATFaceME.Xamarin.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : Common.BasePage
    {
        public AdminPage()
        {
            InitalizeViewModel(new AdminViewModel());
            InitializeComponent();
        }
    }
}