using System.ServiceProcess;
using System.Threading;


namespace akubi
{
    internal static class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        static void Main()
        {
            var service = new Akubi();

            // デバッグのときはコンソールアプリとして実行する
            // https://qiita.com/TheParkSider/items/dc0aa0a03c02cec462e6
            if (System.Environment.UserInteractive)
            {
                logger.Info($"[Debug Mode] {service.ServiceName} started.");

                var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "StopService");
                service.StartDebug(null);
                eventWaitHandle.WaitOne();
                service.StopDebug();
            }
            else
            {
                logger.Info($"{service.ServiceName} started.");
                ServiceBase.Run(service);
            }
        }
    }
}
