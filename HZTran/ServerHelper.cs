using Common;
using Model;
using System;
using System.IO;

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
                var xmlFile = Path.Combine(CreateFilePath("responseFiles"), DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt");
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
                var xmlFile = Path.Combine(CreateFilePath("putResponseFiles"), DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt");
                XmlHelper.SaveToFile(data, xmlFile);
            }
            //var info = XmlHelper.Deserialize<XMLInfo>(data);
            //return info;

        }

        private static string CreateFilePath(string pathName)
        {

            var path = AppDomain.CurrentDomain.BaseDirectory + pathName + "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
