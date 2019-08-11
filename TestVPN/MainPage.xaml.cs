using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TestVPN.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestVPN
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ViewModel = new MainPageViewModel();

            ApplicationView.PreferredLaunchViewSize = new Size(480, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            ConnectionProgress.IsActive = true;
            await ViewModel.OnConnectcClicked();
            ConnectionProgress.IsActive = false;
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadConfiguration();
        }

        private MainPageViewModel ViewModel;
    }
}
