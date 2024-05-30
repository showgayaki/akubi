using System;
using System.Diagnostics;
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

        private void postLineNotify(string action)
        {
            var lineNotify = new LineNotify(this.settings.apiUrl, this.settings.lineToken);
            lineNotify.sendMessage($"[{this.hostName}]が{action}");
            lineNotify = null;
        }

        protected override void OnStart(string[] args)
        {
            //this.postLineNotify("起動しました");
        }

        protected override void OnStop()
        {
            // this.postLineNotify("停止しています");
        }
        protected override void OnShutdown()
        {
            this.postLineNotify("シャットダウンしています");
        //base.OnShutdown();
        }

    }
}
