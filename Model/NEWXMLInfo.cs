using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    [XmlRoot("mo")]
    public class NEWXMLInfo
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
        public int status { get; set; }
        public string errMsg { get; set; }
    }


    public class body
    {
        [XmlElement("CBEE_ELIST")]
        public ENTRYBILL_HEAD ENTRYBILL_HEAD { get; set; }

        [XmlElement("CBEE_ELIST_ITEM")]
        public ENTRYBILL_LIST ENTRYBILL_LIST { get; set; }
    }

    public class ENTRYBILL_HEAD
    {
        /// <summary>
        /// 账册编号
        /// </summary>
        [StringLength(32)]
        public string EMS_NO { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [StringLength(64)]
        public string ORDER_NO { get; set; }

        /// <summary>
        /// 收发货人代码
        /// </summary>
        [StringLength(32)]
        public string EBC_CODE { get; set; }

        /// <summary>
        /// 收发货人名称
        /// </summary>
        [StringLength(300)]
        public string EBC_NAME { get; set; }

        /// <summary>
        /// 物流运单编号
        /// </summary>
        [StringLength(64)]
        public string LOGISTICS_NO { get; set; }


        /// <summary>
        /// 物流企业代码
        /// </summary>
        [StringLength(32)]
        public string LOGISTICS_CODE { get; set; }

        /// <summary>
        /// 物流企业名称
        /// </summary>
        [StringLength(300)]
        public string LOGISTICS_NAME { get; set; }

        /// <summary>
        /// 清单编号
        /// </summary>
        [StringLength(32)]
        public string INVT_NO { get; set; }


        /// <summary>
        /// 进出口标记  I-进口 ， E-出口
        /// </summary>
        [StringLength(4)]
        public string I_E_FLAG { get; set; }

        /// <summary>
        /// 进口日期
        /// </summary>
        public string I_E_DATE { get; set; }

        /// <summary>
        /// 申报企业代码
        /// </summary>
        [StringLength(32)]
        public string AGENT_CODE { get; set; }

        /// <summary>
        /// 申报企业名称
        /// </summary>
        [StringLength(300)]
        public string AGENT_NAME { get; set; }

        /// <summary>
        /// 区内企业代码
        /// </summary>
        [StringLength(32)]
        public string AREA_CODE { get; set; }

        /// <summary>
        /// 区内企业名称
        /// </summary>
        [StringLength(300)]
        public string AREA_NAME { get; set; }

        /// <summary>
        /// 贸易方式
        /// </summary>
        [StringLength(8)]
        public string TRADE_MODE { get; set; }

        /// <summary>
        /// 提运单号
        /// </summary>
        [StringLength(60)]
        public string BILL_NO { get; set; }

        /// <summary>
        /// 运抵国
        /// </summary>
        [StringLength(20)]
        public string COUNTRY { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public string PACK_NO { get; set; }


        /// <summary>
        /// 毛重（kg）
        /// </summary>
        public double GROSS_WEIGHT { get; set; }

        /// <summary>
        /// 直属关区代码
        /// </summary>
        [StringLength(18)]
        public string DISTRICT_CUSTOMS { get; set; }

        /// <summary>
        /// 验放指令 放行，查验，审核通过
        /// </summary>
        [StringLength(20)]
        public string INSPECTION_STATUS { get; set; }

        /// <summary>
        /// 风险提示信息
        /// </summary>
        [StringLength(2000)]
        public string RISK_INFO { get; set; }

        /// <summary>
        /// 总包号
        /// </summary>
        [StringLength(64)]
        public string TOTAL_PACKAGE_NO { get; set; }
    }

    public class ENTRYBILL_LIST
    {
        /// <summary>
        /// 清单ID
        /// </summary>
        [StringLength(64)]
        public string P_ID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [StringLength(4)]
        public string G_NUM { get; set; }

        /// <summary>
        /// 账册备案料号
        /// </summary>
        [StringLength(64)]
        public string ITEM_RECORD_NO { get; set; }

        /// <summary>
        /// 企业商品货号
        /// </summary>
        [StringLength(32)]
        public string ITEM_NO { get; set; }

        /// <summary>
        /// 企业商品品名
        /// </summary>
        [StringLength(2000)]
        public string ITEM_NAME { get; set; }


        public string H_S_CODE { get; set; }

        public string G_NAME { get; set; }

        public string G_MODEL { get; set; }


        /// <summary>
        /// 目的国（地区）代码
        /// </summary>
        [StringLength(20)]
        public string COUNTRY { get; set; }

        /// <summary>
        /// 申报数量
        /// </summary>
        public double QTY { get; set; }

        /// <summary>
        /// 申报计量单位
        /// </summary>
        [StringLength(10)]
        public string UNIT { get; set; }

        /// <summary>
        /// 第一（法定）数量
        /// </summary>
        public double QTY1 { get; set; }

        /// <summary>
        /// 第一(法定)计量单位
        /// </summary>
        [StringLength(10)]
        public string UNIT1 { get; set; }

        /// <summary>
        /// 第二数量
        /// </summary>
        public double QTY2 { get; set; }

        /// <summary>
        /// 第二计量单位
        /// </summary>
        [StringLength(10)]
        public string UNIT2 { get; set; }

        /// <summary>
        /// 申报币制
        /// </summary>
        [StringLength(10)]
        public string CURRENCY { get; set; }

        /// <summary>
        /// 申报单价
        /// </summary>
        public decimal PRICE { get; set; }

        /// <summary>
        /// 申报总价
        /// </summary>
        public decimal TOTAL_PRICE { get; set; }

        /// <summary>
        /// 清单号
        /// </summary>
        [StringLength(32)]
        public string INVT_NO { get; set; }


    }


    public class ColumnMap
    {
        public string Table { get; set; }
        public string XML { get; set; }
    }
}