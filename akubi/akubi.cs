using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace akubi
{
    public partial class akubi : ServiceBase
    {
        public akubi()
        {
            InitializeComponent();
        }
        public void StartDebug(string[] args){ OnStart(args); }
        public void StopDebug() { OnStop(); }
        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
