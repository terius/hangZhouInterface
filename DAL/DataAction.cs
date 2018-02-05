using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Model;

namespace DAL
{
    public class DataAction
    {
        private readonly string TableName = System.Configuration.ConfigurationManager.AppSettings["TableName"];
        private string GetHeadSQL = "select  bill_no from {0} where read_flag = '0'";
        private string UpdateHead_Output_SQL = "update {0} set VOYAGE_NO=@VOYAGE_NO,L_D_PORT=@L_D_PORT,I_E_FLAG=@I_E_FLAG,I_E_PORT=@I_E_PORT,D_DATE=@D_DATE,TRADE_CODE=@TRADE_CODE,"
            + "TRADE_NAME=@TRADE_NAME,OWNER_NAME=@OWNER_NAME,SEND_NAME=@SEND_NAME,PACK_NO=@PACK_NO,GROSS_WT=@GROSS_WT,TOTAL_VALUE=@TOTAL_VALUE,"
            + "RG_FLAG=@RG_FLAG,GJ_FLAG=@GJ_FLAG,RSK_FLAG=@RSK_FLAG,MAIN_G_NAME=@MAIN_G_NAME,SEND_COUNTRY=@SEND_COUNTRY,CURR_CODE=@CURR_CODE,READ_FLAG='1' where BILL_NO=@BILL_NO";
        private string UpdateHeadOpFlagSQL = "update {0} set OPER=@OPER,OP_TYPE=@OP_TYPE,READ_DATE=@READ_DATE,SEND_FLAG=0 where BILL_NO =@BILL_NO";
        private string UpdateHead_RequestFail_SQL = "update {0} set OPER=@OPER,OP_TYPE=@OP_TYPE,READ_DATE=@READ_DATE,read_flag='2',MAIN_G_NAME='无EDI数据',send_flag='0' where BILL_NO =@BILL_NO";
        private string UpdateHeadSendFlagSQL = "update {0} set Send_Flag=@Send_Flag where BILL_NO =@BILL_NO";
        private string GetNoSendSQL = "select  bill_no,OPER,op_type,READ_DATE from {0} where send_flag = '0'";


        private string UpdateHead_Input_SQL = "update {0} set VOYAGE_NO=@VOYAGE_NO,L_D_PORT=@L_D_PORT,I_E_FLAG=@I_E_FLAG,I_E_PORT=@I_E_PORT,I_E_DATE=@I_E_DATE,TRADE_CODE=@TRADE_CODE,"
            + "TRADE_NAME=@TRADE_NAME,OWNER_NAME=@OWNER_NAME,SEND_NAME=@SEND_NAME,TRAF_NAME=@TRAF_NAME,SHIP_ID=@SHIP_ID,PACK_NO=@PACK_NO,GROSS_WT=@GROSS_WT,TOTAL_VALUE=@TOTAL_VALUE,NOTE=@NOTE,"
            + "RG_FLAG=@RG_FLAG,GJ_FLAG=@GJ_FLAG,RSK_FLAG=@RSK_FLAG,MAIN_G_NAME=@MAIN_G_NAME,SEND_COUNTRY=@SEND_COUNTRY,SEND_CITY=@SEND_CITY,OPER=@OPER,DEC_DATE=@DEC_DATE,CURR_CODE=@CURR_CODE,READ_FLAG='1' where BILL_NO=@BILL_NO";

        public DataAction()
        {
            GetHeadSQL = string.Format(GetHeadSQL, TableName);
            UpdateHead_Output_SQL = string.Format(UpdateHead_Output_SQL, TableName);
            UpdateHeadOpFlagSQL = string.Format(UpdateHeadOpFlagSQL, TableName);
            UpdateHead_RequestFail_SQL = string.Format(UpdateHead_RequestFail_SQL, TableName);
            UpdateHeadSendFlagSQL = string.Format(UpdateHeadSendFlagSQL, TableName);
            GetNoSendSQL = string.Format(GetNoSendSQL, TableName);
            UpdateHead_Input_SQL = string.Format(UpdateHead_Input_SQL, TableName);
        }

        public DataTable GetScanData()
        {

            return DbHelperSQL.Query(GetHeadSQL).Tables[0];
        }


        public DataTable GetNoSendData()
        {
            return DbHelperSQL.Query(GetNoSendSQL).Tables[0];
        }


        public int UpdateHead_OutPut(DataSet ds)
        {
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            DataRow drHead = ds.Tables[0].Rows[0];
            DataRow drList = ds.Tables[1].Rows[0];
            IList<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@VOYAGE_NO", drHead["ENTRY_NO"]));
            sqlparams.Add(new SqlParameter("@L_D_PORT", drHead["TRADE_COUNTRY"]));
            sqlparams.Add(new SqlParameter("@I_E_FLAG", drHead["I_E_FLAG"]));
            sqlparams.Add(new SqlParameter("@I_E_PORT", drHead["I_E_PORT"]));
            // sqlparams.Add(new SqlParameter("@I_E_DATE", drHead["I_E_DATE"]));
            sqlparams.Add(new SqlParameter("@D_DATE", drHead["D_DATE"]));
            sqlparams.Add(new SqlParameter("@TRADE_CODE", drHead["TRADE_CO"]));
            sqlparams.Add(new SqlParameter("@TRADE_NAME", drHead["TRADE_NAME"]));
            sqlparams.Add(new SqlParameter("@OWNER_NAME", drHead["OWNER_NAME"]));
            sqlparams.Add(new SqlParameter("@SEND_NAME", drHead["AGENT_NAME"]));
            // sqlparams.Add(new SqlParameter("@TRAF_NAME", drHead["TRAF_NAME"]));
            //  sqlparams.Add(new SqlParameter("@SHIP_ID", drHead["VOYAGE_NO"]));
            sqlparams.Add(new SqlParameter("@PACK_NO", drHead["PACK_NO"]));
            sqlparams.Add(new SqlParameter("@GROSS_WT", drHead["GROSS_WT"]));
            sqlparams.Add(new SqlParameter("@TOTAL_VALUE", drHead["DECL_TOTAL"]));
            // sqlparams.Add(new SqlParameter("@NOTE", drHead["NOTE_S"]));
            sqlparams.Add(new SqlParameter("@RG_FLAG", drHead["RG_FLAG"]));
            sqlparams.Add(new SqlParameter("@GJ_FLAG", drHead["GJ_FLAG"]));
            sqlparams.Add(new SqlParameter("@RSK_FLAG", drHead["RSK_FLAG"]));
            // sqlparams.Add(new SqlParameter("@R_FLAG", drHead["R_FLAG"]));
            sqlparams.Add(new SqlParameter("@MAIN_G_NAME", drList["G_NAME"]));
            sqlparams.Add(new SqlParameter("@SEND_COUNTRY", drList["ORIGIN_COUNTRY"]));
            sqlparams.Add(new SqlParameter("@CURR_CODE", drList["TRADE_CURR"]));
            sqlparams.Add(new SqlParameter("@BILL_NO", drHead["WB_NO"]));
            SqlParameter[] pps = sqlparams.ToArray();
            return DbHelperSQL.ExecuteSql(UpdateHead_Output_SQL, pps);

        }

        public int UpdateHead_OutPut(XMLInfo info)
        {
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    DataRow drHead = ds.Tables[0].Rows[0];
            //    DataRow drList = ds.Tables[1].Rows[0];
            var head = info.body.ENTRYBILL_HEAD;
            var list = info.body.ENTRYBILL_LIST[0];

            IList<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@VOYAGE_NO", head.ENTRY_NO));
            sqlparams.Add(new SqlParameter("@L_D_PORT", head.TRADE_COUNTRY));
            sqlparams.Add(new SqlParameter("@I_E_FLAG", head.I_E_FLAG));
            sqlparams.Add(new SqlParameter("@I_E_PORT", head.I_E_PORT));
            // sqlparams.Add(new SqlParameter("@I_E_DATE", drHead["I_E_DATE"]));
            sqlparams.Add(new SqlParameter("@D_DATE", head.D_DATE));
            sqlparams.Add(new SqlParameter("@TRADE_CODE", ""));
            sqlparams.Add(new SqlParameter("@TRADE_NAME", head.TRADE_NAME));
            sqlparams.Add(new SqlParameter("@OWNER_NAME", head.OWNER_NAME));
            sqlparams.Add(new SqlParameter("@SEND_NAME", head.AGENT_NAME));
            // sqlparams.Add(new SqlParameter("@TRAF_NAME", drHead["TRAF_NAME"]));
            //  sqlparams.Add(new SqlParameter("@SHIP_ID", drHead["VOYAGE_NO"]));
            sqlparams.Add(new SqlParameter("@PACK_NO", head.PACK_NO ));
            sqlparams.Add(new SqlParameter("@GROSS_WT", head.GROSS_WT));
            sqlparams.Add(new SqlParameter("@TOTAL_VALUE", head.DECL_TOTAL));
            // sqlparams.Add(new SqlParameter("@NOTE", drHead["NOTE_S"]));
            sqlparams.Add(new SqlParameter("@RG_FLAG", true));
            sqlparams.Add(new SqlParameter("@GJ_FLAG", true));
            sqlparams.Add(new SqlParameter("@RSK_FLAG", head.RSK_FLAG));
            // sqlparams.Add(new SqlParameter("@R_FLAG", drHead["R_FLAG"]));
            sqlparams.Add(new SqlParameter("@MAIN_G_NAME",list.G_NAME));
            sqlparams.Add(new SqlParameter("@SEND_COUNTRY",list.ORIGIN_COUNTRY));
            sqlparams.Add(new SqlParameter("@CURR_CODE",list.TRADE_CURR));
            sqlparams.Add(new SqlParameter("@BILL_NO", head.WB_NO));
            SqlParameter[] pps = sqlparams.ToArray();
            return DbHelperSQL.ExecuteSql(UpdateHead_Output_SQL, pps);

        }


        public int UpdateHead_InPut(DataSet ds)
        {
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            DataRow drHead = ds.Tables[0].Rows[0];
            DataRow drList = ds.Tables[1].Rows[0];
            IList<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@VOYAGE_NO", drHead["ENTRY_NO"]));
            sqlparams.Add(new SqlParameter("@L_D_PORT", drHead["DESTINATION_PORT"]));
            sqlparams.Add(new SqlParameter("@I_E_FLAG", drHead["I_E_FLAG"]));
            sqlparams.Add(new SqlParameter("@I_E_PORT", drHead["I_E_PORT"]));
            sqlparams.Add(new SqlParameter("@I_E_DATE", drHead["I_E_DATE"]));
            // sqlparams.Add(new SqlParameter("@D_DATE", drHead["D_DATE"]));
            sqlparams.Add(new SqlParameter("@TRADE_CODE", drHead["AGENT_CODE"]));
            sqlparams.Add(new SqlParameter("@TRADE_NAME", drHead["AGENT_NAME"]));
            sqlparams.Add(new SqlParameter("@OWNER_NAME", drHead["RECEIVE_NAME"]));
            sqlparams.Add(new SqlParameter("@SEND_NAME", drHead["SEND_NAME"]));
            sqlparams.Add(new SqlParameter("@TRAF_NAME", drHead["TRAF_NAME"]));
            sqlparams.Add(new SqlParameter("@SHIP_ID", drHead["VOYAGE_NO"]));
            sqlparams.Add(new SqlParameter("@PACK_NO", drHead["PACK_NO"]));
            sqlparams.Add(new SqlParameter("@GROSS_WT", drHead["GROSS_WT"]));
            sqlparams.Add(new SqlParameter("@TOTAL_VALUE", drHead["TOTAL_VALUE"]));
            sqlparams.Add(new SqlParameter("@NOTE", drHead["NOTE_S"]));
            sqlparams.Add(new SqlParameter("@RG_FLAG", drHead["RG_FLAG"]));
            sqlparams.Add(new SqlParameter("@GJ_FLAG", drHead["GJ_FLAG"]));
            sqlparams.Add(new SqlParameter("@RSK_FLAG", drHead["RSK_FLAG"]));
            //  sqlparams.Add(new SqlParameter("@R_FLAG", drHead["R_FLAG"]));
            sqlparams.Add(new SqlParameter("@MAIN_G_NAME", drHead["MAIN_G_NAME"]));
            sqlparams.Add(new SqlParameter("@SEND_COUNTRY", drHead["SEND_COUNTRY"]));
            sqlparams.Add(new SqlParameter("@SEND_CITY", drHead["SEND_CITY"]));
            sqlparams.Add(new SqlParameter("@OPER", drHead["OP_ER"]));
            sqlparams.Add(new SqlParameter("@DEC_DATE", drHead["OP_DATE"]));
            sqlparams.Add(new SqlParameter("@CURR_CODE", drHead["CURR_CODE"]));
            sqlparams.Add(new SqlParameter("@BILL_NO", drHead["WB_NO"]));
            SqlParameter[] pps = sqlparams.ToArray();
            return DbHelperSQL.ExecuteSql(UpdateHead_Input_SQL, pps);

        }

        public int UpdateHeadOpType(string bill_no, string OPER, string OP_TYPE, DateTime opTime)
        {

            SqlParameter[] sqlparams = {
                 new SqlParameter("@OPER",OPER),
                 new SqlParameter("@OP_TYPE",OP_TYPE),
                 new SqlParameter("@READ_DATE",opTime),
                new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateHeadOpFlagSQL, sqlparams);
        }

        public int UpdateHead_RequestFail(string bill_no, string OPER, string OP_TYPE, DateTime opTime)
        {

            SqlParameter[] sqlparams = {
                new SqlParameter("@OPER",OPER),
                 new SqlParameter("@OP_TYPE",OP_TYPE),
                 new SqlParameter("@READ_DATE",opTime),
                new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateHead_RequestFail_SQL, sqlparams);
        }

        public int UpdateHeadSendFlag(string bill_no, string flag)
        {

            SqlParameter[] sqlparams = {
                new SqlParameter("@Send_Flag",flag),
                new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateHeadSendFlagSQL, sqlparams);
        }
    }
}
