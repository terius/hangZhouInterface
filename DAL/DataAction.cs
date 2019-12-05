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

        private string GetNoSendDataForTMP_SQL = "select   BILL_NO,F_Weight,M_Result,PICTURE_NAME from {0} where send_flag1 = 0";

        private string UpdateTmpSendFlag_SQL = "update {0} set send_flag1=1,send_time1=getdate() where BILL_NO = @BILL_NO and send_flag1=0";
        Hashtable htParam = new Hashtable();
        public DataAction()
        {
            TableName = MyConfig.TableName;
            CheckTMP_SQL = string.Format(CheckTMP_SQL, TableName);
            CheckTMP_SQL2 = string.Format(CheckTMP_SQL2, TableName);
            GetNoSendDataForTMP_SQL = string.Format(GetNoSendDataForTMP_SQL, TableName);
            UpdateTmpSendFlag_SQL = string.Format(UpdateTmpSendFlag_SQL, TableName);
            CreateHT();
        }

        private void CreateHT()
        {
            //VOYAGE_NO,BILL_NO,I_E_FLAG,SEND_NAME,OWNER_NAME,TRADE_NAME,MAIN_G_NAME,PACK_NO,GROSS_WT,TOTAL_VALUE,CURR_CODE,D_DATE,CUST_ORDER,CUST_ER
            htParam["VOYAGE_NO"] = "VOYAGE_NO";
            // htParam["BILL_NO"] = "BILL_NO";
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


        #region Scan表数据读取
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


        public void TranOracleToTMP(string voyageNo, string billNo)
        {
            FileHelper.WriteLog($"开始根据主运单:{voyageNo},分运单:{billNo}查询Oracle数据库");
            var oracleDataRow = GetOracleData(voyageNo, billNo);
            if (oracleDataRow == null)
            {
                FileHelper.WriteLog("Oracle数据库未找到相关信息,开始更新TMP表为无EDI");
                UpdateTmpIsNoData(voyageNo, billNo);
            }
            else
            {
                FileHelper.WriteLog($"Oracle数据库已找到相关信息,开始更新TMP表");
                var flag = GetUpdateTmpFlag(voyageNo, billNo);
                if (flag == 4)
                {
                    FileHelper.WriteLog($"未在TMP表找到该条记录");
                    return;
                }
                string sql = "";
                var listParam = CreateSqlParam(oracleDataRow, flag, out sql);
                int rs = DbHelperSQL.ExecuteSql(sql, listParam);
                string rsText = rs > 0 ? "成功" : "失败";
                FileHelper.WriteLog($"更新到tmp表{rsText},flag={flag}");
            }

        }


        private void UpdateTmpIsNoData(string voyageNo, string billNo)
        {

            var flag = GetUpdateTmpFlag(voyageNo, billNo);
            string sql = "update " + TableName + " set MAIN_G_NAME ='无EDI数据货物',PACK_NO = 20,GROSS_WT = 1.65,TOTAL_VALUE = 0 "
                + " where ";
            int rs = 0;
            if (flag == 1)
            {
                SqlParameter[] sqlparams = {
                    new SqlParameter("@BILL_NO",billNo)
                };
                rs = DbHelperSQL.ExecuteSql(sql + " BILL_NO=@BILL_NO", sqlparams);
            }
            else if (flag == 2)
            {
                SqlParameter[] sqlparams2 = {
                        new SqlParameter("@BILL_NO",billNo),
                        new SqlParameter("@VOYAGE_NO",voyageNo)
                     };
                rs = DbHelperSQL.ExecuteSql(sql + " BILL_NO=@BILL_NO and VOYAGE_NO=@VOYAGE_NO", sqlparams2);
            }
            if (flag == 1 || flag == 2)
            {
                string rsText = rs > 0 ? "成功" : "失败";
                FileHelper.WriteLog($"更新到tmp表{rsText},flag={flag}");
            }
            else
            {
                FileHelper.WriteLog("tmp表也未找到该条记录");
            }

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
                if (ds.Tables[0].Rows.Count == 1)
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

        private int GetUpdateTmpFlag(string voyageNo, string billNo)
        {
            SqlParameter[] sqlparams = {
                new SqlParameter("@BILL_NO",billNo)
            };
            var icount = DbHelperSQL.GetIntValue(CheckTMP_SQL, sqlparams);
            if (icount > 0)
            {
                if (icount == 1)
                {
                    return 1;
                }
                else
                {
                    SqlParameter[] sqlparams2 = {
                        new SqlParameter("@BILL_NO",billNo),
                        new SqlParameter("@VOYAGE_NO",voyageNo)
                     };
                    icount = DbHelperSQL.GetIntValue(CheckTMP_SQL2, sqlparams2);
                    if (icount == 1)
                    {
                        return 2;
                    }
                    return 3;
                }

            }

            return 4;
        }



        private IList<SqlParameter> CreateSqlParam(DataRow row, int flag, out string sql)
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
                    //  listParam.Add(new SqlParameter("@VOYAGE_NO", voyageNo));
                    break;
                case 3:
                    whereSql = " where BILL_NO=@BILL_NO and VOYAGE_NO=@VOYAGE_NO and Entrance_NO= @Entrance_NO";
                    listParam.Add(new SqlParameter("@BILL_NO", billNo));
                    //  listParam.Add(new SqlParameter("@VOYAGE_NO", voyageNo));
                    var entrance_NO = GetEntrance_NO(voyageNo, billNo);
                    listParam.Add(new SqlParameter("@Entrance_NO", entrance_NO));
                    break;
                default:
                    break;
            }
            StringBuilder sb = new StringBuilder("update " + TableName + " set ");
            foreach (string key in htParam.Keys)
            {
                listParam.Add(new SqlParameter("@" + htParam[key], row[key]));
                sb.AppendFormat(" {0}=@{0},", htParam[key]);
                //Console.WriteLine(string.Format("{0}-{1}", key, ht[key]));
            }
            sb.Remove(sb.Length - 1, 1);
            sql = sb.ToString() + whereSql;
            FileHelper.WriteLog($"TMP表Sql:{sql}");
            return listParam;
        }


        private int GetEntrance_NO(string voyageNo, string billNo)
        {
            var sql = "select top 1 Entrance_NO from " + TableName + " where BILL_NO=@BILL_NO and VOYAGE_NO=@VOYAGE_NO order by Entrance_NO desc";
            SqlParameter[] sqlparams = {
                new SqlParameter("@BILL_NO",billNo),
                new SqlParameter("@VOYAGE_NO",voyageNo)
            };

            return DbHelperSQL.GetIntValue(sql, sqlparams);
        }


        #endregion

        #region TMP表数据读取

        public DataTable GetNoSendDataForTMP()
        {
            return DbHelperSQL.Query(GetNoSendDataForTMP_SQL).Tables[0];
        }

        readonly string insertWeightSql = "insert into x.io_x_weight(ID, KJDH, LSR, CZSJ, ZL, JQBH, HGBZ,TRADE_CODE) values"
                   + "(x.seq_x_weight.nextval,"
                   + ":BILLNO,"
                   + "'fzj',"
                   + "sysdate,"
                   + ":FWEIGHT,"
                   + "'CT31',"
                   + "1,"
                   + " 'FZJ')";

        readonly string insertHgorgjSqL = "insert into x.io_x_hgorgj_order(ID, KJDH, HG, HGSJ, HGR, JQBH, TRADE_CODE, RESULTMASK, JUDGECATEGORY) values"
                    + "(x.seq_X_HGORGJ_ORDER.Nextval, "
                    + ":BILLNO,"
                    + ":MRESULT,"
                    + "sysdate,"
                    + "'fzj',"
                    + "'CT31',"
                    + "'FZJ',"
                    + "'0',"
                    + "'0');";

        readonly string insertPICSql = "insert into x.tb_pic(ID,BILL_NO,V_PATH,D_PASS_DATETIME,V_NOTE,N_SEQ,JQBH) values("
            +"x.seq_PIC.Nextval,"
            + ":BILLNO,"
            + ":PIC,"
            + "sysdate,"
            + "'峰之杰',"
            + "1,"
            + "'CT31');";
        public void TranTMPToOracle(DataRow row)
        {
       
            //BILL_NO,F_Weight,M_Result,PICTURE_NAME
            var bill_no = row["BILL_NO"].ToString();
            var F_Weight = row["F_Weight"].ToString();
            var M_Result = row["M_Result"].ToString();
            var PICTURE_NAME = row["PICTURE_NAME"].ToString();
            FileHelper.WriteLog($"开始存储Oracle表,bill_no:{bill_no},F_Weight:{F_Weight},M_Result:{M_Result},PICTURE_NAME:{PICTURE_NAME}");

            OracleParameter[] sqlparams = {
                new OracleParameter(":BILLNO",bill_no),
                 new OracleParameter(":FWEIGHT",F_Weight),
            };
            int rs = DbHelperOra.ExecuteSql(insertWeightSql, sqlparams);

            OracleParameter[] sqlparams2 = {
                new OracleParameter(":BILLNO",bill_no),
                 new OracleParameter(":MRESULT",M_Result),
            };
             rs += DbHelperOra.ExecuteSql(insertHgorgjSqL, sqlparams2);

            OracleParameter[] sqlparams3 = {
                new OracleParameter(":BILLNO",bill_no),
                 new OracleParameter(":PIC",PICTURE_NAME),
            };
            rs += DbHelperOra.ExecuteSql(insertPICSql, sqlparams3);
            FileHelper.WriteLog($"存储Oracle记录条数：{rs}");

            SqlParameter[] sqlparams4 = {
                new SqlParameter("@BILL_NO",bill_no)
            };
            rs = DbHelperSQL.ExecuteSql(UpdateTmpSendFlag_SQL, sqlparams4);
            FileHelper.WriteLog($"tmp表更新send_flag=1：{rs}");
        }
        #endregion


    }
}
