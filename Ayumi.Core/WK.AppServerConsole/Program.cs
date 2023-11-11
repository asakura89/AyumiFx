using System;
using System.Configuration;
using WK.AppStatic;
using WK.AppUtility;

namespace WK.AppServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var appConfig = GetAppConfig();
            var exec = new AppExecutive(appConfig);

            Console.WriteLine("Listening at port {0}", appConfig.Port);
            exec.StartApp();

            Console.WriteLine("Application server is running. Press any key to exit");
            Console.ReadKey();

            exec.ShutDownApp();
        }

        private static AppConfigData GetAppConfig()
        {
            string connString = ConfigurationManager.ConnectionStrings["AppConnectionString"].ConnectionString;
            int port = int.Parse(ConfigurationManager.AppSettings["AppPort"]);

            var appConfig = new AppConfigData();
            appConfig.ConnectionString = connString;
            appConfig.Port = port;

            return appConfig;
        }
    }
}
