using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TestVPN.Configuration;
using TestVPN.Utils;
using TestVPN.RESTClient;
using Windows.Networking.Vpn;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestVPN
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            vpn = new VPNClient();

            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(480, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var status = await vpn.GetStatusAsync();

                if (status == VpnManagementConnectionStatus.Connected)
                {
                    await vpn.Disconnect();
                    SetAppearenceDiconnected();
                }
                else
                {
                    ConnectionProgress.IsActive = true;
                    status = await vpn.Connect((await ConfigurationManager.GetServers())[ServerList.SelectedIndex]);

                    if (status == VpnManagementConnectionStatus.Connected)
                    {
                        ConnectionProgress.IsActive = false;
                        SetAppearenceConnected();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await vpn.Disconnect();
            }
        }

        private void SetAppearenceConnected()
        {
            ConnectButton.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            ConnectButton.Content = "Disconnect";
            Status.Text = "Connected";
            Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            AddressBlock.Text = NetworkingTool.GetLocalIp();
            ServerList.SelectedValue = vpn.ActiveProfile;
        }

        private void SetAppearenceDiconnected()
        {
            ConnectButton.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            ConnectButton.Content = "Connect";
            Status.Text = "Not Connected";
            Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
            AddressBlock.Text = NetworkingTool.GetLocalIp();
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var servers = await ConfigurationManager.GetServers();

                foreach (var server in servers)
                {
                    ServerList.Items.Add(server.country);
                }

                var status = await vpn.GetStatusAsync();

                if (status == VpnManagementConnectionStatus.Connected)
                {
                    SetAppearenceConnected();
                }
                else
                {
                    SetAppearenceDiconnected();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await vpn.Disconnect();
            }
        }

        private VPNClient vpn;
    }
}
