using Common;
using DAL;
using Model;
using System;
using System.Data;
using System.IO;
using System.Threading;

namespace HangZhouTran
{
    public class HZAction
    {
        DataAction da = new DataAction();
        volatile bool isRun = false;
        readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;

        public void BeginRun()
        {
            try
            {
                FileHelper.WriteLog("服务已启动");
                MyConfig.Load();
                isRun = true;
                CheckDirectory();
                Thread MainThread = new Thread(RunTask);
                MainThread.IsBackground = true;
                MainThread.Name = "ReadFileThread";
                MainThread.Start();

                //Thread MainThread2 = new Thread(RunTask2);
                //MainThread2.IsBackground = true;
                //MainThread2.Name = "ReadTableThread";
                //MainThread2.Start();

            }
            catch (Exception ex)
            {
                Loger.LogMessage("服务报错：" + ex.ToString());
            }
        }



        private void CheckDirectory()
        {
            var path = MyConfig.ScanPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = MyConfig.SendPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "ScanFileSave");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = AppDomain.CurrentDomain.BaseDirectory + "Send";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #region 扫描文件夹
        object obRun = new object();
        private void RunTask()
        {
            lock (obRun)
            {
                while (isRun)
                {
                    SendData();
                    Thread.Sleep(MyConfig.LoopTime1);
                }
            }
        }

        private void SendData()
        {
            try
            {
                var ReadData = da.GetNoSendData();
                SendXML xml = null;
                if (ReadData != null && ReadData.Rows.Count > 0)
                {
                    foreach (DataRow row in ReadData.Rows)
                    {
                        xml = CreateSendXML(row);
                        var fileName = Path.Combine(MyConfig.SendPath, DateTime.Now.ToString("send_yyyyMMddHHmmssfff.xml"));
                        XmlHelper.SerializerToFile(xml, fileName);
                    }

                }
            }
            catch (Exception ex)
            {
                Loger.LogMessage(ex);
            }
        }

        private SendXML CreateSendXML(DataRow row)
        {
            var info = new SendXML();
            info.Body.AWB_INFO.AWB = row["BILL_NO"].ToString();
            info.Body.AWB_INFO.DEC_TYPE = row["DEC_TYPE"].ToString();
            info.Body.AWB_INFO.MainAWB = da.GetBILLNOFromAWB(row["BILL_NO"].ToString());
            info.Body.AWB_INFO.MX_TIME = ConvertHelper.ToDateTimeStr(row["MX_TIME"],"yyyyMMddHHmmss");
            info.Body.AWB_INFO.M_RESULT = row["M_RESULT"].ToString();
            info.Body.AWB_INFO.VOYAGE_NO = row["VOYAGE_NO"].ToString();

            return info;
        }

        #endregion


        public void EndRun()
        {
            FileHelper.WriteLog("服务中止！\r\n\r\n\r\n");
            isRun = false;
        }
    }
}
