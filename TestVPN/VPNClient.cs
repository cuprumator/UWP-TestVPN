using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Vpn;
using Windows.Security.Credentials;
using TestVPN.Configuration;

namespace TestVPN
{
    class VPNClient 
    {
        public VPNClient()
        {
            this.mgr = new VpnManagementAgent();
        }

        public async Task<VpnManagementConnectionStatus> Connect(Server server)
        {
            VpnNativeProfile profile = new VpnNativeProfile()
            {
                AlwaysOn = true,
                NativeProtocolType = VpnNativeProtocolType.IpsecIkev2,
                ProfileName = Constants.connectionProfileName,
                RememberCredentials = false,
                RequireVpnClientAppUI = false,
                RoutingPolicyType = VpnRoutingPolicyType.ForceAllTrafficOverVpn,
                TunnelAuthenticationMethod = VpnAuthenticationMethod.Eap,
                UserAuthenticationMethod = VpnAuthenticationMethod.Eap,
                //load eap from xml placed at assembly folder
                EapConfiguration = File.ReadAllText(Path.Combine(Windows.ApplicationModel.Package.Current.Installed­Location.Path, @"profile.xml"))
            };

            profile.Servers.Add(server.serverAddress);

            PasswordCredential credentials = new PasswordCredential
            {
                UserName = server.eap_name,
                Password = server.eap_secret
            };

            VpnManagementErrorStatus profileStatus = await mgr.AddProfileFromObjectAsync(profile);

            VpnManagementErrorStatus connectStatus = await mgr.ConnectProfileWithPasswordCredentialAsync(profile, credentials);
            activeProfile = profile;

            return profile.ConnectionStatus;
        }

        public async Task<VpnManagementConnectionStatus> GetStatusAsync()
        {
            var list = await mgr.GetProfilesAsync();
            foreach (VpnNativeProfile profile in list)
            {
                var servers = await ConfigurationManager.GetServers();
                foreach (var server in servers)
                {
                    if (profile.ProfileName == Constants.connectionProfileName)
                    {
                        var status = profile.ConnectionStatus;
                        if (status == VpnManagementConnectionStatus.Connected)
                        {
                            activeProfile = profile;
                            return status;
                        }
                        return status;
                    }
                }
               
            }
            return VpnManagementConnectionStatus.Disconnected;
        }

        public async Task Disconnect()
        {
            if (activeProfile != null)
            {
                await mgr.DisconnectProfileAsync(activeProfile);
                await mgr.DeleteProfileAsync(activeProfile);
            }
        }

        private VpnManagementAgent mgr;
        private VpnNativeProfile activeProfile;

        public VpnManagementConnectionStatus Connected { get; private set; }
        public string ActiveProfile { get => activeProfile.ProfileName; }
    }
}
