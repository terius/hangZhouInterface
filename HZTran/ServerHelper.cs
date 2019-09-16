using Common;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace HangZhouTran
{
    public class ServerHelper
    {
        static ServiceReference1.yServiceSoapClient client = new ServiceReference1.yServiceSoapClient();
      //  static readonly string appNo = System.Configuration.ConfigurationManager.AppSettings["APPNO"];
      //  static readonly int SaveResData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SaveResData"]);

        public static Dictionary<string, string> GetOutputData2(string wbNo,ref string infoData)
        {
            Dictionary<string, string> xmlItems = null;
            try
            {

                var data = client.GetInfo(wbNo, AppConfig.AppNo);
                data = data.Replace("&CO", "");
                infoData = data;
                //   var data = data1.Body.GetInfoResult;
                if (AppConfig.SaveResData == 1)
                {
                    var path = FileHelper.CreatePathWithDate("responseFiles");
                    var xmlFile = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt");
                    FileHelper.SaveToFile(data, xmlFile);
                }

                xmlItems = GetXML(data);
            }
            catch (Exception ex)
            {
                xmlItems = null;
                Loger.LogMessage("GetOutputData2失败：" + ex.ToString());
            }

            return xmlItems;
        }



        private static Dictionary<string, string> GetXML(string xmlStr)
        {
            Dictionary<string, string> xmlItems = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            var filterStr = XmlHelper.Filter(xmlStr);
            doc.LoadXml(filterStr);

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

            return xmlItems;
        }

        private static Dictionary<string, string> GetSendXML(string xmlStr)
        {
            Dictionary<string, string> xmlItems = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);

            var status = doc.GetElementsByTagName("status").Item(0).InnerText;
            xmlItems.Add("status", status);

            var errMsg = doc.GetElementsByTagName("errMsg").Item(0).InnerText;
            xmlItems.Add("errMsg", errMsg);

            return xmlItems;
        }

        public static Dictionary<string, string> putData(string wbNo)
        {
            Dictionary<string, string> items = null;
            try
            {
                var data = client.SetExam(wbNo, AppConfig.AppNo, 1, 1, 23, "");
                if (AppConfig.SaveResData == 1)
                {
                    var path = FileHelper.CreatePathWithDate("putResponseFiles");
                    var xmlFile = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt");
                    FileHelper.SaveToFile(data, xmlFile);
                }
                items = GetSendXML(data);
            }
            catch (Exception ex)
            {
                Loger.LogMessage("putData失败：" + ex.ToString());
            }

            return items;

        }


    }
}
