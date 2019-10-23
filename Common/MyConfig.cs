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
            ScanPath = appSettings["ScanPath"];
            SelectColumn = appSettings["SelectColumn"];
            XMLHEAD_MessageID = appSettings["MessageID"];
            XMLHEAD_MessageType = appSettings["MessageType"];
            XMLHEAD_Sender = appSettings["Sender"];
            XMLHEAD_Version = appSettings["Version"];
            SendPath = appSettings["SendPath"];
        }

        public static int LoopTime1 { get; private set; }
        public static int LoopTime2 { get; private set; }

        public static int ReadType { get; private set; }
        public static int SaveLog { get; private set; }
        public static int SaveResData { get; private set; }
        public static string TableName { get; private set; }

        public static string ScanPath { get; private set; }

        public static string SelectColumn { get; private set; }

        public static string XMLHEAD_MessageID { get; private set; }
        public static string XMLHEAD_MessageType { get; private set; }
        public static string XMLHEAD_Sender { get; private set; }
        public static string XMLHEAD_Version { get; private set; }

        public static string SendPath { get; private set; }
    }
}
