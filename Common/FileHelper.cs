using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Common
{
    public class FileHelper
    {
        // const int LOCK = 500; //申请读写时间
        //  const int SLEEP = 100; //线程挂起时间
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

                var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
                {
                    sw.WriteLine(now +"---------" + msg);
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


        public static string CreatePathWithDate(string pathName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathName, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string CreateFileNameWithDate(string pathName, string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathName, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, fileName);
        }

        public static bool SaveToFile(string content, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                File.WriteAllText(fileName, content);
                return true;
                //using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                //{
                //    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                //    {
                //        sw.Write(content);
                //        sw.Close();
                //    }
                //}
            }
            return false;
        }

        /// <summary>
        /// 去除文件名中的非法字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearInvalidString(string fileName)
        {
            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(rInvalidChar.ToString(), string.Empty);
            }
            return fileName;

        }
    }
}
