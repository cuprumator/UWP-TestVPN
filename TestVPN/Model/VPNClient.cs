using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Vpn;
using Windows.Security.Credentials;
using TestVPN.Configuration;
using System.Diagnostics;

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
            if (profileStatus == VpnManagementErrorStatus.Ok)
            {
                VpnManagementErrorStatus connectStatus = await ManagementAgent.ConnectProfileWithPasswordCredentialAsync(profile, credentials);

                if (connectStatus == VpnManagementErrorStatus.Ok)
                {
                    ActiveProfile = profile;
                    return profile.ConnectionStatus;
                }
                else
                {
                    throw new Exception("Connetion failed");
                }

            }
            else
            {
                throw new Exception("VPN profile add failed");
            }
        }

        public async Task<VpnManagementConnectionStatus> GetStatusAsync()
        {
            var list = await ManagementAgent.GetProfilesAsync();
            foreach (var profile in list)
            {
                if (profile is VpnNativeProfile)
                {
                    var servers = await ConfigurationManager.GetServers();
                    foreach (var server in servers)
                    {
                        if (profile.ProfileName == Constants.connectionProfileName)
                        {
                            VpnNativeProfile nativeProfile = (VpnNativeProfile)profile;
                            try
                            {
                                var status = nativeProfile.ConnectionStatus;
                                if (status == VpnManagementConnectionStatus.Connected)
                                {
                                    ActiveProfile = nativeProfile;
                                    return status;
                                }
                                return status;
                            }
                            catch (Exception)
                            {
                                await ManagementAgent.DeleteProfileAsync(nativeProfile);
                                return VpnManagementConnectionStatus.Disconnected;
                            }
                        }
                    }
                }

            }
            return VpnManagementConnectionStatus.Disconnected;
        }

        public async Task Disconnect()
        {
            if (ActiveProfile != null)
            {
                if (VpnManagementErrorStatus.Ok == await ManagementAgent.DisconnectProfileAsync(ActiveProfile))
                {
                    await RemoveConnection();
                }
                else
                {
                    throw new Exception("Could not disconnect");
                }
            }
            else
            {
                throw new Exception("Active profile not valid");
            }
        }

        public async Task RemoveConnection()
        {
            try
            {
                await ManagementAgent.DeleteProfileAsync(ActiveProfile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private VpnManagementAgent ManagementAgent;
        private VpnNativeProfile ActiveProfile;
    }
}
