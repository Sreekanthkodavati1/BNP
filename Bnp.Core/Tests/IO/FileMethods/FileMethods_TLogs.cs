using System;
using System.IO;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using BnPBaseFramework.IO.FileFunctions_Common;
using Npgsql;
using TemplateItems;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Bnp.Core.Tests.IO.FileMethods
{
    public class FileMethods_TLogs
    {
        public static int lineNum = 0;
        public static string SELQuery = null;
        public static string WhereClause = null;
        public static string libJobQuery = null;
        public static string libMsgQuery = null;
        public static string OracleReason = null;
        public static string JobNumber = null;
        public static string Reason = null;
        public static string filePath = null;
        public static OracleDataReader ORCDR = null;
        public static NpgsqlDataReader NpgsqlDR = null;
        public static string Columns = null;
        public static string INSQueryPositive = null;
        public static string LibJobTbl = ConfigurationManager.AppSettings["LibJob"];
        public static string LibmsglogTbl = ConfigurationManager.AppSettings["LibMSGLog"];
        public static string QADBSchma = ConfigurationManager.AppSettings["QADBSchema"];
        public static string TLogFilePath = ConfigurationManager.AppSettings["TLogFilePth"];
        public static string UserName = ConfigurationManager.AppSettings["UsrName"];
        public static string TxnHeaderTbl = ConfigurationManager.AppSettings["TLogHeadrTable"];
        public static string TxndetailItmTbl = ConfigurationManager.AppSettings["TLogLineItmTable"];
        public static string LoyalityIDError = "Empty Member/VirtualCard/LoyaltyIdNumber in input Xml for Member lookup.";
        public static string TxnIDError = "ORA-01400: cannot insert NULL into (\"" + UserName + "\"" + ".\"" + TxnHeaderTbl + "\"" + "." + "\"" + "A_TXNHEADERID\")";
        public static string RegisterIDError = "ORA-01400: cannot insert NULL into (\"" + UserName + "\"" + ".\"" + TxnHeaderTbl + "\"" + "." + "\"" + "A_TXNREGISTERNUMBER\")";
        public static string TxnDetailIdError = "ORA-01400: cannot insert NULL into (\"" + UserName + "\"" + ".\"" + TxndetailItmTbl + "\"" + "." + "\"" + "A_TXNDETAILID\")";
        public static string TotalTxnAmtError = "Inner exception for accrual factor error: Object reference not set to an instance of an object.";
        public static string MandatoryAttributesPresent = "No Empty string";
        public static string PLSQLresults = null;
        public static string Oracleresults = null;
        public static List<string> records = null;
        public static List<string> Records = null;
        public static Dictionary<string, string> OracleTxnHeaderResult = null;
        public static Dictionary<string, string> OracleTxnDtlItmResult = null;
        public static Dictionary<string, string> txnAmount = null;
        public static Dictionary<string, string> lyltyID = null;
        public static Dictionary<string, string> PostgressResult = null;
        public static Dictionary<string, string> OracleResult = null;
        public static Dictionary<string, string> PLSQLTxnHeaderResults = null;
        public static Dictionary<string, string> PLSQLTxnDtlItmResults = null;
        public static Dictionary<string, string>[] postgressValuesArray = null;
        public static Dictionary<string, string>[] oracleValuesArray = null;
        public static Dictionary<string, string>[] txnamtAndLoyaltyIdValues = null;
        public static ArrayList headerPassed = null;
        public static ArrayList detailPassed = null;
        public static ArrayList errorPassed = null;
        public static ArrayList headerFailed = null;
        public static ArrayList detailFailed = null;
        public static ArrayList errorFailed = null;
        public static bool recordPassed = false;
        public static bool recordFailed = true;

        public static int totalCount = 0;

        public static Dictionary<string, string> posgtressHeader = null;
        public static Dictionary<string, string> postgressDetail = null;
        public static Dictionary<string, string> postgressError = null;
        public static Dictionary<string, string> oracleHeader = null;
        public static Dictionary<string, string> oracleDetail = null;
        public static Dictionary<string, string> oracleError = null;
        public static Dictionary<string, string> values7 = null;
        public static Dictionary<string, string> values8 = null;
        public static bool result = false;

        public static string ErrorPrefix = "This error message is from lw_libmessagelog table. ";
        public static string CorrectResultPrefixTxHeadr = "This result set is from ats_txnheader table. ";
        public static string CorrectResultPrefixTxDetailItm = "This result set is from ats_txndetailitem table. ";
        public static int noOfRecords = 0;
        public static string PostgressPtsResult = null;
        public static string OraclePtsResult = null;

        public string ReadFromOracle(string TableName, string WhereClause)
        {
            try
            {
                SELQuery = "SELECT * FROM " + TableName + " WHERE " + WhereClause;
                ORCDR = DataReader.OracleDataReader(SELQuery);
                if (ORCDR.HasRows && (TableName.Equals("ATS_TXNHEADER")))
                {
                    while (ORCDR.Read())
                    {
                        Oracleresults = ORCDR[3].ToString() + ORCDR[13].ToString() + ORCDR[12].ToString() + ORCDR[10].ToString() + ORCDR[15].ToString();
                    }

                }

                SELQuery = "SELECT * FROM " + TableName + " WHERE " + WhereClause;
                ORCDR = DataReader.OracleDataReader(SELQuery);
                if (ORCDR.HasRows && TableName == "ATS_TXNDETAILITEM")
                {
                    while (ORCDR.Read())
                    {
                        Oracleresults = ORCDR[6].ToString() + ORCDR[7].ToString() + ORCDR[13].ToString() + ORCDR[15].ToString() + ORCDR[14].ToString();
                    }
                }
                ORCDR.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Oracleresults;
        }

        public string ReadReasonFromOracle(string OracleReason)
        {
            try
            {
                libJobQuery = "SELECT JOBNUMBER FROM " + LibJobTbl + " WHERE ROWNUM = 1 ORDER BY ID DESC";
                ORCDR = DataReader.OracleDataReader(libJobQuery);
                if (ORCDR.HasRows)
                {
                    while (ORCDR.Read())
                    {
                        JobNumber = ORCDR[0].ToString();
                    }
                    ORCDR.Close();
                }

                libMsgQuery = " SELECT REASON FROM " + LibmsglogTbl + " WHERE JOBNUMBER = " + JobNumber + "AND REASON = '" + OracleReason + "'";
                ORCDR = DataReader.OracleDataReader(libMsgQuery);
                if (ORCDR.HasRows)
                {
                    while (ORCDR.Read())
                    {
                        Reason = ORCDR[0].ToString();
                    }
                    ORCDR.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Reason;
        }

        public string WriteToPLSQL(string InsQuery)
        {
            try
            {
                DataReader.PLSQLDataReader(InsQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return InsQuery;
        }

        public string ReadFromPostgress(string TableName, string WhereClause)
        {
            try
            {
                SELQuery = "SELECT * FROM " + QADBSchma + "." + TableName + " WHERE " + WhereClause;
                NpgsqlDR = DataReader.PLSQLDataReader2(SELQuery);
                if (NpgsqlDR.HasRows && TableName == TxnHeaderTbl)
                {
                    while (NpgsqlDR.Read())
                    {
                        PLSQLresults = NpgsqlDR[3].ToString() + NpgsqlDR[13].ToString() + NpgsqlDR[12].ToString() + NpgsqlDR[10].ToString() + NpgsqlDR[15].ToString();
                    }

                }
                if (NpgsqlDR.HasRows && TableName == TxndetailItmTbl)
                {
                    while (NpgsqlDR.Read())
                    {
                        PLSQLresults = NpgsqlDR[6].ToString() + NpgsqlDR[7].ToString() + NpgsqlDR[13].ToString() + NpgsqlDR[15].ToString() + NpgsqlDR[14].ToString();
                    }
                }
                NpgsqlDR.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return PLSQLresults;
        }

        public Dictionary<string, string>[] ReadValuesFromPostgress(string fileName)
        {
            lineNum = 0;

            Dictionary<string, string> PGTxnHeaderresults = new Dictionary<string, string>();
            Dictionary<string, string> PGTxnDetailresults = new Dictionary<string, string>();
            Dictionary<string, string> Errorresults1 = new Dictionary<string, string>();

            Dictionary<string, string> OracleTxnHeaderresults = new Dictionary<string, string>();
            Dictionary<string, string> OracleTxnDetailresults = new Dictionary<string, string>();
            Dictionary<string, string> Errorresults2 = new Dictionary<string, string>();
            Dictionary<string, string> LyltyId = new Dictionary<string, string>();
            Dictionary<string, string> TxnAmt = new Dictionary<string, string>();


            try
            {
                filePath = TLogFilePath + "\\" + fileName;
                foreach (string line in File.ReadLines(filePath))
                {
                    string value = line.Replace('|', ',');

                    string[] record = value.ToString().Split(',');
                    if (record.Length == 15)
                    {
                        string Header = record[0].ToString();
                        string LoyaltyID = record[1].ToString();
                        string TxnID = record[2].ToString();
                        string RegisterID = record[5].ToString();
                        string TotalTxnAmt = record[9].ToString();
                        string TxnDetailId = record[10].ToString();
                        string BarCode = record[7].ToString();
                        string StoreNumber = record[4].ToString();
                        string TxnDate = record[3].ToString();
                        string LineItmAmount = record[14].ToString();
                        string DtlProductId = record[11].ToString();
                        string UnitPrice = record[12].ToString();
                        string Quantity = record[13].ToString();


                        string error = LoyaltyID == "" ? LoyalityIDError : TxnID == "" ? TxnIDError : RegisterID == "" ? RegisterIDError : TotalTxnAmt == "" ? TotalTxnAmtError : TxnDetailId == "" ? TxnDetailIdError : MandatoryAttributesPresent;

                        //lineNum = 0;
                        if (string.Equals(error, LoyalityIDError))
                        {
                            //Postgress results
                            string INSQueryLoyaltyIDError = "INSERT INTO " + QADBSchma + "." + LibmsglogTbl + " VALUES(" + "'" + LoyalityIDError + "'" + ")";
                            WriteToPLSQL(INSQueryLoyaltyIDError);
                            PLSQLresults = ErrorPrefix + LoyalityIDError;
                            Errorresults1.Add(lineNum.ToString(), PLSQLresults + "-Line: " + lineNum);

                            //Oracle results
                            OracleReason = LoyalityIDError;
                            Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
                            Errorresults2.Add(lineNum.ToString(), Oracleresults + "-Line: " + lineNum);

                            lineNum++;


                        }
                        if (string.Equals(error, TxnIDError))
                        {
                            //Postgress results
                            string INSQueryTxnIDError = "INSERT INTO " + QADBSchma + "." + LibmsglogTbl + " VALUES(" + "'" + TxnIDError + "'" + ")";
                            WriteToPLSQL(INSQueryTxnIDError);
                            PLSQLresults = ErrorPrefix + TxnIDError;
                            Errorresults1.Add(lineNum.ToString(), PLSQLresults + "-Line: " + lineNum);

                            //Oracle Results
                            OracleReason = TxnIDError;
                            Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
                            Errorresults2.Add(lineNum.ToString(), Oracleresults + "-Line: " + lineNum);

                            lineNum++;

                        }
                        if (string.Equals(error, RegisterIDError))
                        {
                            //Postgress results
                            string INSQueryRegisterIDError = "INSERT INTO " + QADBSchma + "." + LibmsglogTbl + " VALUES(" + "'" + RegisterIDError + "'" + ")";
                            WriteToPLSQL(INSQueryRegisterIDError);
                            PLSQLresults = ErrorPrefix + RegisterIDError;
                            Errorresults1.Add(lineNum.ToString(), PLSQLresults + "-Line: " + lineNum);

                            //Oracle results
                            OracleReason = RegisterIDError;
                            Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
                            Errorresults2.Add(lineNum.ToString(), Oracleresults + "-Line: " + lineNum);

                            lineNum++;


                        }
                        if (string.Equals(error, TotalTxnAmtError))
                        {
                            //Postgress results
                            string INSQueryTotalTxnAmtError = "INSERT INTO " + QADBSchma + "." + LibmsglogTbl + " VALUES(" + "'" + TotalTxnAmtError + "'" + ")";
                            WriteToPLSQL(INSQueryTotalTxnAmtError);
                            PLSQLresults = ErrorPrefix + TotalTxnAmtError;
                            Errorresults1.Add(lineNum.ToString(), PLSQLresults + "-Line: " + lineNum);

                            //Oracle results
                            OracleReason = TotalTxnAmtError;
                            Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
                            Errorresults2.Add(lineNum.ToString(), Oracleresults + "-Line: " + lineNum);

                            lineNum++;

                        }

                        if (string.Equals(error, TxnDetailIdError))
                        {
                            //Postgress results
                            string INSQueryTxnDetailIdError = "INSERT INTO " + QADBSchma + "." + LibmsglogTbl + " VALUES(" + "'" + TxnDetailIdError + "'" + ")";
                            WriteToPLSQL(INSQueryTxnDetailIdError);
                            PLSQLresults = ErrorPrefix + TxnDetailIdError;
                            Errorresults1.Add(lineNum.ToString(), PLSQLresults + "-Line: " + lineNum);

                            //Oracle Results
                            OracleReason = TxnDetailIdError;
                            Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
                            Errorresults2.Add(lineNum.ToString(), Oracleresults + "-Line: " + lineNum);

                            lineNum++;

                        }

                        if (string.Equals(error, MandatoryAttributesPresent))
                        {
                            //PostGress Header Results
                            string values = "";
                            Columns = "(" + "\"A_TXNHEADERID\"" + "," + "\"A_TXNREGISTERNUMBER\"" + "," + "\"A_TXNAMOUNT\"" + "," + "\"A_TXNDATE\"" + "," + "\"A_TXNNUMBER\"" + "," + "\"A_TXNSTOREID\"" + ")";
                            value = TxnID + "," + RegisterID + "," + TotalTxnAmt + "," + TxnDate + "," + BarCode + "," + StoreNumber;
                            values = "'" + value.Replace(",", "','") + "'";

                            INSQueryPositive = "INSERT INTO " + QADBSchma + "." + TxnHeaderTbl + Columns + " VALUES" + "(" + values + ")";

                            WriteToPLSQL(INSQueryPositive);

                            WhereClause = "\"A_TXNHEADERID\"" + "=" + "'" + record[2].ToString() + "'" + " AND " + "\"A_TXNREGISTERNUMBER\"" + "=" + "'" + record[5].ToString() + "'" + " AND " + "\"A_TXNAMOUNT\"" + "=" + "'" + record[9].ToString() + "'";


                            PLSQLresults = CorrectResultPrefixTxHeadr + ReadFromPostgress(TxnHeaderTbl, WhereClause);

                            PGTxnHeaderresults.Add(lineNum.ToString(), PLSQLresults + "-Line: " + lineNum);


                            //Oracle Header Results
                            WhereClause = "A_TXNHEADERID=" + "'" + record[2].ToString() + "'" + " AND " + "A_TXNREGISTERNUMBER=" + "'" + record[5].ToString() + "'" + " AND " + "A_TXNAMOUNT=" + "'" + record[9].ToString() + "'";

                            string TxnHeadr_Oracleresults = CorrectResultPrefixTxHeadr + ReadFromOracle(TxnHeaderTbl, WhereClause);

                            OracleTxnHeaderresults.Add(lineNum.ToString(), TxnHeadr_Oracleresults + "-Line: " + lineNum);


                            //Postgress Detail Results
                            Columns = "(" + "\"A_TXNDETAILID\"" + "," + "\"A_DTLPRODUCTID\"" + "," + "\"A_DTLRETAILAMOUNT\"" + "," + "\"A_DTLSALEAMOUNT\"" + "," + "\"A_DTLQUANTITY\"" + ")";
                            value = TxnDetailId + "," + DtlProductId + "," + UnitPrice + "," + LineItmAmount + "," + Quantity;
                            values = "'" + value.Replace(",", "','") + "'";
                            INSQueryPositive = "INSERT INTO " + QADBSchma + "." + TxndetailItmTbl + Columns + " VALUES" + "(" + values + ")";
                            WriteToPLSQL(INSQueryPositive);

                            WhereClause = "\"A_TXNDETAILID\"" + "=" + "'" + record[10].ToString() + "'" + " AND " + "\"A_DTLRETAILAMOUNT\"" + "=" + "'" + record[12].ToString() + "'" + " AND " + "\"A_DTLSALEAMOUNT\"" + "=" + "'" + record[14].ToString() + "'";

                            PLSQLresults = CorrectResultPrefixTxDetailItm + ReadFromPostgress(TxndetailItmTbl, WhereClause);
                            PGTxnDetailresults.Add(lineNum.ToString(), PLSQLresults + "-Line: " + lineNum);


                            //Oracle Detail Results
                            WhereClause = "A_TXNDETAILID=" + "'" + record[10].ToString() + "'" + " AND " + "A_DTLRETAILAMOUNT=" + "'" + record[12].ToString() + "'" + " AND " + "A_DTLSALEAMOUNT=" + "'" + record[14].ToString() + "'";
                            string TxnDetail_Oracleresults = CorrectResultPrefixTxDetailItm + ReadFromOracle(TxndetailItmTbl, WhereClause);

                            OracleTxnDetailresults.Add(lineNum.ToString(), TxnDetail_Oracleresults + "-Line: " + lineNum);

                            //LoyaltyID and TxnAmount
                            LyltyId.Add(lineNum.ToString(), LoyaltyID);
                            TxnAmt.Add(lineNum.ToString(), TotalTxnAmt);

                            lineNum++;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new[] { PGTxnHeaderresults, PGTxnDetailresults, Errorresults1, OracleTxnHeaderresults, OracleTxnDetailresults, Errorresults2, TxnAmt, LyltyId };
        }

        //public static Dictionary<string, string>[] ReadValuesFromOracle(string fileName)
        //{

        //   linenum = 0;
        //    Dictionary<string, string> OracleTxnHeaderresults = new Dictionary<string, string>();
        //    Dictionary<string, string> OracleTxnDetailresults = new Dictionary<string, string>();
        //    Dictionary<string, string> Errorresults2 = new Dictionary<string, string>();
        //    Dictionary<string, string> LyltyId = new Dictionary<string, string>();
        //    Dictionary<string, string> TxnAmt = new Dictionary<string, string>();

        //    try
        //    {
        //        filePath = TLogFilePath + "\\" + fileName;

        //        foreach (string line in File.ReadLines(filePath))
        //        {
        //            string value = line.Replace('|', ',');

        //            string[] record = value.ToString().Split(',');
        //            if (record.Length == 15)
        //            {
        //                string Header = record[0].ToString();
        //                string LoyaltyID = record[1].ToString();
        //                string TxnID = record[2].ToString();
        //                string RegisterID = record[5].ToString();
        //                string TotalTxnAmt = record[9].ToString();
        //                string TxnDetailId = record[10].ToString();
        //                string DtlProductId = record[11].ToString();
        //                string UnitPrice = record[12].ToString();
        //                string Quantity = record[13].ToString();
        //                string LineItmAmount = record[14].ToString();
        //                string BarCode = record[7].ToString();
        //                string StoreNumber = record[4].ToString();
        //                string TxnDate = record[3].ToString();


        //                string error = LoyaltyID == "" ? LoyalityIDError : TxnID == "" ? TxnIDError : RegisterID == "" ? RegisterIDError : TotalTxnAmt == "" ? TotalTxnAmtError : TxnDetailId == "" ? TxnDetailIdError : MandatoryAttributesPresent;


        //                if (string.Equals(error, LoyalityIDError))
        //                {

        //                    OracleReason = LoyalityIDError;
        //                    Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
        //                    Errorresults2.Add(linenum.ToString(), Oracleresults + "-Line: " + linenum);
        //                    linenum++;

        //                }

        //                if (string.Equals(error, TxnIDError))
        //                {

        //                    OracleReason = TxnIDError;
        //                    Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
        //                    Errorresults2.Add(linenum.ToString(), Oracleresults + "-Line: " + linenum);
        //                    linenum++;
        //                }

        //                if (string.Equals(error, RegisterIDError))
        //                {

        //                    OracleReason = RegisterIDError;
        //                    Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
        //                    Errorresults2.Add(linenum.ToString(), Oracleresults + "-Line: " + linenum);
        //                   linenum++;
        //                }

        //                if (string.Equals(error, TotalTxnAmtError))
        //                {

        //                    OracleReason = TotalTxnAmtError;
        //                    Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
        //                    Errorresults2.Add(linenum.ToString(), Oracleresults + "-Line: " + linenum);
        //                    linenum++;
        //                }

        //                if (string.Equals(error, TxnDetailIdError))
        //                {

        //                    OracleReason = TxnDetailIdError;
        //                    Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);
        //                    Errorresults2.Add(linenum.ToString(), Oracleresults + "-Line: " + linenum);
        //                   linenum++;
        //                }

        //                if (string.Equals(error, MandatoryAttributesPresent))
        //                {


        //                    WhereClause = "A_TXNHEADERID=" + "'" + record[2].ToString() + "'" + " AND " + "A_TXNREGISTERNUMBER=" + "'" + record[5].ToString() + "'" + " AND " + "A_TXNAMOUNT=" + "'" + record[9].ToString() + "'";

        //                    string TxnHeadr_Oracleresults = CorrectResultPrefixTxHeadr + ReadFromOracle(TxnHeaderTbl, WhereClause);

        //                    OracleTxnHeaderresults.Add(linenum.ToString(), TxnHeadr_Oracleresults + "-Line: " + linenum);

        //                    WhereClause = "A_TXNDETAILID=" + "'" + record[10].ToString() + "'" + " AND " + "A_DTLRETAILAMOUNT=" + "'" + record[12].ToString() + "'" + " AND " + "A_DTLSALEAMOUNT=" + "'" + record[14].ToString() + "'";
        //                    string TxnDetail_Oracleresults = CorrectResultPrefixTxDetailItm + ReadFromOracle(TxndetailItmTbl, WhereClause);

        //                    OracleTxnDetailresults.Add(linenum.ToString(), TxnDetail_Oracleresults + "-Line: " + linenum);
        //                    LyltyId.Add(linenum.ToString(), LoyaltyID);
        //                    TxnAmt.Add(linenum.ToString(), TotalTxnAmt);

        //                  linenum++;
        //                }

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    return new[] { OracleTxnHeaderresults, OracleTxnDetailresults, Errorresults2, TxnAmt, LyltyId };
        //}

        public string[] WriteToFile(string filename)
        {
            int totalPassedCount = 0;
            int totalFailedCount = 0;
            int headerPassCount = 0;
            int detailPassCount = 0;
            int errorPassCount = 0;
            int headerFailCount = 0;
            int detailFailCount = 0;
            int errorFailcount = 0;

            bool result = true;
            ArrayList headerPassed = new ArrayList();
            ArrayList detailPassed = new ArrayList();
            ArrayList errorPassed = new ArrayList();
            ArrayList headerFailed = new ArrayList();
            ArrayList detailFailed = new ArrayList();
            ArrayList errorFailed = new ArrayList();

            FileMethods_TLogs fileMethodsTlogs = new FileMethods_TLogs();
            Dictionary<string, string>[] postgressValuesArray = fileMethodsTlogs.ReadValuesFromPostgress(filename);
            // Dictionary<string, string>[] oracleValuesArray = FileMethods_TLogs.ReadValuesFromOracle(filename);


            Dictionary<string, string> postgressHeader = postgressValuesArray[0];
            Dictionary<string, string> postgressDetail = postgressValuesArray[1];
            Dictionary<string, string> postgressError = postgressValuesArray[2];

            Dictionary<string, string> oracleHeader = postgressValuesArray[3];
            Dictionary<string, string> oracleDetail = postgressValuesArray[4];
            Dictionary<string, string> oracleError = postgressValuesArray[5];


            Dictionary<string, string> txnAmountValues = postgressValuesArray[6];
            Dictionary<string, string> loyaltyIDValues = postgressValuesArray[7];

            string errorLogPath = TLogFilePath.ToString().Replace("InputTextFiles", "ErrorLogs") + @"\";

            string headerFilePath = errorLogPath + @"TxnHeader_Failed_" + DateTime.UtcNow.ToString("MMddyy_HHmmss") + ".txt";
            string detailFilePath = errorLogPath + @"TxnDetailItem_Failed_" + DateTime.UtcNow.ToString("MMddyy_HHmmss") + ".txt";
            string errorFilePath = errorLogPath + @"LibMsgLog_Failed_" + DateTime.UtcNow.ToString("MMddyy_HHmmss") + ".txt";


            if (postgressHeader.Count != 0 && oracleHeader.Count != 0)
            {

                totalCount = postgressHeader.Count;
                for (int i = 0; i < totalCount; i++)
                {
                    if (postgressHeader.Values.ElementAt(i).Equals(oracleHeader.Values.ElementAt(i)))
                    {
                        recordPassed = false;
                        Console.WriteLine("Postgress and Oracle Header results are matching : see below");
                        Console.WriteLine("This resultset is from Postgress DB:{0}", postgressHeader.Values.ElementAt(i));
                        Console.WriteLine("This resultset is from Oracle DB:{0}", oracleHeader.Values.ElementAt(i));
                        headerPassCount++;


                    }
                    else
                    {


                        headerFailCount++;

                        if (postgressHeader.Values.ElementAt(i).ToLower().Contains("ats_txnheader"))
                        {
                            if (!File.Exists(headerFilePath))
                                File.CreateText(headerFilePath).Close();

                            string content = "Status:FAILED" + "|expected: " + postgressHeader.Values.ElementAt(i) + " |actual: " + oracleHeader.Values.ElementAt(i);

                            File.AppendAllText(headerFilePath, content + Environment.NewLine);
                            Console.WriteLine("Postgress and Oracle Error results are not matching. For further info check the file at :{0}", errorLogPath + @"TxnDetailItem_Failed.txt");
                        }
                    }

                }
            }

            if (postgressDetail.Count != 0 && oracleDetail.Count != 0)
            {
                totalCount = postgressDetail.Count;
                for (int i = 0; i < totalCount; i++)
                {
                    if (postgressDetail.Values.ElementAt(i).Equals(oracleDetail.Values.ElementAt(i)))
                    {

                        Console.WriteLine("Postgress and Oracle Detail results are matching : see below");
                        Console.WriteLine("This resultset is from Postgress DB:{0}", postgressDetail.Values.ElementAt(i));
                        Console.WriteLine("This resultset is from Oracle DB:{0}", oracleDetail.Values.ElementAt(i));
                        detailPassCount++;


                    }
                    else
                    {
                        detailFailCount++;

                        if (postgressDetail.Values.ElementAt(i).ToLower().Contains("ats_txndetailitem"))
                        {
                            if (!File.Exists(detailFilePath))
                                File.CreateText(detailFilePath).Close();

                            string content = "Status:FAILED" + "|expected: " + postgressDetail.Values.ElementAt(i) + " |actual: " + oracleDetail.Values.ElementAt(i);

                            File.AppendAllText(detailFilePath, content + Environment.NewLine);
                            Console.WriteLine("Postgress and Oracle Error results are not matching. For further info check the file at :{0}", errorLogPath + @"TxnDetailItem_Failed.txt");
                        }
                    }

                }
            }


            if (postgressError.Count != 0 && oracleError.Count != 0)
            {
                totalCount = postgressError.Count;
                for (int i = 0; i < totalCount; i++)
                {
                    if (postgressError.Values.ElementAt(i).Equals(oracleError.Values.ElementAt(i)))
                    {

                        Console.WriteLine("Postgress and Oracle Error results are matching : see below");
                        Console.WriteLine("This resultset is from Postgress DB:{0}", postgressError.Values.ElementAt(i));
                        Console.WriteLine("This resultset is from Oracle DB:{0}", oracleError.Values.ElementAt(i));
                        errorPassCount++;

                    }
                    else
                    {

                        errorFailcount++;

                        if (postgressError.Values.ElementAt(i).ToLower().Contains("lw_libmessagelog"))
                        {
                            if (!File.Exists(errorFilePath))
                                File.CreateText(errorFilePath).Close();

                            string content = "Status:FAILED" + "|expected: " + postgressError.Values.ElementAt(i) + " |actual: " + oracleError.Values.ElementAt(i);

                            File.AppendAllText(errorFilePath, content + Environment.NewLine);
                            Console.WriteLine("Postgress and Oracle Error results are not matching. For further info check the file at :{0}", errorLogPath + @"LibMsgLog_Failed.txt");
                        }
                    }

                }
            }


            string[] pointsResults = fileMethodsTlogs.Rule_TierPoints_DatabseComparison(txnAmountValues, loyaltyIDValues);

            if (headerFailCount > 0 || detailFailCount > 0 || errorFailcount > 0)
            {
                result = false;
                Console.WriteLine("Test flow failed  without points validation for an input record");
            }
            else
            {
                result = true;
                Console.WriteLine("Test flow passed  without points validation for an input record");
            }
            totalPassedCount = headerPassCount + detailPassCount + errorPassCount;
            totalFailedCount = headerFailCount + detailFailCount + errorFailcount;


            return new[] { result.ToString(), totalPassedCount.ToString(), totalFailedCount.ToString(), pointsResults[0], pointsResults[1], pointsResults[2] };
        }

        public string[] Rule_TierPoints_DatabseComparison(Dictionary<string, string> txnAmountValues, Dictionary<string, string> loyaltyIDValues)
        {
            int pointsPassedCount = 0;
            int pointsFailedCount = 0;
            bool pointsResult = false;

            string errorLogPath = TLogFilePath.ToString().Replace("InputTextFiles", "ErrorLogs") + @"\";

            string pointsFilePath = errorLogPath + @"Points_Failed_" + DateTime.UtcNow.ToString("MMddyy_HHmmss") + ".txt";

            if (txnAmountValues.Count != 0 && loyaltyIDValues.Count != 0 && txnAmountValues.Count == loyaltyIDValues.Count)
                for (int i = 0; i < txnAmountValues.Count; i++)
                {
                    string txnAmount = txnAmountValues.Values.ElementAt(i);
                    string lyltyID = loyaltyIDValues.Values.ElementAt(i);
                    var pointsResultsArray = RulesEngine.Execute_Rule_TierPoints(Convert.ToDouble(txnAmount), Convert.ToInt32(lyltyID));
                    var postgressPoints = pointsResultsArray[0];
                    var oraclePoints = pointsResultsArray[1];
                    if (postgressPoints.Equals(oraclePoints))
                    {
                        pointsPassedCount++;
                        Console.WriteLine("Postgress and Oracle Detail results are matching : see below");
                        Console.WriteLine("PointsResultSet from PostgressDB:{0}", postgressPoints + " PointsResultSet from OracleDB:{0}", oraclePoints);
                    }
                    else
                    {
                        pointsFailedCount++;
                        if (!File.Exists(pointsFilePath))
                            File.CreateText(pointsFilePath).Close();

                        string content = "Status:FAILED" + "|expected: " + postgressPoints[i] + " |actual: " + oraclePoints[i];

                        File.AppendAllText(pointsFilePath, content + Environment.NewLine);
                        Console.WriteLine("Postgress and Oracle Error results are not matching. For further info check the file at :{0}", errorLogPath + @"Points_Failed.txt");
                    }
                    if (pointsFailedCount > 0)
                    {
                        pointsResult = false;
                        Console.WriteLine("Test flow failed  with points rule validation for an input record");
                    }
                    else
                    {
                        pointsResult = true;
                        Console.WriteLine("Test flow passed  with points rule validation for an input record");
                    }

                }

            return new[] { pointsResult.ToString(), pointsPassedCount.ToString(), pointsFailedCount.ToString() };
        }

    }
}
