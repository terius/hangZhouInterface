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
        private readonly int LoopTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LoopTime"]);

        public void BeginRun()
        {
            FileHelper.WriteLog("服务已启动");
            isRun = true;
            CheckDirectory();

            Thread MainThread = new Thread(RunTask);
            MainThread.IsBackground = true;
            MainThread.Name = "HangZhouXrayServer";
            MainThread.Start();

            Thread MainThread2 = new Thread(RunTask2);
            MainThread2.IsBackground = true;
            MainThread2.Name = "HangZhouXrayServer2";
            MainThread2.Start();


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
                        //FileHelper.WriteLog("开始读取数据");
                        UpdateHead();
                        // FileHelper.WriteLog("操作完成");
                        Thread.Sleep(LoopTime * 1000);
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
                        //FileHelper.WriteLog("开始读取数据");
                        UpdateSendData();
                        // FileHelper.WriteLog("操作完成");
                        Thread.Sleep(LoopTime * 1000);
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
                string bill_no, oper, opType;
                DateTime opTime = DateTime.Now;
                foreach (DataRow dr in data.Rows)
                {
                    bill_no = dr["bill_no"].ToString();
                    oper = dr["OPER"].ToString();
                    opType = dr["op_type"].ToString();
                    opTime = SaveGetDateTime(dr["READ_DATE"]);
                    FileHelper.WriteLog("发送send_flag = 0的数据   bill_no:" + bill_no + " oper:" + oper + " optype:" + opType + " optime:" + opTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    PutData(bill_no, oper, opType, opTime);
                }
            }
            catch (Exception ex)
            {
                Loger.LogMessage(ex);
            }
        }


        private DateTime SaveGetDateTime(object oDateTime, string defaultTime = null)
        {
            DateTime t = DateTime.Now;
            if (oDateTime != null && oDateTime.ToString() != "")
            {
                bool rs = DateTime.TryParse(oDateTime.ToString(), out t);
                if (!rs)
                {
                    if (!string.IsNullOrEmpty(defaultTime))
                    {
                        t = Convert.ToDateTime(defaultTime);
                    }
                }
            }
            return t;
        }

        private void UpdateHead()
        {
            try
            {
                var ReadData = da.GetScanData();
                if (ReadData != null && ReadData.Rows.Count > 0)
                {
                    // ServerHelper server = new ServerHelper();
                    string bill_no, oper, optype;
                    DateTime opTime = DateTime.Now;
                    int rs = 0;
                    foreach (DataRow dr in ReadData.Rows)
                    {
                        bill_no = dr["bill_no"].ToString();
                        FileHelper.WriteLog("---------------------------");
                        FileHelper.WriteLog("获取到分运单号：" + bill_no);
                        var eData = ServerHelper.GetOutputData(bill_no);
                        if (HasValue(eData))
                        {
                            FileHelper.WriteLog("获取到接口数据");

                            //IorE = eData.Tables[0].Rows[0]["I_E_FLAG"].ToString();
                            //if (IorE == "I")
                            //{
                            //    rs = da.UpdateHead_InPut(eData);
                            //    FileHelper.WriteLog("更新进口数据 " + bill_no + (rs > 0 ? " 成功" : " 失败"));
                            //}
                            //else
                            //{
                            rs = da.UpdateHead_OutPut(eData);
                            FileHelper.WriteLog("更新出口数据 " + bill_no + (rs > 0 ? " 成功" : " 失败"));
                            //  }

                            // var GJ_FLAG = eData.Tables[0].Rows[0]["GJ_FLAG"].ToString();
                            // var R_FLAG = eData.Tables[0].Rows[0]["R_FLAG"].ToString();
                            var RSK_FLAG = eData.body.ENTRYBILL_HEAD.RSK_FLAG;
                            FileHelper.WriteLog("RSK_FLAG = " + RSK_FLAG);
                            if (!RSK_FLAG)
                            {
                                oper = "000000";
                                opTime = DateTime.Now;
                                optype = "01";
                                FileHelper.WriteLog("更新Head   bill_no:" + bill_no + " oper:" + oper + " optype:" + optype + " optime:" + opTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                da.UpdateHeadOpType(bill_no, oper, optype, opTime);
                                // PutData(bill_no, oper, optype, opTime);
                            }
                        }
                        else
                        {
                            string msg = eData.head.errMsg;
                            if (msg == "没有查询结果")
                            {
                                FileHelper.WriteLog("未获取到接口数据");
                                oper = "000000";
                                opTime = DateTime.Now;
                                optype = "04";
                                FileHelper.WriteLog("更新Head   bill_no:" + bill_no + " oper:" + oper + " optype:" + optype + " optime:" + opTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                da.UpdateHead_RequestFail(bill_no, oper, optype, opTime);
                            }
                            else
                            {
                                FileHelper.WriteLog("读取错误，错误信息：" + msg);
                            }
                            // PutData(bill_no, oper, optype, opTime);
                        }
                        FileHelper.WriteLog("---------------------------\r\n\r\n");
                        //   Thread.Sleep(200);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.LogMessage(ex);
            }
        }

        private void PutData(string bill_no, string oper, string opType, DateTime dt)
        {
            ServerHelper.putData(bill_no, opType);
            da.UpdateHeadSendFlag(bill_no, "1");
        }

        private bool HasValue(DataSet eData)
        {
            return eData != null && eData.Tables.Count > 0 && eData.Tables[0].Rows.Count > 0;
        }

        private bool HasValue(XMLInfo eData)
        {
            return eData.head.status == 1;
        }

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
