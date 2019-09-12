using System;

namespace Common
{
    public static class AppConfig
    {
        public readonly static int SaveResData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SaveResData"]);
        public readonly static string AppNo = System.Configuration.ConfigurationManager.AppSettings["APPNO"];
        public readonly static string TableName = System.Configuration.ConfigurationManager.AppSettings["TableName"];
        public readonly static int LoopTime1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LoopTime1"]);
        public readonly static int LoopTime2 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LoopTime1"]);
        public readonly static int SaveProcessLog = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SaveLog"]);

    }
}
