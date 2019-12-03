using Common;
using Model;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DataAction
    {
        private string TableName;
        private string GetNoSendDataForScan_SQL = "select  * from SCAN where send_flag = 0";
        public DataAction()
        {
            TableName = MyConfig.TableName;
            UpdateSendFlag_SQL = string.Format(UpdateSendFlag_SQL, TableName);
        }

        public DataTable GetNoSendDataForScan()
        {
            return DbHelperSQL.Query(GetNoSendDataForScan_SQL).Tables[0];
        }


        readonly string UpdateScanTable_SQL = "update SCAN set RETURNINFO = @RETURNINFO,RETURNTIME=@RETURNTIME,SEND_FLAG=1,SEND_TIME=getdate() where ";
        public int UpdateScanTable(ResultXML info)
        {
            DateTime returnTime = DateTime.Parse(info.returnTime);
            SqlParameter[] sqlparams = {
                new SqlParameter("@RETURNINFO",info.returnInfo),
                new SqlParameter("@RETURNTIME",returnTime)
            };
            return DbHelperSQL.ExecuteSql(UpdateScanTable_SQL, sqlparams);
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

        string UpdateSendFlag_SQL = "update {0} set send_flag=@send_flag where AWB = @AWB";
        public int UpdateSendFlag(string AWB, string send_flag)
        {
            if (string.IsNullOrWhiteSpace(AWB))
            {
                return 0;
            }
            SqlParameter[] sqlparams = {
                new SqlParameter("@AWB",AWB),
                new SqlParameter("@send_flag",send_flag)
            };
            return DbHelperSQL.ExecuteSql(UpdateSendFlag_SQL, sqlparams);
        }

        string InsertAWB_SQL = "insert into AWB(BILL_NO,AWB) values (@BILL_NO,@AWB)";
        public int SaveScanDataForXLSM(DataTable data)
        {

            string bill_no;
            string awb;
            int rs = 0;
            string row1 = "";
            foreach (DataRow row in data.Rows)
            {
                bill_no = row[0].ToString().Trim();
                row1 = row[1] == null ? "" : row[1].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(bill_no))
                {
                    if (string.IsNullOrWhiteSpace(row1))
                    {
                        awb = bill_no;
                    }
                    else
                    {
                        awb = row1;
                    }

                    rs += InsertOrUpdateAWB(bill_no, awb);

                }
            }
            return rs;
        }



        private bool CheckAWBIsDup(string awb)
        {
            SqlParameter[] sqlparams = {
                   new SqlParameter("@awb",awb)
                };
            return DbHelperSQL.Exists("select count(1) from AWB where awb = @awb", sqlparams);
        }

        private int InsertOrUpdateAWB(string bill_no, string awb)
        {
            SqlParameter[] sqlparams = {
                   new SqlParameter("@BILL_NO",bill_no),
                   new SqlParameter("@AWB",awb)
                };
            if (CheckAWBIsDup(awb))
            {
                return DbHelperSQL.ExecuteSql("update AWB set BILL_NO=@BILL_NO where AWB = @AWB", sqlparams);
            }
            else
            {
                return DbHelperSQL.ExecuteSql(InsertAWB_SQL, sqlparams);
            }
        }


        public int SaveScanDataForXML(awblist data)
        {
            int rs = 0;
            string bill_no;
            string awb;
            foreach (var item in data.awb)
            {
                if (item.trklist != null && item.trklist.trknbr != null)
                {
                    foreach (var awbItem in item.trklist.trknbr)
                    {
                        bill_no = item.awbnbr;
                        awb = awbItem;
                        rs += InsertOrUpdateAWB(bill_no, awb);
                    }
                }
                else
                {
                    bill_no = awb = item.awbnbr;
                    rs += InsertOrUpdateAWB(bill_no, awb);
                }
            }
            return rs;
        }
    }
}
