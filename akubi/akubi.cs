using System;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

using akubi.lib;

namespace akubi
{
    public partial class Akubi : ServiceBase
    {
        // OnCustomCommand用
        const int SERVICE_ACCEPT_PRESHUTDOWN = 0x100;
        const int SERVICE_CONTROL_PRESHUTDOWN = 0xf;

        private bool isConnected = false;
        private readonly string hostName = Dns.GetHostName();
        private readonly Discord discord = new Discord(Settings.discordWebhookUrl);
        private readonly Internet internet = new Internet(Settings.connectCheckURl);
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Akubi()
        {
            InitializeComponent();

            // OnCustomCommand用
            FieldInfo acceptedCommandsFieldInfo = typeof(ServiceBase).GetField("acceptedCommands",
                BindingFlags.Instance | BindingFlags.NonPublic) ??
                throw new Exception("acceptedCommands field not found");

            int value = (int)acceptedCommandsFieldInfo.GetValue(this);
            acceptedCommandsFieldInfo.SetValue(this, value | SERVICE_ACCEPT_PRESHUTDOWN);
        }

        internal void StartDebug(string[] args) { OnStart(args); }
        internal void StopDebug() { OnStop(); }

        private async Task CheckConnectInternetAsync()
        {
            this.isConnected = await internet.IsConnectedAsync();
        }

        private async void PostDiscordAsync(string action)
        {
            bool isPosted = await discord.SendMessageAsync($"hostname: {hostName}\n{action}");

            if (isPosted) { logger.Info("Post Result: OK"); }
            else { logger.Error("Post Result: NG"); }
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("OnStart()");
            this.CheckConnectInternetAsync().Wait();

            if (this.isConnected)
            {
                logger.Info("Internet Connection: OK");
                this.PostDiscordAsync("起動しました");
            }
            else
            {
                logger.Error("Internet Connection: NG");
            }
        }

        protected override void OnStop()
        {
            logger.Info("OnStop()");
            //this.PostLineNotifyAsync($"{this.ServiceName}を停止しています");
        }
        protected override void OnCustomCommand(int command)
        {
            // 参考
            // https://ameblo.jp/ramtieger/entry-12241398750.html
            // https://www.sivachandran.in/2012/03/handling-pre-shutdown-notification-in-c.html
            logger.Info("OnCustomCommand()");
            if (command == SERVICE_CONTROL_PRESHUTDOWN)
            {
                this.PostDiscordAsync("シャットダウンしています");
            }
            else
            {
                base.OnCustomCommand(command);
            }
        }
    }
}
