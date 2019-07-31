using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Common
{
    public class FileHelper
    {
        const int LOCK = 500; //申请读写时间
        const int SLEEP = 100; //线程挂起时间
        static ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();
        private static readonly int SaveLog = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SaveLog"]);

        public static void WriteLog(string msg) //写入文件
        {
            if (SaveLog != 1)
            {
                return;
            }
            readWriteLock.EnterWriteLock();
            try
            {

                string path = AppDomain.CurrentDomain.BaseDirectory + "Actionlogs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!File.Exists(path))
                {
                    FileStream fs1 = File.Create(path);
                    fs1.Close();
                    Thread.Sleep(10);
                }
               
                using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
                {
                    sw.WriteLine(msg);
                    sw.Flush();
                    sw.Close();
                }
              //  Thread.Sleep(SLEEP);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                readWriteLock.ExitWriteLock();
            }
        }
    }
}
