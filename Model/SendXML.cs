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
        public string MessageID => MyConfig.XMLHEAD_MessageID;
        public string MessageType => MyConfig.XMLHEAD_MessageType;
        public string Sender => MyConfig.XMLHEAD_Sender;

        public string SendTime => DateTime.Now.ToString("yyyyMMddHHmmss");
        public string Version => MyConfig.XMLHEAD_Version;

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
        public string VOYAGE_NO { get; set; }
        public string MainAWB { get; set; }
        public string AWB { get; set; }
        public string MX_TIME { get; set; }

        public string DEC_TYPE { get; set; }

        public string M_RESULT { get; set; }
        public string Site => "1";

    }
}
