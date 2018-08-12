using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    [XmlRoot("mo")]
    public class XMLInfo_OLD
    {
        [XmlAttribute("version")]
        public string version { get; set; }
        public head_old head { get; set; }
        public body_old body { get; set; }
    }

    public class head_old
    {
        public string businessType { get; set; }
        public string createTime { get; set; }
        public int status { get; set; }
        public string errMsg { get; set; }
    }


    public class body_old
    {
        public ENTRYBILL_HEAD_old ENTRYBILL_HEAD { get; set; }

        [XmlElement("ENTRYBILL_LIST")]
        public List<ENTRYBILL_LIST_old> ENTRYBILL_LIST { get; set; }
    }

    public class ENTRYBILL_HEAD_old
    {
        public string ENTRY_NO { get; set; }
        public string WB_NO { get; set; }
        public string TRADE_COUNTRY { get; set; }
        public string I_E_FLAG { get; set; }
        public string I_E_PORT { get; set; }
        public string I_E_DATE { get; set; }
        public string D_DATE { get; set; }
        public string TRADE_NAME { get; set; }
        public string OWNER_NAME { get; set; }
        public string AGENT_NAME { get; set; }
        public string LOGIS_NAME { get; set; }
        public string TRADE_MODE { get; set; }
        public string TRAF_MODE { get; set; }
        public string TRAF_NAME { get; set; }
        public string VOYAGE_NO { get; set; }
        public string BILL_NO { get; set; }
        public string WRAP_TYPE { get; set; }
        public decimal PACK_NO { get; set; }

        public decimal GROSS_WT { get; set; }
        public decimal DECL_TOTAL { get; set; }
        public string DECL_PORT { get; set; }
   
        public string RSK_FLAG { get; set; }
        public string EXAM_FLAG { get; set; }
        public int I_E_TYPE { get; set; }
        public string INTERNAL_NAME { get; set; }
        public string EBP_NAME { get; set; }
        public string BAG_NO { get; set; }

        public bool R_FLAG { get; set; }

        public bool GJ_FLAG { get; set; }

        public string Send_FLAG { get; set; }
        public string Op_type { get; set; }
    }

    public class ENTRYBILL_LIST_old
    {
        public string ENTRY_NO { get; set; }
        public string PASS_NO { get; set; }
        public string CODE_TS { get; set; }
        public string G_NAME { get; set; }
        public string G_MODEL { get; set; }
        public string ORIGIN_COUNTRY { get; set; }
        public decimal G_QTY { get; set; }
        public string G_UNIT { get; set; }
        public decimal DECL_PRICE { get; set; }
        public string TRADE_CURR { get; set; }
        public decimal DECL_TOTAL { get; set; }
        public decimal QTY_1 { get; set; }
        public string UNIT_1 { get; set; }
        public decimal QTY_2 { get; set; }
        public string UNIT_2 { get; set; }
       
        public string USD_PRICE { get; set; }
        public int? DUTY_MODE { get; set; }
        public decimal G_WT { get; set; }


    }
}