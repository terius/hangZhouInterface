using Common;
using System;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    [XmlRoot("Message")]
    public class SendXML
    {
        public SendXML()
        {
            Head = new Head();
            Body = new Body();
        }
        public Head Head { get; set; }
        public Body Body { get; set; }
    }



    public class Head
    {
        public Head()
        {
            MessageID = MyConfig.XMLHEAD_MessageID;
            MessageType = MyConfig.XMLHEAD_MessageType;
            Sender = MyConfig.XMLHEAD_Sender;
            SendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            Version = MyConfig.XMLHEAD_Version;
        }
        public string MessageID { get; set; }
        public string MessageType { get; set; }
        public string Sender { get; set; }

        public string SendTime { get; set; }
        public string Version { get; set; }
       
    }

    public class Body
    {
        public Body()
        {
            AWB_INFO = new AWB_INFO();
        }
        public AWB_INFO AWB_INFO { get; set; }
    }

    public class AWB_INFO
    {
        public AWB_INFO()
        {
            Site = "1";
        }
        public string VOYAGE_NO { get; set; }
        public string MainAWB { get; set; }
        public string AWB { get; set; }
        public string MX_TIME { get; set; }

        public string DEC_TYPE { get; set; }

        public string M_RESULT { get; set; }
        public string Site { get; set; }

    }
}
