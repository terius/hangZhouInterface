using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DataAction
    {
        private readonly string TableName = System.Configuration.ConfigurationManager.AppSettings["TableName"];
        private string GetHeadSQL = "select  bill_no from {0} where read_flag1 = '0'";
        private string GetNoSendSQL = "select  bill_no from {0} where send_flag2 = '0'";
        private string updateSQL;
        public DataAction()
        {
            GetHeadSQL = string.Format(GetHeadSQL, TableName);
            GetNoSendSQL = string.Format(GetNoSendSQL, TableName);
            UpdateFailInfoToTMP_SQL= string.Format(UpdateFailInfoToTMP_SQL, TableName);
            UpdateSendFailInfoToTMP_SQL= string.Format(UpdateSendFailInfoToTMP_SQL, TableName);
            UpdateSendSuccessInfoToTMP_SQL = string.Format(UpdateSendSuccessInfoToTMP_SQL, TableName);
        }

        public DataTable GetScanData()
        {
            return DbHelperSQL.Query(GetHeadSQL).Tables[0];
        }


        public DataTable GetNoSendData()
        {
            return DbHelperSQL.Query(GetNoSendSQL).Tables[0];
        }

        

        public int UpdateTmp(IList<ColumnMap> list, Dictionary<string, string> xmlItems)
        {
            IList<SqlParameter> sqlparams = new List<SqlParameter>();
            // int i = 0;
            foreach (var item in list)
            {
                //  i++;
                //   if (i < 100)
                //  {
                sqlparams.Add(new SqlParameter("@" + item.Table, xmlItems[item.XML]));
                // }
            }
            if (string.IsNullOrEmpty(updateSQL))
            {
                updateSQL = CreateUpdateSql(sqlparams, "send_flag1=1,send_time1=getdate()");
            }
            return DbHelperSQL.ExecuteSql(updateSQL, sqlparams);
        }

        private string CreateUpdateSql(IList<SqlParameter> sqlparams, string otherUpdateSql= null)
        {
            StringBuilder sb = new StringBuilder("update " + TableName + " set ");
            foreach (var item in sqlparams)
            {
                sb.AppendFormat("{0}={1},", item.ParameterName.Trim('@'), item.ParameterName);
            }
            if (!string.IsNullOrWhiteSpace(otherUpdateSql))
            {
                sb.Append(otherUpdateSql);
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append(" where BILL_NO=@BILL_NO");
            return sb.ToString();
        }
        

        string UpdateFailInfoToTMP_SQL = "update {0} set send_flag1=2,send_time1=getdate(),errmsg1=@errmsg1 where BILL_NO=@BILL_NO";
        public int UpdateFailInfoToTMP(string bill_no, string errmsg)
        {

            SqlParameter[] sqlparams = {
                new SqlParameter("@errmsg1",errmsg),
                 new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateFailInfoToTMP_SQL, sqlparams);
        }


        string UpdateSendFailInfoToTMP_SQL = "update {0} set send_flag2=2,send_time2=getdate(),errmsg2=@errmsg2 where BILL_NO=@BILL_NO";
        public int UpdateSendFailInfoToTMP(string bill_no, string errmsg)
        {

            SqlParameter[] sqlparams = {
                new SqlParameter("@errmsg2",errmsg),
                 new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateSendFailInfoToTMP_SQL, sqlparams);
        }

        string UpdateSendSuccessInfoToTMP_SQL = "update {0} set send_flag2=1,send_time2=getdate() where BILL_NO=@BILL_NO";
        public int UpdateSendSuccessInfoToTMP(string bill_no)
        {

            SqlParameter[] sqlparams = {
                 new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateSendSuccessInfoToTMP_SQL, sqlparams);
        }


    }
}
