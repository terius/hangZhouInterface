using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        DataAction da = new DataAction();
        private readonly string BackUpPath = System.Configuration.ConfigurationManager.AppSettings["BackupPath"];
        private readonly string ReadPath = System.Configuration.ConfigurationManager.AppSettings["ReadPath"];
        private readonly string ResponsePath = System.Configuration.ConfigurationManager.AppSettings["ResponsePath"];
        ServiceReference1.yServiceSoapClient client = new ServiceReference1.yServiceSoapClient();
        public Form1()
        {
            InitializeComponent();
        }





        private void btnReadFile_Click(object sender, EventArgs e)
        {
            //    ServiceReference1.yServiceSoapClient client = new ServiceReference1.yServiceSoapClient();
            //     var data = client.GetData("fyd001");

            //  HangZhouTran.HZAction action = new HangZhouTran.HZAction();
            //  action.BeginRun();

            var xx = XmlHelper.DeserializeFromFile<NEWXMLInfo>(@"C:\Users\teriushome\Desktop\新建文件夹\新1.txt");
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            //XMLInfo info = new XMLInfo();
            //head head = new head();
            //head.businessType = "ENTRYBILL_INFO";
            //head.createTime = "2017/11/22 14:14:03";
            //head.status = "1";
            //head.errMsg = "";

            //ENTRYBILL_HEAD h = new ENTRYBILL_HEAD();
            //h.ENTRY_NO = "QD350166852160019603245";
            //h.WB_NO = "570670855326";

            //List<ENTRYBILL_LIST> list = new List<ENTRYBILL_LIST>();
            //for (int i = 0; i < 2; i++)
            //{
            //    ENTRYBILL_LIST l = new ENTRYBILL_LIST();
            //    l.ENTRY_NO = "QD350166852160019603245";
            //    l.PASS_NO = "QD350166852160019603245";
            //    list.Add(l);
            //}


            //body body = new body();
            //body.ENTRYBILL_HEAD = h;
            //body.ENTRYBILL_LIST = list;
            //info.head = head;
            //info.body = body;

            //info.version = "1.0.0.1";
            //string x = XmlHelper.Serializer(info);
            Dictionary<string, string> xmlItems = new Dictionary<string, string>();
            try
            {


                string filePath = @"E:\20180816\20180816135432088_od180808009.txt";

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                var list = doc.GetElementsByTagName("CBEE_ELIST");
            
                foreach (XmlNode item in list)
                {
                    var child = item.ChildNodes;
                    foreach (XmlNode item2 in child)
                    {
                        xmlItems.Add(item2.Name, item2.InnerText);
                    }
                }


                list = doc.GetElementsByTagName("CBEE_ELIST_ITEM");
                foreach (XmlNode item in list)
                {
                    var child = item.ChildNodes;
                    foreach (XmlNode item2 in child)
                    {
                        if (xmlItems.ContainsKey(item2.Name))
                        {
                            xmlItems.Add("CBEE_ELIST_ITEM_" + item2.Name, item2.InnerText);
                        }
                        else
                        {
                            xmlItems.Add(item2.Name, item2.InnerText);
                        }
                       
                    }
                }

                var sss = xmlItems["G_NAME"];
            }
            catch (Exception ex)
            {

                throw;
            }
            //   var xmlInfo = XmlHelper.DeserializeFromFile<NEWXMLInfo>(filePath);



            //NEWXMLInfo info = new NEWXMLInfo();
            //info.version = "2.0.0";
            //info.head = new head();
            //info.head.businessType = "CBEE_INFO";
            //info.head.createTime = "2018/8/16 13:50:00";
            //info.head.status = 1;
            //info.head.errMsg = "";
            //info.body = new body();
            //info.body.ENTRYBILL_HEAD = new ENTRYBILL_HEAD();
            //info.body.ENTRYBILL_HEAD.EMS_NO = "";
            //info.body.ENTRYBILL_HEAD.ORDER_NO = "wb180808008";
            //info.body.ENTRYBILL_HEAD.EBC_CODE = "3318961D6D";
            //info.body.ENTRYBILL_HEAD.EBC_NAME = "义乌融易通供应链管理有限公司";
            //info.body.ENTRYBILL_HEAD.LOGISTICS_NO = "wb180808008";
            //info.body.ENTRYBILL_HEAD.LOGISTICS_CODE = "330198Z018";
            //info.body.ENTRYBILL_HEAD.LOGISTICS_NAME = "中外运空运发展股份有限公司浙江分公司";
            //info.body.ENTRYBILL_HEAD.INVT_NO = "29162018E000000122";
            //info.body.ENTRYBILL_HEAD.I_E_FLAG = "E";
            //info.body.ENTRYBILL_HEAD.I_E_DATE = "2017/02/07 00:00:00";
            //info.body.ENTRYBILL_HEAD.AGENT_CODE = "3318961D6D";
            //info.body.ENTRYBILL_HEAD.AGENT_NAME = "义乌融易通供应链管理有限公司";
            //info.body.ENTRYBILL_HEAD.AREA_CODE = "";
            //info.body.ENTRYBILL_HEAD.AREA_NAME = "";
            //info.body.ENTRYBILL_HEAD.TRADE_MODE = "9610";
            //info.body.ENTRYBILL_HEAD.BILL_NO = "20180706004";
            //info.body.ENTRYBILL_HEAD.COUNTRY = "西班牙";
            //info.body.ENTRYBILL_HEAD.PACK_NO = null;
            //info.body.ENTRYBILL_HEAD.GROSS_WEIGHT = 2;
            //info.body.ENTRYBILL_HEAD.DISTRICT_CUSTOMS = "2900";

            //var xml = XmlHelper.Serializer(info);





        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var value = this.textBox1.Text.Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var res = await client.GetInfoAsync(value, "");
                    AddMessage(res.Body.GetInfoResult);
                }
            }
        }

        private void AddMessage(string text)
        {
            this.txtContent.AppendText(text + "\r\n\r\n");
        }
    }
}
