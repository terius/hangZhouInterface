using HangZhouTran;
using System.ServiceProcess;
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
