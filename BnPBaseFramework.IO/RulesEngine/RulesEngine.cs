using System;
using System.Configuration;
using Npgsql;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using BnPBaseFramework.IO.FileFunctions_Common;
namespace TemplateItems
{
    public static class RulesEngine
    {
        /// <summary>
        /// oracle connection  string to hold database details
        /// </summary>
        public static string OracleConnection = ConfigurationManager.AppSettings["OracleConn"];
        public static bool isEqual = false;
        public static int VCkey = 0;
        public static int vckey = 0;
        public static bool isInserted = false;
        public static OracleDataReader ORCDR = null;
        public static string oraclePointsResult = null;
        public static string postgressPointsResults = null;
        public static NpgsqlDataReader NpgsqlDR = null;
        public static string selQquery = null;


        /// <summary>
        /// Method to apply rules given transaction by taking transaction amount and loyalityid as input
        /// </summary>
        /// <param name="txnAmount">amount of the given tranasaction</param>
        /// <param name="loyalityID">loyalityid  of respective transaction </param>
        /// <returns></returns>
        public static string[] Execute_Rule_TierPoints(double txnAmount, int loyalityID)
        {
            int lineNum = 0;
            string ruleValue = ConfigurationManager.AppSettings["Rule"];
            isEqual = false;
            vckey = GetVCkeyFromLoyality(loyalityID);
            double point = Convert.ToInt32(ruleValue) * txnAmount;
            bool isInserted = InsertPoints(vckey, txnAmount, point);
            if (isInserted)
            {
                lineNum++;
                oraclePointsResult = "LineNumber:" + lineNum + GetPointsDataFromClientDb(vckey);
                postgressPointsResults = "LineNumber:" + lineNum + GetPointsDataFromQADb(vckey);
            }
            else
            {
                lineNum++;
                Console.WriteLine("Data not inserted");
            }
            return new[] { oraclePointsResult, postgressPointsResults };
        }

        /// <summary>
        /// Method to get VCkey from loyalityID
        /// </summary>
        /// <param name="loyalityID">given loyalityID</param>
        /// <returns></returns>
        public static int GetVCkeyFromLoyality(int loyalityID)
        {
            VCkey = 0;
            OracleConnection oracleConn = null;
            OracleCommand orclCommand = null;

            try
            {
                string query = "SELECT lw_virtualcard.vckey from lw_virtualcard where loyaltyIDNUMBER=" + "'" + loyalityID + "'" + "";
                oracleConn = new OracleConnection(OracleConnection);
                oracleConn.Open();
                orclCommand = new OracleCommand(query, oracleConn);
                orclCommand.CommandType = CommandType.Text;
                OracleDataReader dr = orclCommand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    VCkey = Convert.ToInt32(dr["VCKey"]);
                }
                else
                {
                    orclCommand.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                orclCommand.Dispose();
            }
            catch (Exception e)
            {
                orclCommand.Dispose();
                VCkey = 0;
            }
            finally
            {
                oracleConn.Close();
            }
            return VCkey;
        }

        /// <summary>
        /// Function to Insert points values into qa database 
        /// </summary>
        /// <param name="vCkey">given VCkey</param>
        /// <param name="txtAmount">given transaction amount</paramg>
        /// <param name="txtPoints">given transaction points</param>
        /// <returns></returns>
        public static bool InsertPoints(int vCkey, double txtAmount, double txtPoints)
        {
            isInserted = false;
            try
            {
                using (var connection = new NpgsqlConnection("Server=10.4.10.31;Port=5432;Database=qa_automation;User Id=bta_dev;Password=Aut0*13Q;"))
                {
                    connection.Open();
                    string commandText = "INSERT INTO automation." + '"' + "Points" + '"' + " " + "Values('" + vCkey + "','" + txtAmount + "','" + txtPoints + "')";
                    NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
                    int result = command.ExecuteNonQuery();
                    if (result == 1)
                        isInserted = true;
                }
            }
            catch (Exception ex)
            {
                isInserted = false;
            }
            return isInserted;
        }

        /// <summary>
        /// Get points details from QA database
        /// </summary>
        /// <param name="vCkey"></param>
        /// <returns></returns>
        public static string GetPointsDataFromQADb(int vCkey)
        {
            try
            {
                string selQquery = "Select * from automation." + '"' + "Points" + '"' + " " + "where" + '"' + "VCKEY" + '"' + " =" + vCkey + " order by " + '"' + "Created_At" + '"' + " desc LIMIT 1";
                NpgsqlDR = DataReader.PLSQLDataReader2(selQquery);
                if (NpgsqlDR.HasRows)
                {
                    while (NpgsqlDR.Read())
                    {
                        postgressPointsResults = NpgsqlDR[0].ToString() + NpgsqlDR[2].ToString();
                    }
                    NpgsqlDR.Close();
                }
            }

            catch (Exception ex)
            {
                postgressPointsResults = null;
            }
            return postgressPointsResults;
        }

        /// <summary>
        /// GetPoints  from Client Database
        /// </summary>
        /// <param name="vckey"></param>
        /// <returns></returns>
        public static string GetPointsDataFromClientDb(int vckey)
        {
            try
            {
                string selQquery = "Select * from (select * from lw_pointtransaction  where vckey=" + vckey + " order by pointtransactionid desc) where rownum=1";
                ORCDR = DataReader.OracleDataReader(selQquery);
                if (ORCDR.HasRows)
                {
                    while (ORCDR.Read())
                    {
                        oraclePointsResult = ORCDR[0].ToString() + ORCDR[7].ToString();
                    }
                    ORCDR.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return oraclePointsResult;
        }


    }

}



