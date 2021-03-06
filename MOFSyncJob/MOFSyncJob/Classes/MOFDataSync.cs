﻿using BOL.Wizard;
using MOFSyncJob.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace MOFSyncJob
{
    internal class MOFDataSync
    {
        internal static DataTable wizardData = new DataTable();

        #region EDFI    
        internal static void EDFIData()
        {
            LogFileHelper.logList = new ArrayList();
            ArrayList dtList = new ArrayList();
            //RD005_maklumat_peralatan_import
            //DataTable dtMaklumatPeralatan = getMaklumatPeralatan();
            //dtList.Add(dtMaklumatPeralatan);

            //RD001_maklumat_permohonan
            Console.WriteLine(string.Format("[{0}] : Start Sync EDFI Data..", DateTime.Now.ToString()));
            LogFileHelper.logList.Add(string.Format("[{0}] : Start Sync EDFI Data...", DateTime.Now.ToString()));
            DataTable dtMaklumatPermohonan = getMaklumatPermohonan();
            dtList.Add(dtMaklumatPermohonan);
            Console.WriteLine(string.Format("[{0}] : Finished fetch Data from EXPAT DB..", DateTime.Now.ToString()));
            LogFileHelper.logList.Add(string.Format("[{0}] : Finished fetch Data from EXPAT DB...", DateTime.Now.ToString()));
            InsertMaklumatPermohonan(dtMaklumatPermohonan);
            Console.WriteLine(string.Format("[{0}] : Finished all sync..", DateTime.Now.ToString()));
            LogFileHelper.logList.Add(string.Format("[{0}] : Finished all sync..", DateTime.Now.ToString()));
            //R001_kod_stesen_kastam
            //DataTable dtStesenKastam = getStesenKastam();
            //dtList.Add(dtStesenKastam);

            //R005_kod_unit_ukuran
            //DataTable dtKodUnitUkuran = getKodUnitUkuran();
            //dtList.Add(dtKodUnitUkuran);


        }

        private static void InsertMaklumatPermohonan(DataTable dtMaklumatPermohonan)
        {
            // INSERT INTO[dbo].[EDFI_Maklumat_Permohonan]
            //([no_permohonan]
            //,[roc]
            //,[tarikh_permohonan]
            //,[tarikh_terima]
            //,[no_ruj_mdec]
            //,[maklumat_import]
            //,[pengesyoran]
            //,[tarikh_mula]
            //,[tarikh_akhir]
            //,[jum_duti_import]
            //,[status_syor_permohonan]
            //,[no_mesyuarat]
            //,[tarikh_mesyuarat]
            //,[pic_name]
            //,[pic_no_tel]
            //,[pic_emel]
            //,[SyncDate])
            foreach (DataRow dr in dtMaklumatPermohonan.Rows)
            {
                if (!CheckEDFIRecordExist(dr["no_permohonan"].ToString(), dr["roc"].ToString()))
                {
                    SqlConnection Connection = SQLHelper.GetConnectionMOF();
                    SqlCommand com = new SqlCommand();
                    System.Text.StringBuilder sql = new System.Text.StringBuilder();

                    DateTime SyncDate = DateTime.Now;
                    string sSyncDate = SyncDate.ToString("yyyy-MM-dd");
                    sql.AppendLine("INSERT INTO[dbo].[EDFI_Maklumat_Permohonan] ");
                    sql.AppendLine("(");
                    sql.AppendLine("[no_permohonan],[roc],[tarikh_permohonan],[tarikh_terima],[no_ruj_mdec],[maklumat_import],[pengesyoran]");
                    sql.AppendLine(",[tarikh_mula],[tarikh_akhir],[jum_duti_import],[status_syor_permohonan],[no_mesyuarat],[tarikh_mesyuarat]");
                    sql.AppendLine(",[pic_name],[pic_no_tel],[pic_emel],[SyncDate]) ");
                    sql.AppendLine(")");
                    sql.AppendLine("VALUES(");
                    sql.AppendLine("@no_permohonan, @roc, @tarikh_permohonan, @tarikh_terima, @no_ruj_mdec, @maklumat_import, @pengesyoran, ");
                    sql.AppendLine("@tarikh_mula, @tarikh_akhir, @jum_duti_import, @status_syor_permohonan, @no_mesyuarat, @tarikh_mesyuarat,");
                    sql.AppendLine("@pic_name, @pic_no_tel, @pic_emel,SyncDate");
                    sql.AppendLine(")");

                    com.Parameters.Add(new SqlParameter("@no_permohonan", dr["no_permohonan"].ToString()));
                    com.Parameters.Add(new SqlParameter("@roc", dr["roc"].ToString()));
                    com.Parameters.Add(new SqlParameter("@tarikh_permohonan", dr["tarikh_permohonan"].ToString()));
                    com.Parameters.Add(new SqlParameter("@tarikh_terima", dr["tarikh_terima"].ToString()));
                    com.Parameters.Add(new SqlParameter("@no_ruj_mdec", dr["no_ruj_mdec"].ToString()));
                    com.Parameters.Add(new SqlParameter("@maklumat_import", dr["maklumat_import"].ToString()));
                    com.Parameters.Add(new SqlParameter("@pengesyoran", dr["pengesyoran"].ToString()));
                    com.Parameters.Add(new SqlParameter("@tarikh_mula", dr["tarikh_mula"].ToString()));
                    com.Parameters.Add(new SqlParameter("@tarikh_akhir", dr["tarikh_akhir"].ToString()));
                    com.Parameters.Add(new SqlParameter("@jum_duti_import", dr["jum_duti_import"].ToString()));
                    com.Parameters.Add(new SqlParameter("@status_syor_permohonan", dr["status_syor_permohonan"].ToString()));
                    com.Parameters.Add(new SqlParameter("@no_mesyuarat", dr["no_mesyuarat"].ToString()));
                    com.Parameters.Add(new SqlParameter("@tarikh_mesyuarat", dr["tarikh_mesyuarat"].ToString()));
                    com.Parameters.Add(new SqlParameter("@pic_name", dr["pic_name"].ToString()));
                    com.Parameters.Add(new SqlParameter("@pic_no_tel", dr["pic_no_tel"].ToString()));
                    com.Parameters.Add(new SqlParameter("@pic_emel", dr["pic_emel"].ToString()));
                    com.Parameters.Add(new SqlParameter("@SyncDate", DateTime.Now));

                    com.CommandText = sql.ToString();
                    com.CommandType = CommandType.Text;
                    com.Connection = Connection;
                    com.CommandTimeout = int.MaxValue;

                    try
                    {
                        //con.Open()
                        com.ExecuteNonQuery();
                        Console.WriteLine(string.Format("[{0}] : New record inserted in EDFI_Maklumat_Permohonan table", DateTime.Now.ToString()));
                        LogFileHelper.logList.Add(string.Format("[{0}] : New record inserted in EDFI_Maklumat_Permohonan table", DateTime.Now.ToString()));
                    }
                    catch
                    {

                    }
                    finally
                    {
                        Connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("[{0}] : Record existed, skipped..", DateTime.Now.ToString()));
                    LogFileHelper.logList.Add(string.Format("[{0}] : Record existed, skipped..", DateTime.Now.ToString()));
                }
            }


        }

        private static bool CheckEDFIRecordExist(string NoPermohonan, string ROCNumber)
        {
            string sSyncDate = DateTime.Now.ToString("yyyy-MM-dd");
            bool existed = false;
            SqlConnection con = SQLHelper.GetConnectionMOF();
            SqlCommand com = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter(com);

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine("Select TOP 1 roc from [MOF].[dbo].[EDFI_Maklumat_Permohonan] where [no_permohonan]='" + NoPermohonan + "' AND [roc]='" + ROCNumber + "'");
            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = con;
            com.CommandTimeout = int.MaxValue;

            try
            {
                DataTable dt = new DataTable();

                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                    existed = true;
                return existed;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        private static DataTable getKodUnitUkuran()
        {
            DataTable output = new DataTable("R005_kod_unit_ukuran");
            string connectionString = @"Server=192.168.22.202;userid=itduser;password=Mdec@2016;Database=edfi2;Convert Zero Datetime=True";
            MySqlConnection conn = null;
            conn = new MySqlConnection(connectionString);
            conn.Open();

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT * FROM R005_kod_unit_ukuran");
                MySqlDataAdapter returnVal = new MySqlDataAdapter(sql.ToString(), conn);
                returnVal.Fill(output);
            }
            catch (Exception)
            {

                throw;
            }
            conn.Close();
            return output;
        }
        private static DataTable getStesenKastam()
        {
            DataTable output = new DataTable("R001_kod_stesen_kastam");
            string connectionString = @"Server=192.168.22.202;userid=itduser;password=Mdec@2016;Database=edfi2;Convert Zero Datetime=True";
            MySqlConnection conn = null;
            conn = new MySqlConnection(connectionString);
            conn.Open();

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT * FROM R001_kod_stesen_kastam");
                MySqlDataAdapter returnVal = new MySqlDataAdapter(sql.ToString(), conn);
                returnVal.Fill(output);
            }
            catch (Exception)
            {

                throw;
            }
            conn.Close();
            return output;
        }
        private static DataTable getMaklumatPermohonan()
        {
            DataTable output = new DataTable("RD001_maklumat_permohonan");
            string connectionString = @"Server=192.168.22.202;userid=itduser;password=Mdec@2016;Database=edfi2;Convert Zero Datetime=True";
            MySqlConnection conn = null;
            conn = new MySqlConnection(connectionString);
            conn.Open();

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT * FROM RD001_maklumat_permohonan");
                MySqlDataAdapter returnVal = new MySqlDataAdapter(sql.ToString(), conn);
                returnVal.Fill(output);
            }
            catch (Exception)
            {

                throw;
            }
            conn.Close();
            return output;
        }
        private static DataTable getMaklumatPeralatan()
        {
            DataTable output = new DataTable("RD005_maklumat_peralatan_import");
            string connectionString = @"Server=192.168.22.202;userid=itduser;password=Mdec@2016;Database=edfi2;Convert Zero Datetime=True";
            MySqlConnection conn = null;
            conn = new MySqlConnection(connectionString);
            conn.Open();

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT * FROM RD005_maklumat_peralatan_import");
                MySqlDataAdapter returnVal = new MySqlDataAdapter(sql.ToString(), conn);
                returnVal.Fill(output);
            }
            catch (Exception)
            {

                throw;
            }
            conn.Close();
            return output;
        }
        #endregion
        #region MSC
        internal static void Start(bool isStartUp)
        {
            LogFileHelper.logList = new ArrayList();
            DateTime SyncDate = DateTime.Now;
            using (SqlConnection Connection = SQLHelper.GetConnection())
            {
                Console.WriteLine(string.Format("[{0}] : Start Sync MOF Sync Data..", DateTime.Now.ToString()));
                LogFileHelper.logList.Add(string.Format("[{0}] : Start Sync MOF Sync Data...", DateTime.Now.ToString()));
                try
                {
                    //READ FROM SPBigFIle:
                    string SyncedDate = "";
                    if (!isStartUp)
                    {
                        wizardData = SelectACApprovedAccountList(); Console.WriteLine(string.Format("[{0}] : successful retrieved data from spbig file", DateTime.Now.ToString()));
                        LogFileHelper.logList.Add(string.Format("[{0}] : successful retrieved data from spbig file", DateTime.Now.ToString()));
                    }
                    else
                    {
                        //READ FROM EXCEL FILE:
                        string ExcelFileName = getLatestExcelFile();
                        wizardData = ExcelToDT.exceldata(ExcelFileName); Console.WriteLine(string.Format("[{0}] : successful retrieved MSC4Startup data from excel file", DateTime.Now.ToString()));
                        LogFileHelper.logList.Add(string.Format("[{0}] : successful retrieved MSC4Startup data from excel file", DateTime.Now.ToString()));
                    }
                    if (wizardData.Rows.Count > 0)
                    {
                        foreach (DataRow row in wizardData.Rows)
                        {
                            ArrayList MSCDataList = new ArrayList();

                            //No1. FileID
                            string FileID = row["FileID"].ToString();
                            MSCDataList.Add(FileID);
                            int Tier = 0;
                            if (FileID.Contains("ST"))
                                Tier = 4;
                            else
                                Tier = getTier(FileID);
                            //No2. Tier
                            MSCDataList.Add(Tier);
                            //No.3 Submit TYpe
                            string SubmitType = row["SubmitType"].ToString();
                            MSCDataList.Add(SubmitType);
                            //No. 4 CompName
                            string CompanyName = row["CompanyName"].ToString();
                            MSCDataList.Add(CompanyName);
                            //No.5 ROCNumber
                            string ROCNumber = row["ROCNumber"].ToString();
                            MSCDataList.Add(ROCNumber);
                            //No.6 Operational Status
                            string OperationalStatus = "";
                            if (row["OperationalStatus"].ToString().ToUpper().Contains("ACTIVE"))
                            {
                                OperationalStatus = "1";
                            }
                            else
                            {
                                OperationalStatus = "0";
                            }
                            MSCDataList.Add(OperationalStatus);
                            //No.7 Core Activities
                            string CoreActivities = row["CoreActivities"].ToString();
                            MSCDataList.Add(CoreActivities);
                            //No.8 URL
                            string URL = row["URL"].ToString();
                            MSCDataList.Add(URL);
                            //No.9 DateofIncorporation
                            string DateofIncorporation = row["ROCDate"].ToString();
                            MSCDataList.Add(DateofIncorporation);
                            //No.10 YearOfApproval
                            string YearOfApproval = row["Year"].ToString();
                            MSCDataList.Add(YearOfApproval);
                            //No.11 ApprovalLetterdate
                            string ApprovalLetterdate = row["ApprovalLetterdate"].ToString();
                            MSCDataList.Add(ApprovalLetterdate);
                            //No.12 ACMeetingDate
                            string ACMeetingDate = row["DateOfApproval"].ToString();
                            MSCDataList.Add(ACMeetingDate);
                            //No.13 FinancialIncentive
                            string FinancialIncentive = row["FinancialIncentive"].ToString();
                            MSCDataList.Add(FinancialIncentive);
                            //No.14 Cluster
                            string Cluster = row["MainCluster"].ToString();
                            MSCDataList.Add(Cluster);
                            //No.15 BusinessPhone
                            //+60; + 3; + 123086088;
                            string BusinessPhone = row["PhISDCode"].ToString().Replace(";", "") + row["PhCalAreaCode"].ToString().Replace(";", "") + row["PhoneNumber"].ToString().Replace(";", "");
                            MSCDataList.Add(BusinessPhone);
                            //No.16 Fax
                            string Fax = row["FaxISDCode"].ToString().Replace(";", "") + row["FaxCalAreaCode"].ToString().Replace(";", "") + row["Fax"].ToString().Replace(";", "");
                            MSCDataList.Add(Fax);
                            //No.17 BA
                            string BA = row["BusinestAnalyst"].ToString();
                            MSCDataList.Add(BA);
                            //No.18 Acc5YearsTax
                            Decimal? Acc5YearsTax = SyncHelper.ConvertToDecimal(row["CumulativeTaxLoss"].ToString());
                            MSCDataList.Add(Acc5YearsTax);
                            //No.19 MSCCertNo
                            string MSCCertNo = "";
                            string MSCFileID = "";
                            string Unfilter = FileID;
                            if (Unfilter.Contains("ST"))
                                MSCFileID = Unfilter.Replace("ST/3/", "");
                            if (Unfilter.Contains("CS"))
                                MSCFileID = Unfilter.Replace("CS/3/", "");
                            if (Unfilter.Contains("KWD"))
                                MSCFileID = Unfilter.Replace("KWD-4/", "");
                            if (Unfilter.Contains("MSC-INC"))
                                MSCFileID = Unfilter.Replace("MSC-INC/137/", "");
                            MSCCertNo = getMSCCertNoFromWizard(MSCFileID);
                            MSCDataList.Add(MSCCertNo);
                            //No.20 SubmitDate
                            string SubmitDate = getSubmitDatefromWizard(FileID);
                            MSCDataList.Add(SubmitDate);
                            Console.WriteLine(string.Format("[{0}] : Done get all paramerters", DateTime.Now.ToString()));
                            LogFileHelper.logList.Add(string.Format("[{0}] : Done get all paramerters", DateTime.Now.ToString()));
                            if (MSCDataList.Count > 0)
                            {
                                DateTime LastSync =Convert.ToDateTime(getLastSyncedDate());
                                if (!CheckRecordExist(FileID, SubmitType, LastSync))
                                {
                                    InsertIntoMOFMaklumatSyarikat(MSCDataList);
                                    Console.WriteLine(string.Format("[{0}] : successful stored in MOF_MaklumatSyarikat", DateTime.Now.ToString()));
                                    LogFileHelper.logList.Add(string.Format("[{0}] : successful stored in MOF_MaklumatSyarikat", DateTime.Now.ToString()));
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("[{0}] : record already stored, skip...", DateTime.Now.ToString()));
                                    LogFileHelper.logList.Add(string.Format("[{0}] : record already stored, skip...", DateTime.Now.ToString()));
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static int getTier(string MSCFileID)
        {
            int Tier = 0;
            string FileID = "";
            if (MSCFileID.Contains("CS"))
                FileID = MSCFileID.Replace("CS/3/", "");
            if (MSCFileID.Contains("KWD"))
                FileID = MSCFileID.Replace("KWD-4/", "");
            if (MSCFileID.Contains("MSC-INC"))
                FileID = MSCFileID.Replace("MSC-INC/137/", "");

            SqlConnection con = SQLHelper.GetConnectionMOF();
            SqlCommand com = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter(com);

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine("SELECT [Tier] FROM [etouch].[dbo].[prd_WIZTier]  where [RefNumber]='" + FileID + "'");
            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = con;
            com.CommandTimeout = int.MaxValue;

            try
            {
                DataTable dt = new DataTable();

                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                    Tier = Convert.ToInt32(dt.Rows[0][0].ToString());
                return Tier;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        private static string getLatestExcelFile()
        {
            string FileName = "";
            string folderPath = ConfigurationSettings.AppSettings["ExcelStartUpLocation"].ToString();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string ExcelFile = "";
            var directory = new DirectoryInfo(folderPath);
            var fileName = directory.GetFiles()
            .OrderByDescending(f => f.LastWriteTime)
            .First();
            if (fileName != null)
                FileName = ConfigurationSettings.AppSettings["ExcelStartUpLocation"].ToString() + fileName.ToString();
            return FileName;
        }
        private static string getSubmitDatefromWizard(string MSCFileID)
        {
            string SubmitDate = "";
            StringBuilder sql = new StringBuilder();
            SqlConnection con = SQLHelper.GetConnectionWizard();
            SqlCommand com = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter(com);
            string FileID = "";
            if (MSCFileID.Contains("CS") || MSCFileID.Contains("ST"))
            {
                FileID = MSCFileID.Replace("CS/3/", "");
                FileID = MSCFileID.Replace("ST/3/", "");
                sql.AppendLine("select b.submitdate from tbPreApplication b, tbappmain a where a.preappid = b.preappid and a.refnumber=" + FileID);

            }
            if (MSCFileID.Contains("KWD"))
            {
                FileID = MSCFileID.Replace("KWD-4/", "");
                sql.AppendLine("select b.submitdate from tbIHLPreAppMain b, tbIHLAppMain a where a.preappid = b.preappid  and a.refnumber=" + FileID);

            }
            if (MSCFileID.Contains("MSC-INC"))
            {
                FileID = MSCFileID.Replace("MSC-INC/137/", "");
                sql.AppendLine("select b.submitdate from tbINCPreAppMain b, tbINCAppMain a where a.preappid = b.preappid  and a.refnumber=" + FileID);

            }
            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = con;
            com.CommandTimeout = int.MaxValue;

            try
            {
                DataTable dt = new DataTable();

                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                    SubmitDate = dt.Rows[0]["submitdate"].ToString();
                return SubmitDate;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        private static string getMSCCertNoFromWizard(string RefNumber)
        {
            string MSCCertNo = "";
            SqlConnection con = SQLHelper.GetConnectionWizard();
            SqlCommand com = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter(com);

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine("Select MSCCertNo from tbfsMainDtls Where RefNumber=" + RefNumber);
            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = con;
            com.CommandTimeout = int.MaxValue;

            try
            {
                DataTable dt = new DataTable();

                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                    MSCCertNo = dt.Rows[0]["MSCCertNo"].ToString();
                return MSCCertNo;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        private static bool CheckRecordExist(string fileID, string submitType, DateTime SyncDate)
        {
            //2017-02-12
            string sSyncDate = SyncDate.ToString("yyyy-MM-dd");
            bool existed = false;
            SqlConnection con = SQLHelper.GetConnectionMOF();
            SqlCommand com = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter(com);

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine("Select TOP 1 MSCFileID from MOF_MaklumatSyarikat1 where MSCFileID='" + fileID + "' AND SyncDate='" + sSyncDate + "' AND SubmitType='" + submitType + "'");
            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = con;
            com.CommandTimeout = int.MaxValue;

            try
            {
                DataTable dt = new DataTable();

                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                    existed = true;
                return existed;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        private static void InsertIntoMOFMaklumatSyarikat(ArrayList MSCDataList)
        {
            //No1. FileID
            //No2. Tier
            //No.3 Submit TYpe
            //No. 4 CompName
            //No.5 ROCNumber
            //No.6 Operational Status
            //No.7 Core Activities
            //No.8 URL
            //No.9 DateofIncorporation
            //No.10 YearOfApproval
            //No.11 ApprovalLetterdate
            //No.12 ACMeetingDate
            //No.13 FinancialIncentive
            //No.14 Cluster
            //No.15 BusinessPhone
            //No.16 Fax
            //No.17 BA
            //No.18 Acc5YearsTax
            //No.19 MSCCertNo
            //No.20 SubmitDate
            SqlConnection Connection = SQLHelper.GetConnectionMOF();
            SqlCommand com = new SqlCommand();
            System.Text.StringBuilder sql = new System.Text.StringBuilder();

            sql.AppendLine("INSERT INTO MOF_MaklumatSyarikat1 ");
            sql.AppendLine("(");
            sql.AppendLine("[CompanyName],[MSCFileID],[CoreActivities],[ROCNumber],[URL],[ACMeetingDate],[ApprovalLetterDate]");
            sql.AppendLine(",[YearOfApproval],[DateofIncorporation],[OperationalStatus],[FinancialIncentive],[Cluster],[BusinessPhone]");
            sql.AppendLine(",[Fax],[TaxRevenueLoss],[Tier],[SubmitDate],[MSCCertNo],[BA],[SubmitType],[SyncDate] ");
            sql.AppendLine(")");
            sql.AppendLine("VALUES(");
            sql.AppendLine("@CompanyName, @MSCFileID, @CoreActivities, @ROCNumber, @URL, @ACMeetingDate, @ApprovalLetterDate, ");
            sql.AppendLine("@YearOfApproval, @DateofIncorporation, @OperationalStatus, @FinancialIncentive, @Cluster, @BusinessPhone,");
            sql.AppendLine("@Fax, @TaxRevenueLoss, @Tier, @SubmitDate, @MSCCertNo, @BA, @SubmitType,@SyncDate");
            sql.AppendLine(")");

            com.Parameters.Add(new SqlParameter("@CompanyName", MSCDataList[3].ToString()));
            com.Parameters.Add(new SqlParameter("@MSCFileID", MSCDataList[0].ToString()));
            com.Parameters.Add(new SqlParameter("@CoreActivities", MSCDataList[6].ToString()));
            com.Parameters.Add(new SqlParameter("@ROCNumber", MSCDataList[4].ToString()));
            com.Parameters.Add(new SqlParameter("@URL", MSCDataList[7].ToString()));
            com.Parameters.Add(new SqlParameter("@ACMeetingDate", MSCDataList[11].ToString()));
            com.Parameters.Add(new SqlParameter("@ApprovalLetterDate", MSCDataList[10].ToString()));
            com.Parameters.Add(new SqlParameter("@YearOfApproval", MSCDataList[9].ToString()));
            com.Parameters.Add(new SqlParameter("@DateofIncorporation", MSCDataList[8].ToString()));
            com.Parameters.Add(new SqlParameter("@OperationalStatus", MSCDataList[5].ToString()));
            com.Parameters.Add(new SqlParameter("@FinancialIncentive", MSCDataList[12].ToString()));
            com.Parameters.Add(new SqlParameter("@Cluster", MSCDataList[13].ToString()));
            com.Parameters.Add(new SqlParameter("@BusinessPhone", MSCDataList[14].ToString()));
            com.Parameters.Add(new SqlParameter("@Fax", MSCDataList[15].ToString()));
            if (MSCDataList[17] != null)
                com.Parameters.Add(new SqlParameter("@TaxRevenueLoss", MSCDataList[17].ToString()));
            else
                com.Parameters.Add(new SqlParameter("@TaxRevenueLoss", DBNull.Value));
            com.Parameters.Add(new SqlParameter("@Tier", MSCDataList[1].ToString()));

            com.Parameters.Add(new SqlParameter("@SubmitDate", MSCDataList[19].ToString()));
            com.Parameters.Add(new SqlParameter("@MSCCertNo", MSCDataList[18].ToString()));
            com.Parameters.Add(new SqlParameter("@BA", MSCDataList[16].ToString()));
            com.Parameters.Add(new SqlParameter("@SubmitType", MSCDataList[2].ToString()));
            com.Parameters.Add(new SqlParameter("@SyncDate", DateTime.Now));

            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = Connection;
            com.CommandTimeout = int.MaxValue;

            try
            {
                //con.Open()
                com.ExecuteNonQuery();
            }
            catch
            {

            }


        }

        private static DataTable SelectACApprovedAccountList()
        {
            SqlConnection con = SQLHelper.GetConnection();
            SqlCommand com = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter(com);
            string LastSync = getLastSyncedDate();
            StringBuilder sql = new System.Text.StringBuilder();
            //sql.AppendLine(ConfigurationSettings.AppSettings["WizardStoredProc"].ToString()).Append(" '").Append(LastSync).Append("'");
            sql.AppendLine(ConfigurationSettings.AppSettings["WizardStoredProc"].ToString()).Append(" '").Append("2017-02-14 00:00:00 AM").Append("'");

            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = con;
            com.CommandTimeout = int.MaxValue;

            try
            {
                DataTable dt = new DataTable();

                ad.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        private static string getLastSyncedDate()
        {
            string LastSyncedDate = "";
            SqlConnection con = SQLHelper.GetConnection();
            SqlCommand com = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter(com);
            StringBuilder sql = new System.Text.StringBuilder();
            sql.AppendLine("SELECT[ParamValue]  FROM[CRM_PRD].[dbo].[Parameter]  where ParamCode = 'WIZARD_TMS' ");
            com.CommandText = sql.ToString();
            com.CommandType = CommandType.Text;
            com.Connection = con;
            com.CommandTimeout = int.MaxValue;

            try
            {
                DataTable dt = new DataTable();

                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                    LastSyncedDate = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return LastSyncedDate;
        }
        #endregion
    }
}