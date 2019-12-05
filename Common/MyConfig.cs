using System;

namespace Common
{
    public class MyConfig
    {
        static MyConfig()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            LoopTimeForScan = Convert.ToInt32(appSettings["LoopTimeForScan"]);
            SaveLog = Convert.ToInt32(appSettings["SaveLog"]);
            TableName = appSettings["TableName"];
            ScanPath = appSettings["ScanPath"];
            ServerUrl = appSettings["ServerUrl"];
            LoopTimeBeforeReadOracle = Convert.ToInt32(appSettings["LoopTimeBeforeReadOracle"]);
            WebServicePath = appSettings["WebServicePath"];

            LoopTimeForTMP = Convert.ToInt32(appSettings["LoopTimeForTMP"]);

            OpenTMPThread = Convert.ToInt32(appSettings["OpenTMPThread"]);
        }

        public static int LoopTimeForScan { get; private set; }
       
   
        public static int SaveLog { get; private set; }
   
        public static string TableName { get; private set; }

        public static string ScanPath { get; private set; }

 

        public static string ServerUrl { get; private set; }
        public static int LoopTimeBeforeReadOracle { get; private set; }

        public static string WebServicePath { get; private set; }


        public static int LoopTimeForTMP { get; private set; }

        public static int OpenTMPThread { get; private set; }
        
    }
}
