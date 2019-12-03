using System;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    [XmlRoot("LOGISTICS_LIBRARY_RETURN")]
    public class ResultXML
    {
        public string returnStatus { get; set; }
        public string returnTime { get; set; }
        public string returnInfo { get; set; }

    }
}
