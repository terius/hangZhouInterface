using System;

namespace Common
{
    public class MyConfig
    {
        public static void Load()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            LoopTime1 = Convert.ToInt32(appSettings["LoopTime1"]);
            LoopTime2 = Convert.ToInt32(appSettings["LoopTime2"]);
            ReadType = Convert.ToInt32(appSettings["ReadType"]);
            SaveLog = Convert.ToInt32(appSettings["SaveLog"]);
            SaveResData = Convert.ToInt32(appSettings["SaveResData"]);
            TableName = appSettings["TableName"];
        }

        public static int LoopTime1 { get; private set; }
        public static int LoopTime2 { get; private set; }

        public static int ReadType { get; private set; }
        public static int SaveLog { get; private set; }
        public static int SaveResData { get; private set; }
        public static string TableName { get; private set; }
    }
}
