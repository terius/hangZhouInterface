﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Model
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
        public DateTime createTime { get; set; }
        public int status { get; set; }
        public string errMsg { get; set; }
    }


    public class body
    {
        public ENTRYBILL_HEAD ENTRYBILL_HEAD { get; set; }

        [XmlElement("ENTRYBILL_LIST")]
        public List<ENTRYBILL_LIST> ENTRYBILL_LIST { get; set; }
    }

    public class ENTRYBILL_HEAD
    {
        public string ENTRY_NO { get; set; }
        public string WB_NO { get; set; }
        public string TRADE_COUNTRY { get; set; }
        public string I_E_FLAG { get; set; }
        public string I_E_PORT { get; set; }
        public DateTime? I_E_DATE { get; set; }
        public DateTime? D_DATE { get; set; }
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
        public bool RSK_FLAG { get; set; }
        public bool EXAM_FLAG { get; set; }
        public int I_E_TYPE { get; set; }
        public string INTERNAL_NAME { get; set; }
        public string EBP_NAME { get; set; }
        public string BAG_NO { get; set; }



    }

    public class ENTRYBILL_LIST
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
        public decimal USD_PRICE { get; set; }
        public string DUTY_MODE { get; set; }
        public decimal G_WT { get; set; }


    }
}