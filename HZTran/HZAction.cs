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
        readonly string saveScanFilePath = "ScanFilesSave";
        readonly string badScanFilePath = "ScanFilesBad";


        public void BeginRun()
        {
            try
            {
                FileHelper.WriteLog("服务已启动");
                //  MyConfig.Load();
                isRun = true;
                CheckDirectory();
                //扫描文件夹
                Thread MainThread = new Thread(RunTask);
                MainThread.IsBackground = true;
                MainThread.Name = "ScanFileToTableThread";
                MainThread.Start();

                //读取数据
                Thread MainThread2 = new Thread(RunTask2);
                MainThread2.IsBackground = true;
                MainThread2.Name = "SendDataToFileThread";
                MainThread2.Start();



            }
            catch (Exception ex)
            {
                Loger.LogMessage("服务报错：" + ex.ToString());
            }
        }





        #region 扫描文件夹中数据保存到数据库
        object obRun = new object();
        private void RunTask()
        {
            lock (obRun)
            {
                while (isRun)
                {
                    ScanFiles();
                    Thread.Sleep(MyConfig.LoopTimeForScan);
                }
            }
        }

        private void ScanFiles()
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(basePath, MyConfig.ScanPath));

            if (MyConfig.ReadType == 1)//扫描.xlsm文件
            {

                foreach (var file in di.EnumerateFiles("*.xlsm"))
                {
                    try
                    {
                        var excelData = ExcelHelper.GetData(file.FullName, 2, 2, 3);
                        da.SaveScanDataForXLSM(excelData);
                        file.MoveTo(Path.Combine(basePath, saveScanFilePath, file.Name));
                    }
                    catch (Exception ex)
                    {
                        file.MoveTo(Path.Combine(basePath, badScanFilePath, file.Name));
                        Loger.LogMessage(ex);
                    }

                }
            }
            else //扫描xml文件
            {
                foreach (var file in di.EnumerateFiles("*.xml"))
                {
                    try
                    {
                        var xmlData = XmlHelper.DeserializeFromFile<awblist>(file.FullName);
                        da.SaveScanDataForXML(xmlData);
                        file.MoveTo(Path.Combine(basePath, saveScanFilePath, file.Name));
                    }
                    catch (Exception ex)
                    {
                        file.MoveTo(Path.Combine(basePath, badScanFilePath, file.Name));
                        Loger.LogMessage(ex);
                    }
                }
            }

        }

        #endregion

        #region 读取send_flag=0的数据发送到指定文件夹中
        object obRun2 = new object();
        private void RunTask2()
        {
            lock (obRun2)
            {
                while (isRun)
                {
                    SendData();
                    Thread.Sleep(MyConfig.LoopTimeForRead);
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
                    string AWB = "";
                    string fileName = "";
                    string fileFullName = "";
                    foreach (DataRow row in ReadData.Rows)
                    {

                        try
                        {
                            AWB = row["AWB"].ToString();
                            FileHelper.WriteLog($"开始处理{AWB}的数据");
                            xml =  CreateSendXML(row);
                            fileName = AWB + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml";
                            fileFullName = Path.Combine(basePath, MyConfig.SendPath, fileName);
                            XmlHelper.SerializerToFile(xml, fileFullName);
                            if (!string.IsNullOrWhiteSpace(MyConfig.SendPathBak))
                            {
                                fileFullName = Path.Combine(FileHelper.CreatePathWithDate(MyConfig.SendPathBak), fileName);
                                XmlHelper.SerializerToFile(xml, fileFullName);
                            }
                            da.UpdateSendFlag(AWB, "1");
                            Thread.Sleep(10);
                        }
                        catch (Exception ex)
                        {
                            da.UpdateSendFlag(AWB, "2");
                            Loger.LogMessage(ex);
                        }

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
            info.Body.AWB_INFO.AWB = row["AWB"].ToString();
            info.Body.AWB_INFO.DEC_TYPE = row["DEC_TYPE"].ToString();
            info.Body.AWB_INFO.MainAWB = row["BILL_NO"].ToString();
            info.Body.AWB_INFO.MX_TIME = ConvertHelper.ToDateTimeStr(row["MX_TIME"], "yyyyMMddHHmmss");
            info.Body.AWB_INFO.M_RESULT = row["M_RESULT"].ToString();
            info.Body.AWB_INFO.VOYAGE_NO = row["VOYAGE_NO"].ToString();

            return info;
        }

        #endregion



        private void CheckDirectory()
        {
            var path = Path.Combine(basePath, MyConfig.ScanPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(basePath, MyConfig.SendPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(basePath, saveScanFilePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(basePath, badScanFilePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }

        public void EndRun()
        {
            FileHelper.WriteLog("服务中止！\r\n\r\n\r\n");
            isRun = false;
        }
    }
}
