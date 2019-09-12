using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    public class DataAction
    {
        private string TableName;
        private string GetHeadSQL = "select  bill_no from {0} where send_flag1 = '0'";
        private string GetNoSendSQL = "select  bill_no from {0} where send_flag2 = '0'";
        private string updateSQL;
        public DataAction()
        {
            TableName = AppConfig.TableName;
            GetHeadSQL = string.Format(GetHeadSQL, TableName);
            GetNoSendSQL = string.Format(GetNoSendSQL, TableName);
            UpdateSendFlag1Sql = string.Format(UpdateSendFlag1Sql, TableName);
            UpdateSendFlag2Sql = string.Format(UpdateSendFlag2Sql, TableName);
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
            int rs = 0;
            try
            {
                IList<SqlParameter> sqlparams = new List<SqlParameter>();

                foreach (var item in list)
                {
                    sqlparams.Add(new SqlParameter("@" + item.Table, xmlItems[item.XML]));
                }
                if (string.IsNullOrEmpty(updateSQL))
                {
                    updateSQL = CreateUpdateSql(sqlparams, "send_flag1=1,send_time1=getdate(),errmsg1='数据正常'");
                }
                rs = DbHelperSQL.ExecuteSql(updateSQL, sqlparams);
            }
            catch (Exception ex)
            {
                string listString = GetListString(xmlItems);
                Loger.LogMessage("sql:" + updateSQL + "\r\n数据:" + listString + "\r\n错误:" + ex.ToString());
            }

            return rs;

        }

        private string GetListString(Dictionary<string, string> myDict)
        {
            if (myDict == null || myDict.Count < 1)
            {
                return "";
            }
            var myStringBuilder = new StringBuilder();
            bool first = true;
            foreach (KeyValuePair<string, string> pair in myDict)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    myStringBuilder.Append(";");
                }

                myStringBuilder.AppendFormat("{0}={1}", pair.Key, pair.Value);
            }

            return myStringBuilder.ToString();
        }

        private string CreateUpdateSql(IList<SqlParameter> sqlparams, string otherUpdateSql = null)
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


       


        string UpdateSendFlag1Sql = "update {0} set send_flag1=@send_flag1,send_time1=getdate(),errmsg1=@errmsg1 where BILL_NO=@BILL_NO";
        public int UpdateSendFlag1(string bill_no, string send_flag1, string errmsg)
        {

            SqlParameter[] sqlparams = {
                new SqlParameter("@send_flag1",send_flag1),
                new SqlParameter("@errmsg1",errmsg),
                new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateSendFlag1Sql, sqlparams);
        }

        string UpdateSendFlag2Sql = "update {0} set send_flag2=@send_flag2,send_time2=getdate(),errmsg2=@errmsg2 where BILL_NO=@BILL_NO";
        public int UpdateSendFlag2(string bill_no, string send_flag2, string errmsg)
        {
            SqlParameter[] sqlparams = {
                new SqlParameter("@send_flag2",send_flag2),
                new SqlParameter("@errmsg2",errmsg),
                new SqlParameter("@BILL_NO",bill_no)
            };
            return DbHelperSQL.ExecuteSql(UpdateSendFlag2Sql, sqlparams);
        }


    }
}
