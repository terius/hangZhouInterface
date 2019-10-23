using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    [XmlRoot("awblist")]
    public class awblist
    {
        [XmlElement("awb")]
        public List<awb> awb { get; set; }
    }
    public class awb
    {
        public string awbnbr { get; set; }

        
        public trklist trklist { get; set; }
    }

    
    public class trklist
    {
        [XmlElement("trknbr")]
        public List<string> trknbr { get; set; }
    }

   
}
