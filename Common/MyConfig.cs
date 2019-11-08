using System;

namespace Common
{
    public class MyConfig
    {
        static MyConfig()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            LoopTimeForScan = Convert.ToInt32(appSettings["LoopTimeForScan"]);
            LoopTimeForRead = Convert.ToInt32(appSettings["LoopTimeForRead"]);
            ReadType = Convert.ToInt32(appSettings["ReadType"]);
            SaveLog = Convert.ToInt32(appSettings["SaveLog"]);
            TableName = appSettings["TableName"];
            ScanPath = appSettings["ScanPath"];
            XMLHEAD_MessageID = appSettings["MessageID"];
            XMLHEAD_MessageType = appSettings["MessageType"];
            XMLHEAD_Sender = appSettings["Sender"];
            XMLHEAD_Version = appSettings["Version"];
            SendPath = appSettings["SendPath"];
            SendPathBak = appSettings["SendPathBak"];
        }

        public static int LoopTimeForScan { get; private set; }
        public static int LoopTimeForRead { get; private set; }

        public static int ReadType { get; private set; }
        public static int SaveLog { get; private set; }
   
        public static string TableName { get; private set; }

        public static string ScanPath { get; private set; }

 

        public static string XMLHEAD_MessageID { get; private set; }
        public static string XMLHEAD_MessageType { get; private set; }
        public static string XMLHEAD_Sender { get; private set; }
        public static string XMLHEAD_Version { get; private set; }

        public static string SendPath { get; private set; }

        public static string SendPathBak { get; private set; }
    }
}
