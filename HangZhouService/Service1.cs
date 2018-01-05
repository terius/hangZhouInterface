using HangZhouTran;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
//using HangZhouTran;

namespace HangZhouInterfaceService
{
    public partial class Service1 : ServiceBase
    {
        HZAction action;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            action = new HZAction();
            action.BeginRun();
        }

        protected override void OnStop()
        {
            action.EndRun();
        }

        protected override void OnPause()
        {
            
        }
        protected override void OnContinue()
        {
           
        }
        protected override void OnShutdown()
        {
            OnStop();
        }
    }
}
