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
            this.ManagementAgent = new VpnManagementAgent();
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

            VpnManagementErrorStatus profileStatus = await ManagementAgent.AddProfileFromObjectAsync(profile);

            VpnManagementErrorStatus connectStatus = await ManagementAgent.ConnectProfileWithPasswordCredentialAsync(profile, credentials);
            ActiveProfile = profile;

            return profile.ConnectionStatus;
        }

        public async Task<VpnManagementConnectionStatus> GetStatusAsync()
        {
            var list = await ManagementAgent.GetProfilesAsync();
            foreach (IVpnProfile profile in list)
            {
                var servers = await ConfigurationManager.GetServers();
                foreach (var server in servers)
                { 
                    if (profile.ProfileName == Constants.connectionProfileName)
                    {
                        VpnNativeProfile nativeProfile = (VpnNativeProfile)profile;
                        var status = nativeProfile.ConnectionStatus;
                        if (status == VpnManagementConnectionStatus.Connected)
                        {
                            ActiveProfile = nativeProfile;
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
            if (ActiveProfile != null)
            {
                await ManagementAgent.DisconnectProfileAsync(ActiveProfile);
                await ManagementAgent.DeleteProfileAsync(ActiveProfile);
            }
        }

        private VpnManagementAgent ManagementAgent;
        private VpnNativeProfile ActiveProfile;

        public VpnManagementConnectionStatus Connected { get; private set; }
    }
}
