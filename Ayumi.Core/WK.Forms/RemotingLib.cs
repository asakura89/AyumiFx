using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using WK.RemotingInterface;

namespace WK.Forms
{
    class RemotingClientLib
    {
        public static IObjectFactory CreateObjectFactory(string server, int port)
        {
            var tcpChannel = new TcpChannel();
            ChannelServices.RegisterChannel(tcpChannel, false);

            var objectFactory = (IObjectFactory)Activator.GetObject(typeof(IObjectFactory),
                string.Format("tcp://{0}:{1}/ObjectFactory", server, port));

            return objectFactory;
        }
    }
}
