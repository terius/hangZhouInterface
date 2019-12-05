using System;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    [XmlRoot("LOGISTICS_LIBRARY_RETURN")]
    public class ResultXML
    {
        public string RETURNSTATUS { get; set; }
        public string RETURNTIME { get; set; }
        public string RETURNINFO { get; set; }

    }
}
