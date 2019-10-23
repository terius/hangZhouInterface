using Common;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DataAction
    {
        private string TableName;
        private string GetNoSendData_SQL = "select  {0} from {1} where send_flag = 0";
        public DataAction()
        {
            TableName = MyConfig.TableName;
            GetNoSendData_SQL = string.Format(GetNoSendData_SQL, MyConfig.SelectColumn, TableName);
        }

        public DataTable GetNoSendData()
        {
            return DbHelperSQL.Query(GetNoSendData_SQL).Tables[0];
        }


        string GetBILLNOANDAWB_SQL = "select top 1 BILL_NO from AWB where AWB=@AWB";
        public string GetBILLNOFromAWB(string awb)
        {
            SqlParameter[] sqlparams = {
                new SqlParameter("@AWB",awb)
            };
            var ob = DbHelperSQL.GetSingle(GetBILLNOANDAWB_SQL, sqlparams);
            return ob != null ? ob.ToString() : awb;
        }
    }
}
