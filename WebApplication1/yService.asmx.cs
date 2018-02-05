﻿using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebApplication1
{
    /// <summary>
    /// yService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class yService : System.Web.Services.WebService
    {
        ///<summary>
        ///获取出口清单或进口个人物品申报单数据,没有找到数据返回null
        ///</summary>
        ///<param name="wbNo">运单号</param>
        ///<returns>出口清单或进口个人物品申报单</returns>
        [WebMethod]
        public string GetInfo(string logistics_No, string app_No)
        {
            XMLInfo info = new XMLInfo();
            head head = new head();
            head.businessType = "ENTRYBILL_INFO";
            head.createTime = "2017/11/22 14:14:03";
            head.status = 1;
            head.errMsg = "";

            ENTRYBILL_HEAD h = new ENTRYBILL_HEAD();
            h.ENTRY_NO = "QD350166852160019603245";
            h.WB_NO = "fyd001";

            List<ENTRYBILL_LIST> list = new List<ENTRYBILL_LIST>();
            for (int i = 0; i < 2; i++)
            {
                ENTRYBILL_LIST l = new ENTRYBILL_LIST();
                l.ENTRY_NO = "QD350166852160019603245";
                l.PASS_NO = "QD350166852160019603245";
                list.Add(l);
            }


            body body = new body();
            body.ENTRYBILL_HEAD = h;
            body.ENTRYBILL_LIST = list;
            info.head = head;
            info.body = body;

            info.version = "1.0.0.1";
            string x = XmlHelper.Serializer(info);
            return x;
            //    DataSet ds = new DataSet();
            //    //出口
            //    DataTable head = DbHelperSQL.QueryReturnDataTable("select * from ENTRYBILL_HEAD where wb_no = '" + wbNo + "'");

            //    DataTable body = DbHelperSQL.QueryReturnDataTable("select * from ENTRYBILL_List where wb_no = '" + wbNo + "'");

            // //   var head = head1.;
            //    head.TableName = "ENTRYBILL_HEAD";
            ////    var body = head1;
            //    body.TableName = "ENTRYBILL_LIST";
            //    ds.Tables.Add(head);
            //    ds.Tables.Add(body);

            //dt.TableName = "ENTRYBILL_HEAD";
            //dt.Columns.Add("ENTRY_NO", typeof(string));
            //dt.Columns.Add("WB_NO", typeof(string));
            //dt.Columns.Add("TRADE_COUNTRY", typeof(string));
            //dt.Columns.Add("I_E_FLAG", typeof(string));
            //dt.Columns.Add("I_E_PORT", typeof(string));
            //dt.Columns.Add("I_E_DATE", typeof(DateTime));
            //dt.Columns.Add("D_DATE", typeof(DateTime));
            //dt.Columns.Add("TRADE_CO", typeof(string));
            //dt.Columns.Add("TRADE_NAME", typeof(string));
            //dt.Columns.Add("OWNER_NAME", typeof(string));
            //dt.Columns.Add("AGENT_NAME", typeof(string));
            //dt.Columns.Add("TRAF_NAME", typeof(string));
            //dt.Columns.Add("VOYAGE_NO", typeof(string));
            //dt.Columns.Add("PACK_NO", typeof(decimal));
            //dt.Columns.Add("GROSS_WT", typeof(decimal));
            //dt.Columns.Add("NET_WT", typeof(decimal));
            //dt.Columns.Add("DECL_TOTAL", typeof(decimal));
            //dt.Columns.Add("NOTE_S", typeof(string));
            //dt.Columns.Add("RG_FLAG", typeof(string));
            //dt.Columns.Add("GJ_FLAG", typeof(Boolean));
            //dt.Columns.Add("RSK_FLAG", typeof(Boolean));
            //dt.Columns.Add("R_FLAG", typeof(Boolean));

            //DataRow dr = dt.NewRow();



            //DataTable dtBody = new DataTable();
            //dtBody.TableName = "ENTRYBILL_LIST";
            //dtBody.Columns.Add("G_NAME", typeof(string));
            //dtBody.Columns.Add("ORIGIN_COUNTRY", typeof(string));
            //dtBody.Columns.Add("TRADE_CURR", typeof(string));

            //    return ds;
        }

        ///<summary>
        ///返回操作结果
        ///</summary>
        ///<param name="wbNo">运单号</param>
        ///<param name="opEr">操作人</param>
        ///<param name="opType">操作类型：01-放行；02-查验；03-捡入待处理区；04-暂停流水线并报警</param>
        ///<param name="opTime">操作时间</param>
        [WebMethod]
        public string SetExam(string logistics_No, string app_No, string exam_Type)
        {
            XMLInfo info = new XMLInfo();
            head head = new head();
            head.businessType = "ENTRYBILL_INFO";
            head.createTime = "2017/11/22 14:14:03";
            head.status = 1;
            head.errMsg = "";
            info.head = head;
            info.version = "1.0.0.1";
            string x = XmlHelper.Serializer(info);
            return x;
        }
    }


}
