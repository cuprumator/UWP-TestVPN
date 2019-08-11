using System;
using System.Threading.Tasks;
using TestVPN.Configuration;
using TestVPN.RESTClient;
using System.Collections.ObjectModel;
using Windows.Networking.Vpn;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using TestVPN.Utils;

namespace TestVPN.ViewModels
{
    class MainPageViewModel : NotificationBase
    {
        public MainPageViewModel()
        {
            Client = new VPNClient();
            SelectedServerIndex = -1;
        }

        public async Task LoadConfiguration()
        {
            await ConfigurationManager.LoadAsync(new JWTRESTClient(Constants.secret), Constants.configurationHosts[0]);
            var servers = await ConfigurationManager.GetServers();
            var status = await Client.GetStatusAsync();

            for (int i = 0; i < servers.Count; i++)
            {
                var server = servers[i];
                var serv = new ServerViewModel(server);
                Servers.Add(serv);
                if (status == VpnManagementConnectionStatus.Connected && server.country == AppConfiguration.LoadLastConnectedServer())
                {
                    SelectedServerIndex = i;
                }
            }

            SetAppearence(status);
        }

        public async Task OnConnectcClicked()
        {
            try
            {
                var status = await Client.GetStatusAsync();

                if (status == VpnManagementConnectionStatus.Connected)
                {
                    await Client.Disconnect();
                    status = VpnManagementConnectionStatus.Disconnected;
                }
                else
                {
                    status = await Client.Connect(SelectedServer);

                    if (status == VpnManagementConnectionStatus.Connected)
                    {
                        AppConfiguration.SaveLastConnectedServer(SelectedServer.country);
                    }
                }

                SetAppearence(status);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Client.Disconnect();
            }
        }

        public int SelectedServerIndex
        {
            get { return ServerIndex; }
            set
            {
                if (SetProperty(ref ServerIndex, value))
                {
                    RaisePropertyChanged(nameof(SelectedServer));
                }
            }
        }

        public Server SelectedServer
        {
            get { return (SelectedServerIndex >= 0) ? servers[SelectedServerIndex] : null; }
        }

   
        private int ServerIndex;

        private ObservableCollection<ServerViewModel> servers = new ObservableCollection<ServerViewModel>();
        public ObservableCollection<ServerViewModel> Servers
        {
            get { return servers; }
            set { SetProperty(ref servers, value); }
        }

        private VPNClient Client;

        private struct Button
        {
            public string Text;
            public SolidColorBrush Color;
        }

        private Button button;
        public string ButtonText
        {
            get { return button.Text; }
            set
            {
                SetProperty(ref button.Text, value);
            }
        }

        public SolidColorBrush ButtonColor
        {
            get { return button.Color; }
            set
            {
                SetProperty(ref button.Color, value);
            }
        }

        private struct Status
        {
            public string Text;
            public SolidColorBrush Color;
        }

        private Status status;

        public string StatusText
        {
            get { return status.Text; }
            set
            {
                SetProperty(ref status.Text, value);
            }
        }

        public SolidColorBrush StatusColor
        {
            get { return status.Color; }
            set
            {
                SetProperty(ref status.Color, value);
            }
        }

        private string addressText;
        public string AddressText
        {
            get { return addressText; }
            set
            {
                SetProperty(ref addressText, value);
            }
        }

        private void SetAppearence(VpnManagementConnectionStatus status)
        {
            if (status == VpnManagementConnectionStatus.Connected)
            {
                ButtonText = "Disconnect";
                ButtonColor = new SolidColorBrush(Windows.UI.Colors.Red);
                StatusText = "Connected";
                StatusColor = new SolidColorBrush(Windows.UI.Colors.Green);

            }
            else
            {
                ButtonText = "Connnect";
                ButtonColor = new SolidColorBrush(Windows.UI.Colors.Green);
                StatusText = "Not Connected";
                StatusColor = new SolidColorBrush(Windows.UI.Colors.Red);
            }

            AddressText = NetworkingTool.GetLocalIp();
        }
    }
}
