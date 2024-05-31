using System;
using System.Net;
using System.Reflection;
using System.ServiceProcess;

using akubi.lib;

namespace akubi
{
    public partial class akubi : ServiceBase
    {
        // OnCustomCommand用
        const int SERVICE_ACCEPT_PRESHUTDOWN = 0x100;
        const int SERVICE_CONTROL_PRESHUTDOWN = 0xf;

        readonly string hostName = Dns.GetHostName();
        private Settings settings;

        public akubi()
        {
            InitializeComponent();
            this.settings = new Settings();

            // OnCustomCommand用
            FieldInfo acceptedCommandsFieldInfo = typeof(ServiceBase).GetField("acceptedCommands", BindingFlags.Instance | BindingFlags.NonPublic);
            if (acceptedCommandsFieldInfo == null)
            {
                throw new Exception("acceptedCommands field not found");
            }

            int value = (int)acceptedCommandsFieldInfo.GetValue(this);
            acceptedCommandsFieldInfo.SetValue(this, value | SERVICE_ACCEPT_PRESHUTDOWN);
        }

        public void StartDebug(string[] args) { OnStart(args); }
        public void StopDebug() { OnStop(); }

        private void PostLineNotify(string action)
        {
            var lineNotify = new LineNotify(this.settings.apiUrl, this.settings.lineToken);
            int statusCode = lineNotify.SendMessage($"hostname: {this.hostName}\n{action}");
            lineNotify = null;
        }

        protected override void OnStart(string[] args)
        {
            this.PostLineNotify("起動しました");
        }

        protected override void OnStop()
        {
            //this.PostLineNotify($"{this.ServiceName}を停止しています");
        }
        protected override void OnCustomCommand(int command)
        {
            // 参考
            //　https://ameblo.jp/ramtieger/entry-12241398750.html
            //　https://www.sivachandran.in/2012/03/handling-pre-shutdown-notification-in-c.html
            if (command == SERVICE_CONTROL_PRESHUTDOWN)
            {
                this.PostLineNotify("シャットダウンしています");
            }
            else
            {
                base.OnCustomCommand(command);
            }
        }
    }
}
