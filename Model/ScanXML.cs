using System;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    [XmlRoot("LOGISTICS_LIBRARY")]
    public class ScanXML
    {
        public ScanXML()
        {
           // LOGISTICSCODE = "410198Z062 ";
         //   CUSTOMSCODE = "4605";
        }
        public string BILLNO { get; set; }
        public string LOGISTICSNO { get; set; }
        public string JQBH { get; set; }
        public string V_TYPE { get; set; }
        public string I_E_FLAG { get; set; }
        public string V_SOURCE { get; set; }
        public string LOGISTICSCODE { get; set; }
        public string CUSTOMSCODE { get; set; }
    }
}
