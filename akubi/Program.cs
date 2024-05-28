using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace akubi
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        static void Main()
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile(path: "appsettings.json");
            //var configuration = builder.Build();

            var service = new akubi();
            // デバッグのときはコンソールアプリとして実行する
            if (System.Environment.UserInteractive)
            {
                var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "StopService");
                service.StartDebug(null);
                eventWaitHandle.WaitOne();
                service.StopDebug();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
