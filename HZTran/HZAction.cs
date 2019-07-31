using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HangZhouTran
{
    public class HZAction
    {
        DataAction da = new DataAction();
        private volatile bool isRun = false;
        private readonly int LoopTime1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LoopTime1"]);
        private readonly int LoopTime2 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LoopTime1"]);
        //  private StringBuilder sbLog;
        IList<ColumnMap> map;
        public void BeginRun()
        {
            try
            {


                FileHelper.WriteLog("服务已启动");
                // sbLog = new StringBuilder();
                isRun = true;
                CheckDirectory();
                GetColumnMap();
                Thread MainThread = new Thread(RunTask);
                MainThread.IsBackground = true;
                MainThread.Name = "HangZhouXrayServer";
                MainThread.Start();

                Thread MainThread2 = new Thread(RunTask2);
                MainThread2.IsBackground = true;
                MainThread2.Name = "HangZhouXrayServer2";
                MainThread2.Start();

            }
            catch (Exception ex)
            {
                Loger.LogMessage("服务报错：" + ex.ToString());
            }
        }
        readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        private void GetColumnMap()
        {

            List<string> lines = File.ReadLines(Path.Combine(basePath, "table.txt")).ToList();
            if (lines.Count < 2)
            {
                throw new Exception("映射文件错误");
            }

            var tables = lines[0].Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            var xmls = lines[1].Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            if (tables.Length != xmls.Length)
            {
                throw new Exception("字段对应数错误");
            }
            map = new List<ColumnMap>();
            for (int i = 0; i < tables.Length; i++)
            {
                map.Add(new ColumnMap { Table = tables[i], XML = xmls[i] });
            }

        }

        private void CheckDirectory()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "responseFiles";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = AppDomain.CurrentDomain.BaseDirectory + "putResponseFiles";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        object obRun = new object();
        private void RunTask()
        {
            try
            {
                lock (obRun)
                {
                    while (isRun)
                    {
                        // sbLog.Clear();
                        //FileHelper.WriteLog("开始读取数据");
                        UpdateHead();
                        // FileHelper.WriteLog(sbLog.ToString());
                        Thread.Sleep(LoopTime1);
                    }
                }

            }
            catch (Exception ex)
            {
                FileHelper.WriteLog("任务出错，服务中止！错误信息：" + ex.Message);
                Loger.LogMessage(ex);
            }
        }

        object obRun2 = new object();
        private void RunTask2()
        {
            try
            {
                lock (obRun2)
                {
                    while (isRun)
                    {
                        UpdateSendData();
                        Thread.Sleep(LoopTime2);
                    }
                }

            }
            catch (Exception ex)
            {
                FileHelper.WriteLog("任务出错，服务中止！错误信息：" + ex.Message);
                Loger.LogMessage(ex);
            }
        }

        private void UpdateSendData()
        {
            try
            {
                var data = da.GetNoSendData();
                string bill_no;
                foreach (DataRow dr in data.Rows)
                {
                    bill_no = dr["bill_no"].ToString();
                    PutData(bill_no);
                }
            }
            catch (Exception ex)
            {
                Loger.LogMessage(ex);
            }
        }





        readonly int _saveLog = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SaveLog"]);
        private string AppendTextWithTime(string msg)
        {
            if (_saveLog != 1)
            {
                return "";
            }
            return string.Format("{0}----{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg);
        }

        private void UpdateHead()
        {
            try
            {
                string msg = "";
                var ReadData = da.GetScanData();
                if (ReadData != null && ReadData.Rows.Count > 0)
                {
                    // ServerHelper server = new ServerHelper();
                    string bill_no="";
                    int rs = 0;
                    foreach (DataRow dr in ReadData.Rows)
                    {
                        try
                        {
                            bill_no = dr["bill_no"].ToString();
                            msg += AppendTextWithTime("查询到send_flag1=0的分运单号：" + bill_no);
                            msg += AppendTextWithTime("开始执行webservice");
                            var eData = ServerHelper.GetOutputData2(bill_no);
                            msg += AppendTextWithTime("执行webservice结束");
                            if (eData == null)
                            {
                                msg += AppendTextWithTime("WebService:GetInfo  fail");
                                continue;
                            }

                            if (eData["status"] == "0")
                            {
                                var errmsg = eData["errMsg"];
                                msg += AppendTextWithTime("response data status=0,errmsg：" + errmsg);
                                da.UpdateFailInfoToTMP(bill_no, errmsg);
                            }
                            else
                            {
                                msg += AppendTextWithTime("response data status=1");
                                rs = da.UpdateTmp(map, eData);
                                msg += AppendTextWithTime("update table " + bill_no + (rs > 0 ? " 成功" : " 失败"));

                            }
                        }
                        catch (Exception ex)
                        {
                            Loger.LogMessage(ex);
                            da.UpdateFailInfoToTMP(bill_no, "执行错误，错误信息："+ ex.Message);
                            msg += AppendTextWithTime(ex.Message);
                        }
                      


                    }
                    if (msg != "")
                    {
                        FileHelper.WriteLog(msg);
                    }



                }
            }
            catch (Exception ex)
            {
                Loger.LogMessage(ex);
            }
        }



        private void PutData(string bill_no)
        {
            var rs = ServerHelper.putData(bill_no);
            if (rs == null)
            {
                throw new Exception("发送机检反馈错误");
            }
            if (rs["status"] == "0")
            {
                var errmsg = rs["errMsg"];
                da.UpdateSendFailInfoToTMP(bill_no, errmsg);
            }
            else
            {
                da.UpdateSendSuccessInfoToTMP(bill_no);
            }
        }

        private bool HasValue(DataSet eData)
        {
            return eData != null && eData.Tables.Count > 0 && eData.Tables[0].Rows.Count > 0;
        }

        //private bool HasValue(XMLInfo eData)
        //{
        //    return eData.head.status == 1;
        //}

        private bool HasValue(DataTable eData)
        {
            return eData != null && eData.Rows.Count > 0;
        }




        public void EndRun()
        {
            FileHelper.WriteLog("服务中止！\r\n\r\n\r\n");
            isRun = false;
        }
    }
}
