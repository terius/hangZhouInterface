using Common;
using DAL;
using HangZhouTran;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        IList<ColumnMap> map;

        private void Form1_Load(object sender, EventArgs e)
        {
            return;
            GetColumnMap();
            Dictionary<string, string> xmlItems = new Dictionary<string, string>();
            try
            {
                string filePath = "4.txt";

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                var status = doc.GetElementsByTagName("status").Item(0).InnerText;
                xmlItems.Add("status", status);

                var errMsg = doc.GetElementsByTagName("errMsg").Item(0).InnerText;
                xmlItems.Add("errMsg", errMsg);

                var list = doc.GetElementsByTagName("CBEE_ELIST");
                string val = "";
                foreach (XmlNode item in list)
                {
                    var child = item.ChildNodes;
                    foreach (XmlNode item2 in child)
                    {
                        if (item2.Name.Equals("INSPECTION_STATUS", StringComparison.CurrentCultureIgnoreCase))
                        {
                            val = item2.InnerText;
                            if (item2.InnerText == "放行" || item2.InnerText == "审核通过")
                            {
                                val = "0";
                            }
                            else if (item2.InnerText == "查验")
                            {
                                val = "1";
                            }
                            xmlItems.Add(item2.Name, val);
                        }
                        else if (item2.Name.Equals("PACK_NO", StringComparison.CurrentCultureIgnoreCase)
                            || item2.Name.Equals("GROSS_WEIGHT", StringComparison.CurrentCultureIgnoreCase)
                            )
                        {
                            val = item2.InnerText;
                            double itemp = 0;
                            if (double.TryParse(val, out itemp))
                            {
                                xmlItems.Add(item2.Name, val);
                            }
                            else
                            {
                                xmlItems.Add(item2.Name, null);
                            }


                        }
                        else
                        {
                            xmlItems.Add(item2.Name, item2.InnerText);
                        }
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


             


            }
            catch (Exception ex)
            {

                throw;
            }

            DataAction da = new DataAction();
            var rs = da.UpdateTmp(map, xmlItems);
            MessageBox.Show(rs.ToString());
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

        public void GetColumnMap()
        {
            List<string> lines = File.ReadLines("table.txt").ToList();
            if (lines.Count() < 2)
            {
                throw new Exception("映射文件错误");
            }
         
            var tables = lines[0].Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            var xmls = lines[1].Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            if (tables.Length != xmls.Length)
            {
                Loger.LogMessage("字段对应数错误");
            }
            map = new List<ColumnMap>();
            for (int i = 0; i < tables.Length; i++)
            {
                map.Add(new ColumnMap { Table = tables[i], XML = xmls[i] });
            }

        }

        private async  void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var value = this.textBox1.Text.Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var res =await  client.GetInfoAsync(value, "");
                    AddMessage(res.Body.GetInfoResult);
                }
            }
        }

        private void AddMessage(string text)
        {
            this.txtContent.AppendText(text + "\r\n\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HZAction ac = new HZAction();
            ac.BeginRun();
        }
    }
}
