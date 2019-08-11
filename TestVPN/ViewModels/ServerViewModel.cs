using TestVPN.Configuration;

namespace TestVPN.ViewModels
{
    class ServerViewModel : NotificationBase<Server>
    {
        public ServerViewModel(Server server = null) : base(server) { }

        public string Country
        {
            get { return This.country; }
            private set { }
        }
    }
}
