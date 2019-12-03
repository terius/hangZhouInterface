using Common;
using DAL;
using Model;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace HangZhouTran
{
    public class HZAction
    {
        DataAction da = new DataAction();
        volatile bool isRun = false;
        readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;

        readonly string serverUrl = MyConfig.ServerUrl;
        readonly int loopTimeForOracle = MyConfig.LoopTimeBeforeReadOracle;
        readonly string scanPath = MyConfig.ScanPath;
        readonly string webPath = MyConfig.WebServicePath;

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
                //Thread MainThread2 = new Thread(RunTask2);
                //MainThread2.IsBackground = true;
                //MainThread2.Name = "SendDataToFileThread";
                //MainThread2.Start();



            }
            catch (Exception ex)
            {
                Loger.LogMessage("服务报错：" + ex.ToString());
            }
        }


        private void CheckDirectory()
        {
            var path = Path.Combine(basePath, scanPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(basePath, webPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


        }





        #region 扫描Scan表
        object obRun = new object();
        private void RunTask()
        {
            lock (obRun)
            {
                while (isRun)
                {
                    ReadTableScan();
                    Thread.Sleep(MyConfig.LoopTimeForScan);
                }
            }
        }

        private void ReadTableScan()
        {
            try
            {
                var ReadData = da.GetNoSendDataForScan();
                if (ReadData != null && ReadData.Rows.Count > 0)
                {
                    ScanXML xml = null;
                    ResultXML resultXML = null;
                    string xmlStr = "";
                    string result = "";
                    string fileName = "";
                    string fileFullName = "";
                    foreach (DataRow row in ReadData.Rows)
                    {

                        try
                        {
                            xml = CreateScanXML(row);
                            xmlStr = XmlHelper.Serializer(xml);
                           // xmlStr = XmlHelper.XML2HtmlEnCode(xmlStr);
                            var requestStr = string.Format("Request={0}&person_code={1}&login_pwd={2}&xmltype={3}", xmlStr, "fzj", "fzjAAA111aaa", "LOGISTICS_LIBRARY");
                            result = SendDataPost(requestStr);
                            resultXML = XmlHelper.Deserialize<ResultXML>(result);
                            int saveResult = da.UpdateScanTable(resultXML);

                            fileName = xml.BILLNO + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml";
                            fileFullName = FileHelper.CreateFileNameWithDate(scanPath, fileName);
                            XmlHelper.SerializerStringToFile(xmlStr, fileFullName);

                            fileFullName = FileHelper.CreateFileNameWithDate(webPath, fileName);
                            XmlHelper.SerializerToFile(resultXML, fileFullName);

                            Thread.Sleep(loopTimeForOracle);
                            FileHelper.WriteLog(result);
                            //<?xml version="1.0" encoding="utf-8"?><LOGISTICS_LIBRARY><BILLNO>aaaaa</BILLNO><LOGISTICSNO>bbbbb</LOGISTICSNO><JQBH>CT31</JQBH><V_TYPE>50</V_TYPE><I_E_FLAG>I</I_E_FLAG><V_SOURCE>0</V_SOURCE><LOGISTICSCODE>410198Z062</LOGISTICSCODE><CUSTOMSCODE>4605</CUSTOMSCODE></LOGISTICS_LIBRARY>
                       
                        }
                        catch (Exception ex)
                        {
                            Loger.LogMessage(ex.ToString());
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                Loger.LogMessage(ex);
            }

        }

        private string SendDataPost(string strXML)
        {
            string str = "";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(serverUrl));
                byte[] bytes = Encoding.UTF8.GetBytes(strXML);
                httpWebRequest.Method = "Post";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                str = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Loger.LogMessage(ex.ToString());
            }
            return str;
        }

        private ScanXML CreateScanXML(DataRow row)
        {
            var info = new ScanXML();
            info.BILLNO = row["BILLNO"].ToString();
            info.CUSTOMSCODE = row["CUSTOMSCODE"].ToString();
            info.I_E_FLAG = row["I_E_FLAG"].ToString();
            info.JQBH = row["JQBH"].ToString();
            info.LOGISTICSCODE = row["LOGISTICSCODE"].ToString();
            info.LOGISTICSNO = row["LOGISTICSNO"].ToString();
            info.V_SOURCE = row["V_SOURCE"].ToString();
            info.V_TYPE = row["V_TYPE"].ToString();

            return info;
        }

        #endregion

        #region 扫描TMP表
        object obRun2 = new object();
        private void RunTask2()
        {
            lock (obRun2)
            {
                while (isRun)
                {
                    ReadTableTMP();
                    Thread.Sleep(MyConfig.LoopTimeForRead);
                }
            }
        }

        private void ReadTableTMP()
        {
            try
            {
                var ReadData = da.GetNoSendDataForScan();
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
                            xml = CreateSendXML(row);
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



     

        public void EndRun()
        {
            FileHelper.WriteLog("服务中止！\r\n\r\n\r\n");
            isRun = false;
        }
    }
}
