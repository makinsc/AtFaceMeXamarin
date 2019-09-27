namespace XamarinConnect.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new ATFaceME.Xamarin.Core.App());
        }
    }
}
