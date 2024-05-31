using System.ServiceProcess;
using System.Threading;


namespace akubi
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        static void Main()
        {
            var service = new akubi();
            // デバッグのときはコンソールアプリとして実行する
            // https://qiita.com/TheParkSider/items/dc0aa0a03c02cec462e6
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
