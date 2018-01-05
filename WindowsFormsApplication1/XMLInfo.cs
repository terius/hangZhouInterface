using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WindowsFormsApplication1
{
    [Serializable]
    [XmlRoot("mo")]
    public class XMLInfo
    {
        [XmlAttribute("version")]
        public string version { get; set; }
        public head head { get; set; }
        public body body { get; set; }
    }

    public class head
    {
        public string businessType { get; set; }
        public string createTime { get; set; }
        public string status { get; set; }
        public string errMsg { get; set; }
    }


    public class body
    {
        public ENTRYBILL_HEAD ENTRYBILL_HEAD { get; set; }
        public ENTRYBILL_LIST[] ENTRYBILL_LIST { get; set; }
    }

    public class ENTRYBILL_HEAD
    {
        public string ENTRY_NO { get; set; }
        public string WB_NO { get; set; }
    }

    public class ENTRYBILL_LIST
    {
        public string ENTRY_NO { get; set; }
        public string PASS_NO { get; set; }
    }
}