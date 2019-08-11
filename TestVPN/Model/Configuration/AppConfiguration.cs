using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TestVPN.Configuration
{
    class AppConfiguration
    {
        public static void SaveLastConnectedServer(string server)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[Constants.lastServerPropertyName] = server;
        }

        public static string LoadLastConnectedServer()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values[Constants.lastServerPropertyName] as string;
        }
    }
}
