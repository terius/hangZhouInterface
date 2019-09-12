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

namespace HangZhouTran
{
    public class HZAction
    {
        DataAction da = new DataAction();
        volatile bool isRun = false;
        IList<ColumnMap> map;
        readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        StringBuilder sbLog = new StringBuilder();
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

            path = AppDomain.CurrentDomain.BaseDirectory + "BadFiles";
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
                        Thread.Sleep(AppConfig.LoopTime1);
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
                        Thread.Sleep(AppConfig.LoopTime2);
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



        private void UpdateHead()
        {
            try
            {
                var ReadData = da.GetScanData();
                if (ReadData != null && ReadData.Rows.Count > 0)
                {
                    var getInfoStatus = "";
                    string bill_no = "";
                    int rs = 0;
                    sbLog.Clear();
                    string xmlData = "";
                    string flag = "";
                    foreach (DataRow dr in ReadData.Rows)
                    {
                        try
                        {
                            bill_no = dr["bill_no"].ToString();
                            var eData = ServerHelper.GetOutputData2(bill_no, ref xmlData);
                            if (eData == null)
                            {
                                flag = "4";
                                da.UpdateSendFlag1(bill_no, flag, "电子口岸无反馈");
                            }
                            else
                            {
                                if (eData["status"] == "0")
                                {
                                    var errmsg = eData["errMsg"];
                                    flag = "2";
                                    da.UpdateSendFlag1(bill_no, flag, errmsg);
                                }
                                else
                                {
                                    flag = "1";
                                    rs = da.UpdateTmp(map, eData);
                                    if (rs < 1)
                                    {
                                        flag = "3";
                                        da.UpdateSendFlag1(bill_no, flag, "数据异常无法写入");
                                        SaveToBadPath(bill_no, xmlData);
                                    }
                                }
                            }
                            getInfoStatus = eData == null ? "fail" : "ok";
                            sbLog.AppendLine($"bill_no:{bill_no},getInfo:{getInfoStatus},status:{eData["status"]},set send_flag1={flag}");
                        }
                        catch (Exception ex)
                        {
                            Loger.LogMessage("读取出错：" + ex.ToString());
                            da.UpdateSendFlag1(bill_no, "3", "执行错误，错误信息：" + ex.Message);
                            SaveToBadPath(bill_no, xmlData);
                            sbLog.AppendLine(ex.Message);
                        }

                    }
                    if (sbLog.Length > 0)
                    {
                        FileHelper.WriteLog(sbLog.ToString());
                    }



                }
            }
            catch (Exception ex)
            {
                Loger.LogMessage(ex);
            }
        }

        private void SaveToBadPath(string bill_no, string data)
        {
            if (AppConfig.SaveResData == 1)
            {
                var path = FileHelper.CreatePathWithDate("BadFiles");
                var xmlFile = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmssfff_") + bill_no + ".txt");
                FileHelper.SaveToFile(data, xmlFile);
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
                da.UpdateSendFlag2(bill_no, "2", errmsg);
            }
            else
            {
                da.UpdateSendFlag2(bill_no, "1", "回写成功");
            }
        }




        public void EndRun()
        {
            FileHelper.WriteLog("服务中止！\r\n\r\n\r\n");
            isRun = false;
        }
    }
}
