using Common;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    public class DataAction
    {
        private string TableName;
        private string GetNoSendDataForScan_SQL = "select  * from SCAN where send_flag = 0";
        private string CheckTMP_SQL = "select  count(1) from {0} where BILL_NO = @BILL_NO";
        private string CheckTMP_SQL2 = "select  count(1) from {0} where BILL_NO = @BILL_NO and VOYAGE_NO=@VOYAGE_NO";
        Hashtable htParam = new Hashtable();
        public DataAction()
        {
            TableName = MyConfig.TableName;
            UpdateSendFlag_SQL = string.Format(UpdateSendFlag_SQL, TableName);

            CheckTMP_SQL = string.Format(CheckTMP_SQL, TableName);
            CheckTMP_SQL2 = string.Format(CheckTMP_SQL2, TableName);
        }

       private void CreateHT()
        {
            //VOYAGE_NO,BILL_NO,I_E_FLAG,SEND_NAME,OWNER_NAME,TRADE_NAME,MAIN_G_NAME,PACK_NO,GROSS_WT,TOTAL_VALUE,CURR_CODE,D_DATE,CUST_ORDER,CUST_ER
            htParam["VOYAGE_NO"] = "VOYAGE_NO";
            htParam["BILL_NO"] = "BILL_NO";
            htParam["I_E_FLAG"] = "I_E_FLAG";
            htParam["SEND_NAME"] = "SEND_NAME";
            htParam["OWNER_NAME"] = "OWNER_NAME";
            htParam["TRADE_NAME"] = "TRADE_NAME";
            htParam["MAIN_G_NAME"] = "MAIN_G_NAME";
            htParam["PACK_NO"] = "PACK_NO";
            htParam["GROSS_WT"] = "GROSS_WT";
            htParam["TOTAL_VALUE"] = "TOTAL_VALUE";
            htParam["CURR_CODE"] = "CURR_CODE";
            htParam["D_DATE"] = "D_DATE";
            htParam["CUST_ORDER"] = "DEC_TYPE";
            htParam["CUST_ER"] = "DEC_ER";
        }

        public DataTable GetNoSendDataForScan()
        {
            return DbHelperSQL.Query(GetNoSendDataForScan_SQL).Tables[0];
        }


        readonly string UpdateScanTable_SQL = "update SCAN set RETURNINFO = @RETURNINFO,RETURNTIME=@RETURNTIME,SEND_FLAG=1,SEND_TIME=getdate()"
            + " where ID=@ID";
        public int UpdateScanTable(ResultXML info, long id)
        {
            DateTime returnTime = DateTime.Parse(info.returnTime);
            SqlParameter[] sqlparams = {
                new SqlParameter("@RETURNINFO",info.returnInfo),
                new SqlParameter("@RETURNTIME",returnTime),
                new SqlParameter("@ID",id),
            };
            return DbHelperSQL.ExecuteSql(UpdateScanTable_SQL, sqlparams);
        }

        readonly string GetOracleData_SQL = "select VOYAGE_NO,BILL_NO,I_E_FLAG,SEND_NAME,OWNER_NAME,TRADE_NAME,MAIN_G_NAME,PACK_NO,GROSS_WT,TOTAL_VALUE,CURR_CODE,D_DATE,CUST_ORDER,CUST_ER from x.v_x_edi_freightinfo_scan_fzj ";
        public DataRow GetOracleData(string voyageNo, string billNo)
        {
            var sql = GetOracleData_SQL + " where BILL_NO= :BILL_NO order by D_DATE desc";
            OracleParameter[] sqlparams = {
                new OracleParameter(":BILL_NO",billNo)
            };
            var ds = DbHelperOra.Query(sql, sqlparams);
            DataRow row = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count ==1)
                {
                    row = ds.Tables[0].Rows[0];
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(voyageNo))
                    {
                        sql = GetOracleData_SQL + " where BILL_NO= :BILL_NO and VOYAGE_NO=:VOYAGE_NO order by D_DATE desc";
                        OracleParameter[] sqlparams2 = {
                            new OracleParameter(":BILL_NO",billNo),
                            new OracleParameter(":VOYAGE_NO",voyageNo),
                        };
                        ds = DbHelperOra.Query(sql, sqlparams2);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            row = ds.Tables[0].Rows[0];
                        }
                    }

                    if (row == null)
                    {
                        row = ds.Tables[0].Rows[0];
                    }
                }

               
            }

            return row;
            
        }

        private int UpdateTmp(DataRow row)
        {
            SqlParameter[] sqlparams = {
                new SqlParameter("@BILL_NO",row["BILL_NO"].ToString())
            };
            var icount = DbHelperSQL.GetIntValue(CheckTMP_SQL, sqlparams);
            if (icount > 0)
            {
                if (icount == 1)
                {

                }
            }


            
            if (row == null)
            {

            }
            else
            {

            }
            return 1;
        }

        private int UpdateTmpTable(DataRow row ,int flag)
        {
            return 1;
        }

        private SqlParameter[] CreateSqlParam(DataRow row,int flag,out string sql)
        {
            IList<SqlParameter> listParam = new List<SqlParameter>();
            string whereSql = "";
            string voyageNo = row["VOYAGE_NO"].ToString();
            string billNo = row["BILL_NO"].ToString();
            switch (flag)
            {
                case 1:
                    whereSql = " where BILL_NO=@BILL_NO";
                    listParam.Add(new SqlParameter("@BILL_NO", billNo));
                    break;
                case 2:
                    whereSql = " where BILL_NO=@BILL_NO and VOYAGE_NO=@VOYAGE_NO";
                    listParam.Add(new SqlParameter("@BILL_NO", billNo));
                    listParam.Add(new SqlParameter("@VOYAGE_NO", voyageNo));
                    break;
                case 3:
                    whereSql = " where BILL_NO=@BILL_NO and VOYAGE_NO=@VOYAGE_NO and Entrance_NO= @Entrance_NO";
                    listParam.Add(new SqlParameter("@BILL_NO", billNo));
                    listParam.Add(new SqlParameter("@VOYAGE_NO", voyageNo));
                    var entrance_NO = GetEntrance_NO(voyageNo, billNo);
                    listParam.Add(new SqlParameter("@Entrance_NO", entrance_NO));
                    break;
                default:
                    break;
            }
            StringBuilder sb = new StringBuilder($"update {TableName} set ");
            foreach (string key in htParam.Keys)
            {
                listParam.Add(new SqlParameter("@"+ htParam[key], row[key]));
                //Console.WriteLine(string.Format("{0}-{1}", key, ht[key]));
            }
        }


        private int GetEntrance_NO(string voyageNo,string billNo)
        {
            var sql = "select top 1 Entrance_NO from " + TableName + " where BILL_NO=@BILL_NO and VOYAGE_NO=@VOYAGE_NO order by Entrance_NO desc";
            SqlParameter[] sqlparams = {
                new SqlParameter("@BILL_NO",billNo),
                new SqlParameter("@VOYAGE_NO",voyageNo)
            };

            return DbHelperSQL.GetIntValue(sql, sqlparams);
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
