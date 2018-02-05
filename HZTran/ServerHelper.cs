using Common;
using Model;
using System;

namespace HangZhouTran
{
    public class ServerHelper
    {
        static ServiceReference1.yServiceSoapClient client = new ServiceReference1.yServiceSoapClient();
        static readonly string appNo = System.Configuration.ConfigurationManager.AppSettings["APPNO"];
        static readonly int SaveResData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SaveResData"]);
        public static XMLInfo GetOutputData(string wbNo)
        {
            var data = client.GetInfo(wbNo, appNo);
            if (SaveResData == 1)
            {
                var xmlFile = AppDomain.CurrentDomain.BaseDirectory + "responseFiles\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt";
                XmlHelper.SaveToFile(data, xmlFile);
            }
            var info = XmlHelper.Deserialize<XMLInfo>(data);
            return info;
        }

        public static void putData(string wbNo, string opType)
        {

            var data = client.SetExam(wbNo, appNo, opType);
            if (SaveResData == 1)
            {
                var xmlFile = AppDomain.CurrentDomain.BaseDirectory + "putResponseFiles\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt";
                XmlHelper.SaveToFile(data, xmlFile);
            }
            //var info = XmlHelper.Deserialize<XMLInfo>(data);
            //return info;

        }
    }
}
