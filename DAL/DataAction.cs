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
            UpdateSendFlag_SQL = string.Format(UpdateSendFlag_SQL, TableName);
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

        string UpdateSendFlag_SQL = "update {0} set send_flag=@send_flag where bill_no = @bill_no";
        public int UpdateSendFlag(string bill_no, string send_flag)
        {
            if (string.IsNullOrWhiteSpace(bill_no))
            {
                return 0;
            }
            SqlParameter[] sqlparams = {
                new SqlParameter("@bill_no",bill_no),
                new SqlParameter("@send_flag",send_flag)
            };
            return DbHelperSQL.ExecuteSql(UpdateSendFlag_SQL, sqlparams);
        }

        string SaveScanData_SQL = "insert into AWB(BILL_NO,AWB) values (@BILL_NO,@AWB)";
        public int SaveScanDataForXLSM(DataTable data)
        {
          
            string bill_no;
            string awb;
            int rs = 0;
            foreach (DataRow row in data.Rows)
            {
                bill_no = row[0].ToString().Trim();
                if (row[1] == null || string.IsNullOrWhiteSpace(row[1].ToString()))
                {
                    awb = bill_no;
                }
                else
                {
                    awb = row[1].ToString().Trim();
                }
               
                SqlParameter[] sqlparams = {
                   new SqlParameter("@BILL_NO",bill_no),
                   new SqlParameter("@AWB",awb)
                };
                rs += DbHelperSQL.ExecuteSql(SaveScanData_SQL, sqlparams);
            }
            return rs;
        }
    }
}
