using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using WK.AppStatic;
using WK.RemotingManager;

namespace WK.AppUtility
{
    public class AppExecutive
    {
        public AppExecutive(AppConfigData config)
        {
            AppGlobal.Init(config);
        }

        public void StartApp()
        {
            StartRemotingServer();
            StartDbManager();
        }

        private void StartDbManager()
        {
            DatabaseManager.Init();
        }

        public void ShutDownApp()
        {
            
        }

        private static void StartRemotingServer()
        {
            var channel = new TcpChannel(AppGlobal.Port);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ObjectFactory), "ObjectFactory", WellKnownObjectMode.Singleton);
        }
    }
}
