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
                isRun = true;
                CheckDirectory();
                //扫描Scan表
                Thread MainThread = new Thread(RunTask);
                MainThread.IsBackground = true;
                MainThread.Name = "ScanThread";
                MainThread.Start();

                if (MyConfig.OpenTMPThread == 1)
                {
                    //扫描TMP表
                    Thread MainThread2 = new Thread(RunTask2);
                    MainThread2.IsBackground = true;
                    MainThread2.Name = "TMPThread";
                    MainThread2.Start();
                }



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
                    string billNo = "";
                    string voyageNo = "";
                    long id = 0;
                    foreach (DataRow row in ReadData.Rows)
                    {

                        try
                        {
                            voyageNo = row["BILLNO"].ToString();
                            billNo = row["LOGISTICSNO"].ToString();
                            id = Convert.ToInt64(row["ID"]);
                            xml = CreateScanXML(row);
                            xmlStr = XmlHelper.Serializer(xml);
                            // xmlStr = XmlHelper.XML2HtmlEnCode(xmlStr);
                            var requestStr = string.Format("Request={0}&person_code={1}&login_pwd={2}&xmltype={3}", xmlStr, "fzj", "fzjAAA111aaa", "LOGISTICS_LIBRARY");
                            result = SendDataPost(requestStr);
                            resultXML = XmlHelper.Deserialize<ResultXML>(result);
                            int saveResult = da.UpdateScanTable(resultXML, id);

                            fileName = xml.BILLNO + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml";
                            fileFullName = FileHelper.CreateFileNameWithDate(scanPath, fileName);
                            XmlHelper.SerializerStringToFile(xmlStr, fileFullName);

                            fileFullName = FileHelper.CreateFileNameWithDate(webPath, fileName);
                            XmlHelper.SerializerToFile(resultXML, fileFullName);

                            Thread.Sleep(loopTimeForOracle);

                            da.TranOracleToTMP(voyageNo, billNo);

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
                    Thread.Sleep(MyConfig.LoopTimeForTMP);
                }
            }
        }

        private void ReadTableTMP()
        {
            try
            {
                var ReadData = da.GetNoSendDataForTMP();

                if (ReadData != null && ReadData.Rows.Count > 0)
                {
                    foreach (DataRow row in ReadData.Rows)
                    {
                        try
                        {
                            da.TranTMPToOracle(row);
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



        #endregion





        public void EndRun()
        {
            FileHelper.WriteLog("服务中止！\r\n\r\n\r\n");
            isRun = false;
        }
    }
}
