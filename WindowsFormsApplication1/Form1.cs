using Common;
using DAL;
using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        DataAction da = new DataAction();
        private readonly string BackUpPath = System.Configuration.ConfigurationManager.AppSettings["BackupPath"];
        private readonly string ReadPath = System.Configuration.ConfigurationManager.AppSettings["ReadPath"];
        private readonly string ResponsePath = System.Configuration.ConfigurationManager.AppSettings["ResponsePath"];
        public Form1()
        {
            InitializeComponent();
        }





        private void btnReadFile_Click(object sender, EventArgs e)
        {
            //    ServiceReference1.yServiceSoapClient client = new ServiceReference1.yServiceSoapClient();
            //     var data = client.GetData("fyd001");

            HangZhouTran.HZAction action = new HangZhouTran.HZAction();
            action.BeginRun();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            XMLInfo info = new XMLInfo();
            head head = new head();
            head.businessType = "ENTRYBILL_INFO";
            head.createTime = "2017/11/22 14:14:03";
            head.status = "1";
            head.errMsg = "";

            ENTRYBILL_HEAD h = new ENTRYBILL_HEAD();
            h.ENTRY_NO = "QD350166852160019603245";
            h.WB_NO = "570670855326";

            ENTRYBILL_LIST[] list = new ENTRYBILL_LIST[2];
            for (int i = 0; i < 2; i++)
            {
                ENTRYBILL_LIST l = new ENTRYBILL_LIST();
                l.ENTRY_NO = "QD350166852160019603245";
                l.PASS_NO = "QD350166852160019603245";
                list[i] = l;
            }


            body body = new body();
            body.ENTRYBILL_HEAD = h;
            body.ENTRYBILL_LIST = list;
            info.head = head;
            info.body = body;

            info.version = "1.0.0.1";
            string x = XmlHelper.Serializer(info);

        }

    }
}
