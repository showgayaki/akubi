using System;
using System.Net;
using System.ServiceProcess;

using akubi.lib;

namespace akubi
{
    public partial class akubi : ServiceBase
    {
        readonly string hostName = Dns.GetHostName();
        private Settings settings;

        public akubi()
        {
            InitializeComponent();
            this.settings = new Settings();
        }

        public void StartDebug(string[] args) { OnStart(args); }
        public void StopDebug() { OnStop(); }

        private void PostLineNotify(string action)
        {
            var lineNotify = new LineNotify(this.settings.apiUrl, this.settings.lineToken);
            int statusCode = lineNotify.SendMessage($"hostname: {this.hostName}\n{action}");
            lineNotify = null;
            Console.WriteLine(statusCode);
        }

        protected override void OnStart(string[] args)
        {
            this.PostLineNotify("起動しました");
        }

        protected override void OnStop()
        {
            this.PostLineNotify($"{this.ServiceName}を停止しています");
        }
        protected override void OnShutdown()
        {
            this.PostLineNotify("シャットダウンしています");
        }
    }
}
