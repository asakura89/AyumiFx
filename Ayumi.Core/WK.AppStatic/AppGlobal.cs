namespace WK.AppStatic
{
    public class AppGlobal
    {
        public static string ConnectionString { get; private set; }
        public static int Port { get; private set; }

        public static void Init(AppConfigData config)
        {
            ConnectionString = config.ConnectionString;
            Port = config.Port;
        }
    }
}
