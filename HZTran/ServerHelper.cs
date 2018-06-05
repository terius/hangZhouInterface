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

            //var file = XmlHelper.DeserializeFromFile<XMLInfo>("d:\\222.txt");
            //return file;
            XMLInfo info = null;
            try
            {
                var data = client.GetInfo(wbNo, appNo);
                if (SaveResData == 1)
                {
                    var path = CreateFilePath("responseFiles");
                    var xmlFile = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt");
                    XmlHelper.SaveToFile(data, xmlFile);
                }
                if (data.Contains("<errMsg>没有查询结果</errMsg>"))
                {
                    return null;
                }
                info = XmlHelper.Deserialize<XMLInfo>(data);
            }
            catch (Exception ex)
            {
                Loger.LogMessage("GetOutputData失败：" + ex.ToString());
            }

            return info;
        }

        public static void putData(string wbNo, string opType)
        {
            try
            {
                var data = client.SetExam(wbNo, appNo, opType);
                if (SaveResData == 1)
                {
                    var path = CreateFilePath("putResponseFiles");
                    var xmlFile = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmssfff_") + wbNo + ".txt");
                    XmlHelper.SaveToFile(data, xmlFile);
                }
            }
            catch (Exception ex)
            {
                Loger.LogMessage("putData失败：" + ex.ToString());
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
