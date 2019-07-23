using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using Oracle.ManagedDataAccess.Client;

namespace Bnp.Core.Tests.API.Validators
{
    public class DatabaseUtility
    {

        public static string AUTH_DIR = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("bin")) + "\\Tests\\API\\DataSource";
        public static string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source ='" + GetAuthFileName() + "';Extended Properties =\"Excel 12.0;HDR=YES;\"";
        public static OleDbConnection excelConn = new OleDbConnection(connString);


        public static OracleConnection GetDBConnectionSOAP()
        {
            return new OracleConnection(ConfigurationManager.AppSettings["DbConStringSOAP"]);
        }
        public static OracleConnection GetDBConnectionREST()
        {
            return new OracleConnection(ConfigurationManager.AppSettings["DbConStringREST"]);
        }
        public static OracleConnection GetDBConnectionQA()
        {
            return new OracleConnection(ConfigurationManager.AppSettings["DbConStringQA"]);
        }

        /// <summary>
        /// This method is used to retrieve the First Name of the user from database using SOAP service
        /// </summary>
        /// <param name="ID">IPCode of the member</param>
        /// <returns>First Name</returns>
        public static string GetMemberFNAMEfromDBUsingIdSOAP(string ID)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from LW_LOYALTYMEMBER where IPCODE=" + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["FIRSTNAME"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }
        /// <summary>
        /// this method is to retrive names of coupon using coupon id
        /// </summary>
        /// <param name="ID">coupoid for whichyou need coupon name</param>
        /// <returns>Coupon name of the coupon</returns>
        public static string GetCouponNameFromDBCDIS(string ID)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from  lw_coupondef where ID =  " + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["NAME"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// this method is to retrive coupon details using coupon name
        /// </summary>
        /// <param name="ID">couponame for whichyou need details</param>
        /// <returns>Coupon name of the coupon</returns>
        public static CategoryFields GetCouponDetailsFromDB(string Name)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            CategoryFields coupon = new CategoryFields();

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from  lw_coupondef where NAME =  " + "'" + Name + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    coupon.Name = dr["NAME"].ToString();
                    DateTime Sdate = Convert.ToDateTime(dr["STARTDATE"]);
                    coupon.StartDate = Sdate.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                    DateTime Edate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    coupon.ExpiryDate = Edate.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                    coupon.IsGlobal = dr["ISGLOBAL"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return coupon;
        }

        /// <summary>
        /// This method is to retrive names of bonus using bonus id
        /// </summary>
        /// <param name="ID">BonusId for which you need bonus name</param>
        /// <returns>Bonus name of the bonus</returns>
        public static string GetBonusNameFromDBCDIS(string ID)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from  lw_bonusdef where ID =  " + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["NAME"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// this method is to retrive coupon details using coupon name
        /// </summary>
        /// <param name="ID">couponame for whichyou need details</param>
        /// <returns>Coupon name of the coupon</returns>
        public static CategoryFields GetCouponDetailsFromDBCDIS(string Name, string isGlobalStatus)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            CategoryFields coupon = new CategoryFields();

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from  lw_coupondef where NAME =  " + "'" + Name + "' and isglobal=" + isGlobalStatus + "";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    coupon.Name = dr["NAME"].ToString();
                    DateTime Sdate = Convert.ToDateTime(dr["STARTDATE"]);
                    coupon.StartDate = Sdate.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                    DateTime Edate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    coupon.ExpiryDate = Edate.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                    coupon.IsGlobal = dr["ISGLOBAL"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return coupon;
        }

        /// <summary>
        /// the following method is to return the city of member
        /// </summary>
        /// <param name="ipcode">ipcode of the member</param>
        /// <returns>city name</returns>
        public static string GetCityusingIPCODEREST(int ipcode)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from ats_memberdetails where A_ipcode=" + ipcode;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["A_CITY"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This method is used to get theEmail Id when IPCODE of the member is passed as an parameter
        /// </summary>
        /// <param name="IpCode"></param>
        /// <returns>Email ID for a member whose IPCODE is passed as an parameter</returns>
        public static string GetEmailfromDBUsingIpcodeREST(int IpCode)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_loyaltymember where ipcode=" + IpCode;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["PRIMARYEMAILADDRESS"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This Method to get the Reward Catalog
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>CERTIFICATETYPECODE for a member whose ID is passed as an paramete</returns>
        public static string GetRewardCatalogfromDBUsingIdSOAP(string ID)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from LW_Rewardsdef where ID=" + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["ID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This method is to retrive the reward addition id of the required member  using Ipcode of the member
        /// </summary>
        /// <param name="IpCode">Ipcode of required member</param>
        /// <returns> reward addition ID</returns>
        public static string GetRewardIDfromDBUsingIPCODEREST(string IpCode)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_memberrewards where memberid =" + IpCode;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["REWARDDEFID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This member is to retrieve member;s promotion Count using Member Id for SOAP services.
        /// </summary>
        /// <param name="Id">Ipcode</param>
        /// <returns>Member's promotion code</returns>
        public static string GetMemberPromotionsCountUsingIdFromDBSOAP(String Id)
        {

            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select Count(*) from lw_memberpromotion where MEMBERID =" + Id;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["COUNT(*)"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// This method is to retrieve the first Loyalty Id from the database using SOAP
        /// </summary>
        /// <returns>First Card Id</returns>
        public static string GetFirstLoyaltyIDFromDBUSingSOAP(Tiers_EntryPoints tiers)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            string sql = null;
            try
            {
                switch (tiers)
                {
                    case Tiers_EntryPoints.Standard:
                        sql = "Select LW_VIRTUALCARD.LOYALTYIDNUMBER FROM LW_MEMBERTIERS INNER JOIN LW_VIRTUALCARD ON LW_MEMBERTIERS.MEMBERID =LW_VIRTUALCARD.IPCODE WHERE TIERID = '507'";
                        break;
                    case Tiers_EntryPoints.Silver:
                        sql = "Select LW_VIRTUALCARD.LOYALTYIDNUMBER FROM LW_MEMBERTIERS INNER JOIN LW_VIRTUALCARD ON LW_MEMBERTIERS.MEMBERID =LW_VIRTUALCARD.IPCODE WHERE TIERID = '509'";
                        break;
                    case Tiers_EntryPoints.Gold:
                        sql = "Select LW_VIRTUALCARD.LOYALTYIDNUMBER FROM LW_MEMBERTIERS INNER JOIN LW_VIRTUALCARD ON LW_MEMBERTIERS.MEMBERID =LW_VIRTUALCARD.IPCODE WHERE TIERID = '511'";
                        break;
                    case Tiers_EntryPoints.Platinum:
                        sql = "Select LW_VIRTUALCARD.LOYALTYIDNUMBER FROM LW_MEMBERTIERS INNER JOIN LW_VIRTUALCARD ON LW_MEMBERTIERS.MEMBERID =LW_VIRTUALCARD.IPCODE WHERE TIERID = '513'";
                        break;
                    default:
                        break;
                }

                conn = GetDBConnectionSOAP();
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["LOYALTYIDNUMBER"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// this method is to retrive attribute names  of coupons
        /// </summary>
        /// <returns>list of attribute names of coupon</returns>
        public static List<string> GetCouponAttributeNamesfromDBCDIS()
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> names = new List<string>();

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_Contentattributedef lw where lw.contenttypes like '%Coupon%' ";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();


                if (dr.HasRows)
                {
                    dr.Read();
                    do
                    {

                        number = dr["NAME"].ToString();
                        names.Add(number);

                    }
                    while (dr.Read());
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }


                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
                names.Add(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return names;
        }

        public static List<string> GetAttributeDetailsfromDB()
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> names = new List<string>();

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "Select * from lw_contentattributedef cad where 1 = 1 order by cad.createdate desc";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    do
                    {

                        number = dr["NAME"].ToString();
                        names.Add(number);

                    }
                    while (dr.Read());
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }


                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
                names.Add(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return names;
        }


        /// <summary>
        /// This method is to provide the transaction Headerid from txnheadertable using VCkey of the member's Card
        /// </summary>
        /// <param name="vckey">vc key of the member whose transactionId is required</param>
        /// <returns>transaction headerId</returns>
        public static string GetTxnHeaderIdusingVCKeyDBSoap(string vckey)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from ats_txnheader where A_VCKEY=  " + vckey;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["A_TXNHEADERID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        public static string GetMemberDetailsIdusingIpcodefromDBREST(string ID)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from ats_memberdetails where A_IPCODE=" + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["A_ROWKEY"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;

        }

        /// <summary>
        /// This method is to retrieve the First Name of the member from Database using REST Service.
        /// </summary>
        /// <param name="ID">IPCode</param>
        /// <returns>First Name</returns>
        public static string GetMemberFNAMEfromDBUsingIdREST(string ID)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from LW_LOYALTYMEMBER where IPCODE=" + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["FIRSTNAME"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This method is to provide the Quantity from txndetailitem table using VCkey of the member's Card
        /// </summary>
        /// <param name="vckey">VCKey of the member</param>
        /// <returns>Quantity</returns>
        public static string GetQuantityfromDBUsingVCKeyREST(string vckey)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from ats_txndetailitem where A_VCKEY=" + vckey;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["A_DTLQUANTITY"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// Following Method is to fetch the promotions count from database for SOAP services
        /// </summary>
        /// <returns>promotions count value as a string</returns>
        public static string GetpromotionsCountfromDbSOAP()
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select count(*) from lw_promotion where locationgroupid is null";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["COUNT(*)"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// Following Method is to fetch the promotion data from database 
        /// </summary>
        /// <returns>promotion</returns>
        public static Promotions GetpromotionsDetailsfromDb(string Name, int EnrollmenntType, int Targeted)
        {
            Promotions promotion = new Promotions();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_promotion where NAME =  " + "'" + Name + "'" + "And" + " " + "ENROLLMENTSUPPORTTYPE =" + "'" + EnrollmenntType + "'" + "And" + " " + "Targeted=" + "'" + Targeted + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    promotion.Name = dr["NAME"].ToString();
                    promotion.Description = dr["PROMOTIONDESCRIPTION"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided Name");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to retrive data from database");
            }
            finally
            {
                conn.Close();
            }
            return promotion;
        }

        /// <summary>
        /// Following Method is to fetch the bonus count from database 
        /// </summary>
        /// <returns>Bonus count</returns>
        public static string GetBonusCountfromDbSOAP()
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select count(*) from lw_bonusdef";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["COUNT(*)"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This method is to fetch the Loyalty Card count from database for SOAP services
        /// </summary>
        /// <param name="ipCode">IPCode of the member</param>
        /// <returns>Loyalty Card count value</returns>
        public static string GetLoyaltyCardCountfromDbSOAP(string ipCode)
        {
            string number = null;
            int rowcount = 0;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_virtualcard  where IPCODE=" + ipCode;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        rowcount++;
                    }
                    number = rowcount + "";
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }
        /// <summary>
        /// This method is to fetch the email id from database using SOAP services
        /// </summary>
        /// <param name="IPcode">IPCode of the member</param>
        /// <returns>Email Id</returns>
        public static string GetEmailIDfromDBSOAP(string IPcode)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_loyaltymember where IPCODE=" + IPcode;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["PRIMARYEMAILADDRESS"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }

            return number;

        }
        /// <summary>
        /// This method is to fetch member status of a member from database using SOAP services
        /// </summary>
        /// <param name="ipCode">IPCode of the member</param>
        /// <returns>Member Status</returns>
        public static string GetMemberStatusfromDbSOAP(string ipCode)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_loyaltymember where IPCODE='" + ipCode + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["MEMBERSTATUS"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }

            return number;
        }

        /// <summary>
        /// This method is used to retrieve the expiration date of the card from database using SOAP services.
        /// </summary>
        /// <param name="loyaltyIdNumber">Card Id</param>
        /// <returns>Expiration date of the card</returns>
        public static string GetExpirationDatefromDbSOAP(string loyaltyIdNumber)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_virtualcard  where LOYALTYIDNUMBER='" + loyaltyIdNumber + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["ExpirationDATE"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// This method is to retrieve Loyalty Card's status from the database using SOAP services
        /// </summary>
        /// <param name="id">Loyalty Id</param>
        /// <returns>Card status</returns>
        public static string GetLoyaltyCardStatusfromDbSOAP(string id)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_virtualcard where LOYALTYIDNUMBER= '" + id + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["NEWSTATUS"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// This method is to retrieve the VC Key from database by using Rowkey for SOAP services
        /// </summary>
        /// <param name="rowkey"></param>
        /// <returns>VC key</returns>
        public static List<string> GetVCkeyfromDBUsingROWKEYREST(string rowkey)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> ats_txnheader = new List<string>();
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from ats_txnheader  where A_ROWKEY=" + rowkey;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    ats_txnheader.Add(dr["A_VCKEY"].ToString());
                    ats_txnheader.Add(dr["A_TXNHEADERID"].ToString());
                    ats_txnheader.Add(dr["A_TXNAMOUNT"].ToString());
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            finally
            {
                conn.Close();
            }
            return ats_txnheader;
        }

        /// <summary>
        /// This method is used to retrieve existing Loyalty Card id which is in Active status for SOAP services.
        /// </summary>
        /// <returns>Existing Loyalty Card id which is in Active Status</returns>
        public static string GetExistingLoyaltyCardIDwithActiveStatus()
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_virtualcard  where STATUS=1 AND NEWSTATUS is NULL";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["LOYALTYIDNUMBER"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// This method is used to retrieve existing Loyalty Card id which is in Active status for REST services.
        /// </summary>
        /// <returns>Existing Loyalty Card id which is in Active Status</returns>
        public static string GetExistingLoyaltyCardIDwithActiveStatusREST()
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_virtualcard  where STATUS=1 AND NEWSTATUS is NULL";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["LOYALTYIDNUMBER"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }

            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// This method is to retrieve Loyalty Id for SOAP services
        /// </summary>
        /// <param name="ID"> IPCode </param>
        /// <returns>Card Id</returns>
        public static string GetLoyaltyID(string ID)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "Select LOYALTYIDNUMBER from LW_VIRTUALCARD where IPCODE =" + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["LOYALTYIDNUMBER"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }
        /// <summary>
        /// This method is used to retrieve member coupon id using Vc key for SOAP services
        /// </summary>
        /// <param name="Id">Member Id</param>
        /// <returns>Member Coupon Id</returns>
        public static string GetMemberCouponIdUsingVCKeyFromDBSOAP(string Id)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_membercoupon where MEMBERID =" + Id;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["ID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }
        /// <summary>
        /// This member is to retrieve member;s promotion code using Member Id for SOAP services.
        /// </summary>
        /// <param name="Id">Member Id</param>
        /// <returns>Member's promotion code</returns>
        public static string GetMemberPromotionsCodeUsingIdFromDBSOAP(string Id)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_memberpromotion where MEMBERID =" + Id;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["ID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// This Method is to retrieve Enrolled Promotion Member.
        /// </summary>
        /// <param name="Id">Member Id</param>
        /// <returns>Member's promotion code</returns>
        public static string GetEnrollMemberPromotionsFromDBSOAP(string Id)
        {

            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_memberpromotion where MEMBERID =" + Id;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["ENROLLED"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }
        /// <summary>
        /// Following method is to retrieve the attributes list
        /// </summary>
        /// <param name="ID">loyaltyID</param>
        /// <returns>list of values</returns>
        public static string GetCouponAttributesValues(string ID)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select COUNT(*) from Lw_Contentattribute where REFID =  " + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();


                if (dr.HasRows)
                {

                    dr.Read();
                    number = dr["COUNT(*)"].ToString();

                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }


                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }
        public static List<string> GetTransactionDetailsFromTransationHistoryTable(string Transacation)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> namesList = new List<string>();
            try
            {

                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "Select * from ats_historytxndetail where A_TXNHEADERID='" + Transacation + "'";

                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    namesList.Add(dr["A_TXNHEADERID"].ToString());
                    namesList.Add(dr["A_TXNREGISTERNUMBER"].ToString());
                    namesList.Add(dr["A_TXNAMOUNT"].ToString());
                    namesList.Add(dr["A_TXNDATE"].ToString());
                    namesList.Add(dr["A_TXNSTORENUMBER"].ToString());
                    return namesList;
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception details:" + e);
            }
            finally
            {
                conn.Close();
            }
        }
        public static List<string> GetTransactionDetailsFromTransactionHeaderTable(string Transacation)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> namesList = new List<string>();
            try
            {

                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "Select * from Ats_Txnheader where A_TXNHEADERID='" + Transacation + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    namesList.Add(dr["A_TXNHEADERID"].ToString());
                    namesList.Add(dr["A_TXNREGISTERNUMBER"].ToString());
                    namesList.Add(dr["A_TXNAMOUNT"].ToString());
                    namesList.Add(dr["A_TXNDATE"].ToString());
                    namesList.Add(dr["A_TXNSTOREID"].ToString());
                    return namesList;
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception details:" + e);
            }
            finally
            {
                conn.Close();
            }
        }
        public static List<string> GetTransactionDetailsFromTransationHistoryTablewithoutTransaction()
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> namesList = new List<string>();
            try
            {

                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "SELECT *from ats_historytxndetail b WHERE (A_TXNLOYALTYID IS NULL AND ROWNUM = 1) and NOT EXISTS(SELECT* FROM Ats_Txnheader a    WHERE a.A_TXNHEADERID = b.A_TXNHEADERID)";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    namesList.Add(dr["A_TXNHEADERID"].ToString());
                    namesList.Add(dr["A_TXNREGISTERNUMBER"].ToString());
                    namesList.Add(dr["A_TXNAMOUNT"].ToString());
                    namesList.Add(dr["A_TXNDATE"].ToString());
                    namesList.Add(dr["A_TXNSTORENUMBER"].ToString());
                    namesList.Add(dr["A_TXNWEBORDERNMBR"].ToString());
                    return namesList;
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided Query");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception details:" + e);
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool VerifyCategoryAvilableDetails(string Env, string Category)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> namesList = new List<string>();
            string Category_Name;
            try
            {
                if (Env.Contains("Dev"))
                {
                    conn = GetDBConnectionSOAP();
                }
                else
                {
                    conn = GetDBConnectionQA();
                }
                conn.Open();
                string sql = "Select Name from Lw_Category where Name='" + Category + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Category_Name = dr["Name"].ToString();
                    if (Category_Name.Equals(Category))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    cmd.Dispose();
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception details:" + e);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// This method is to retrieve the notification details
        /// </summary>
        /// <param name="Env">Environment name</param>
        /// <param name="notificationName">reward id</param>
        /// <returns>Catalog Count</returns>
        public static bool VerifyNotificationAvilableDetails(string Env, string notificationName)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> namesList = new List<string>();
            string Category_Name;
            try
            {
                if (Env.Contains("Dev"))
                {
                    conn = GetDBConnectionSOAP();
                }
                else
                {
                    conn = GetDBConnectionQA();
                }
                conn.Open();
                string sql = "Select Name from Lw_notificationdef where Name='" + notificationName + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Category_Name = dr["Name"].ToString();
                    if (Category_Name.Equals(notificationName))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    cmd.Dispose();
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception details:" + e);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// This method is to retrieve the Catalog Count
        /// </summary>
        /// <returns>Catalog Count</returns>
        public static string GetRewardCatalogCountfromDBUsingIdSOAP()
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                //if location group is set then to get accurate count we should give the store numbers in the response
                //or we can exculde them in the below query
                // if for any reward rewardtype is set to payment then exclude that reward by adding RewardType != 1 (Because count dont include payment reward)
                string sql = "select COUNT(*) from LW_Rewardsdef where LOCATIONGROUPID is null and RewardType != 1";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["COUNT(*)"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// this method is to fetch the reward name from reward definition table using its id
        /// </summary>
        /// <param name="id">reward id</param>
        /// <returns>reward name</returns>
        public static string GetRewardNameUsingIDDBREST(string id)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_rewardsdef where id=" + id;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["NAME"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }


        /// <summary>
		/// This method is to retrieve member promotions by providing IPCode as parameter.
		/// </summary>
		/// <param name="memberID">IPCode</param>
		/// <returns>Promotion Id</returns>
		public static string GetMemberPromotionsUsingIpcodefromDBREST(string memberID)
        {
            String name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_memberpromotion where memberid=" + memberID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["ID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;

        }

        /// <summary>
		/// This method is to retrieve the recent promotions
		/// </summary>
		/// <returns>List of recent promotionss</returns>
		public static string GETRecentPromotionfromDBREST()
        {
            String name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_promotion  where targeted=1 order by enddate desc";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["CODE"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This method is to retrieve the Redemption Date of the required member using RewardId of the member
        /// </summary>
        /// <param name="RewardId">RewardId of required member</param>
        /// <returns>Redemption Date</returns>
        public static string GetRedemptionDatefromDBUsingRewardIdREST(string RewardId)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_memberrewards where id =" + RewardId;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["REDEMPTIONDATE"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
        /// This method is used to retrieve the Certificate number from database using REST service
        /// </summary>
        /// <param name="ipcode">IPCode of the member</param>
        /// <returns>Certificate Number</returns>
        public static string GetCertNumberFromDBREST(string ipcode)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionREST();
                conn.Open();
                string sql = "select * from lw_memberrewards where memberid=" + ipcode;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["CERTIFICATENMBR"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }


        /// <summary>
        /// This Methos is to get the Reward Catalog Item
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetRewardCatalogitemfromDBUsingIdSOAP(string ID)
        {
            Dictionary<string, string> productList = new Dictionary<string, string>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();

                string sql = "select lw_product.id,lw_product.name, lw_productvariant.id, Lw_Productvariant.Variantdescription, Lw_Productvariant.Partnumber, LW_PRODUCTVARIANT.QUANTITY, LW_PRODUCTVARIANT.VARIANTORDER " +
                    "from lw_productvariant inner join lw_product On lw_product.id = lw_productvariant.Productid where lw_productvariant.Productid =" + ID;
                // string sql = "select * from lw_productvariant inner join lw_product On lw_product.id=lw_productvariant.Productid where lw_productvariant.Productid = " + ID;

                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    productList.Add("ProductID", dr["ID"].ToString());
                    productList.Add("ProductName", dr["NAME"].ToString());
                    productList.Add("ProductvariantID", dr.GetValue(2).ToString());
                    productList.Add("ProductvariantDescription", dr["Variantdescription"].ToString());
                    productList.Add("ProductvariantPartNumber", dr["PARTNUMBER"].ToString());
                    productList.Add("ProductvariantQuantity", dr["QUANTITY"].ToString());
                    productList.Add("ProductvariantOrder", dr["VARIANTORDER"].ToString());
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Exception details:" + e);
            }
            finally
            {
                conn.Close();
            }
            return productList;
        }


        /// <summary>
        /// This Method is to get the Reward Member through ID
        /// </summary>
        /// <param name="IpCode" </param>
        /// <param name="rewarddefid"
        /// <returns>ID</returns>

        public static string GetMemberRewardIDfromDBUsingIdSOAP(string IpCode, string rewarddefid)
        {
            string name = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_memberrewards where memberid=" + IpCode + " and rewarddefid=" + rewarddefid;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    name = dr["ID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return name;
        }

        /// <summary>
		/// This method is to provide the Quantity from txnheadertable using VCkey of the member's Card
		/// </summary>
		/// <param name="txnHeaderId">txn header of the member whose transactionId is required</param>
		/// <returns>transaction headerId</returns>
		public static string GetTxnAmountusingVCKeyDBSoap(string txnHeaderId)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from ats_txnheader where A_TxnHeaderId='" + txnHeaderId + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["A_TXNAMOUNT"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// Following method is to retrieve the Loyalty Currencies 
        /// </summary>
        /// <param name="Currency Description">Description</param>
        /// <returns>Currency Status</returns>
        public static List<String> GetLoyalityCurrenciesfromDBUsingIdSOAP()
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> strings = new List<string>();

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from LW_POINTTYPE";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        strings.Add(dr["NAME"].ToString());
                    }
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }
            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return strings;
        }

        /// <summary>
        /// Generic method to return data from DB
        /// </summary>
        /// <param name="table"></param>
        /// <param name="whereclause"></param>
        /// <param name="paramter"></param>
        /// <param name="columnName"></param>
        /// <returns>returns data for the column specified</returns>
        public static string GetFromSoapDB(string table, string whereclause, string parameter, string columnName, string sqlstmt)
        {
            string result = null;
            List<string> results = new List<string>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            string sql = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                if (!sqlstmt.Equals(string.Empty))
                {
                    sql = sqlstmt;
                }
                else if (whereclause.Equals(string.Empty))
                {
                    sql = "select * from " + table;
                }
                else
                {
                    sql = "select * from " + table + " where " + whereclause + " =  '" + parameter + "'";

                }
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    result = dr[columnName].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                cmd.Dispose();
                result = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// To get active coupons from database
        /// </summary>
        /// <returns></returns>
        public static string GetActiveCouponFromDB()
        {
            string result = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            string sql = "select * from lw_virtualcard where status='1' and newstatus is null";
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    result = dr["LOYALTYIDNUMBER"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                cmd.Dispose();
                result = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// This Method is to get the Available Balance for member rewards
        /// </summary>
        /// <returns>Available Balance</returns>

        public static List<string> GetAvailableBalanceDfromDBUsingIdSOAP(string memberrewardid)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> memberrewards = new List<string>();
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_memberrewards where id=" + memberrewardid;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    memberrewards.Add(dr["AVAILABLEBALANCE"].ToString());
                    memberrewards.Add(dr["REDEMPTIONDATE"].ToString());
                    memberrewards.Add(dr["EXPIRATION"].ToString());
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            finally
            {
                conn.Close();
            }
            return memberrewards;
        }
        /// <summary>
		/// This method is to provide the Get Member Messages
		/// </summary>
		/// <param name="vckey">vc key of the member ID</param>
		/// <returns>Member DefID</returns>
		public static string GetMemberMessageDefIDfromDBUsingIdSOAP(string ID)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "Select * from Lw_Membermessage where ID='" + ID + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["MESSAGEDEFID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// Following method is to retrieve the Loyalty Currencies Balance
        /// </summary>
        /// <param name="Currency Description">Description</param>
        /// <returns>Currency Status</returns>
        public static string GetLoyaltyCurrencyBalancesfromDBUsingIdSOAP(string ID)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select SUM(POINTS) from lw_pointtransaction where vckey =  " + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["SUM(POINTS)"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// Following method is to retrieve the Loyalty Events
        /// </summary>
        /// <returns>list of loyaltyevents</returns>
        public static List<string> GetLoyaltyEventsfromDBUsingIdSOAP()
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> strings = new List<string>();
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from LW_POINTEVENT";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        strings.Add(dr["NAME"].ToString());

                    }
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                cmd.Dispose();
                e.GetBaseException();
            }
            finally
            {
                conn.Close();
            }
            return strings;
        }

        /// <summary>
		/// Following method is to retrieve the SUM(Points)
		/// </summary>
		/// <param name="ID">Virtual card id</param>
		/// <returns>SUM(Points)</returns>
		public static String GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(string ID)
        {
            String number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select SUM(POINTS) from lw_pointtransaction where vckey =  " + ID;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();


                if (dr.HasRows)
                {

                    dr.Read();
                    number = dr["SUM(POINTS)"].ToString();

                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }
        /// <summary>
        /// Following method is to retrieve the Member Reward Status
        /// </summary>
        /// /// <param name="IPCode">IpCode</param>
        /// <param name="ID">Memeber Reward ID</param>
        /// <returns>Memeber Reward Status</returns>
        public static String GetMemberRewardStatusfromDBUsingIdSOAP(string IpCode, String rewarddefid)
        {
            String number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_memberrewards where memberid=" + IpCode + " and rewarddefid=" + rewarddefid;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    number = dr["ORDERSTATUS"].ToString();

                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// Following method is to retrieve the ProductDetails
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static CategoryFields GetProductDetailsFromDB(string Name)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            CategoryFields product = new CategoryFields();
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from  LW_product where NAME =  " + "'" + Name + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    product.Name = dr["NAME"].ToString();
                    product.Quantity = dr["QUANTITY"].ToString();
                    product.ID = dr["ID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return product;
        }

        /// <summary>
        /// To get updated Member Promotions code from Db
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetUpdatedMemberPromotionsCodeUsingIdFromDBSOAP(string Id)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_memberpromotion where MEMBERID =" + Id + " order by CREATEDATE desc";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["ID"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// Following method is to create a product variant from database
        /// </summary>
        /// <param name="Name"></param>
        /// <returns> returns true if product variant gets created else false</returns>
        public static bool CreateProductVariant(CategoryFields productVariant)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            CategoryFields product = new CategoryFields();
            Common common = new Common(new DriverContext());
            int variantID = Convert.ToInt32(common.RandomNumber(4));
            product = GetProductDetailsFromDB(productVariant.CategoryTypeValue);
            int productID = Convert.ToInt32(product.ID);
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string date = DateTime.Now.ToString("dd-MMM-yy hh.mm.ss tt").ToUpper();
                string sql = "INSERT INTO Lw_Productvariant Lv (Lv.variantdescription, Lv.productid, Lv.id,Lv.partnumber,Lv.quantity, Lv.quantitythreshold, Lv.variantorder,Lv.createdate) VALUES('" + productVariant.Name + "', " + productID + ", '" + variantID + "', '" + productVariant.PartNumber + "', '" + productVariant.Quantity + "', '" + productVariant.QuantityThreshold + "', '" + productVariant.VariantOrder + "', '" + date + "')";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                string sqlSelect = "select * from lw_productvariant  where VARIANTDESCRIPTION = '" + productVariant.Name + "'";
                cmd = new OracleCommand(sqlSelect, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    cmd.Dispose();
                    return true;
                }
                else
                {
                    cmd.Dispose();
                    return false;
                }
            }
            catch (Exception e)
            {
                cmd.Dispose();
                throw new Exception(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// To get Tier names from database
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTierNamesFromDBCDIS()
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> tierNames = new List<string>();
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_tiers";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    do
                    {
                        number = dr["TIERNAME"].ToString();
                        tierNames.Add(number);
                    }
                    while (dr.Read());
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
                tierNames.Add(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return tierNames;
        }

        /// <summary>
        /// To get coupons definitions from DB
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCouponsDefFromDB()
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            List<string> tierNames = new List<string>();
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from lw_coupondef";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    do
                    {
                        number = dr["NAME"].ToString();
                        tierNames.Add(number);
                    }
                    while (dr.Read());
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
                tierNames.Add(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return tierNames;
        }

        /// <summary>
        /// this method is to retrive locationgroup details using locationgroup name
        /// </summary>
        /// <param name="Name">locationgroupname for whichyou need details</param>
        /// <returns>Name of the locationgroup</returns>
        public static CategoryFields GetLocationGroupDetailsFromDB(string Name)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            CategoryFields locationGroup = new CategoryFields();

            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from  lw_locationgroup where NAME =  " + "'" + Name + "'";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    locationGroup.LocationGroupName = dr["NAME"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }

                cmd.Dispose();
            }

            catch (Exception e)
            {
                cmd.Dispose();
                number = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return locationGroup;
        }



        public static string GetAuthFileName()
        {

            string name = null;
            if (Directory.GetFiles(AUTH_DIR).Length > 1)
            {
                name = Directory.GetFiles(AUTH_DIR)[1];
                name = name.Replace(@"\", @"\\");
            }
            else
            {
                throw new Exception("Mad files count is not 1");
            }
            //    Logger.Info(name);

            return name;
        }



        public static DataTable GetTestDataForAuthTokenTask()
        {
            return GetTable("SELECT * FROM [AuthTokenRegression$]");
        }

        public static DataRow[] getDataRowsByCondtion(DataTable dataTable, string searchCondition, string searchValue)
        {
            DataRow[] dataRows = dataTable.Select(searchCondition = "'" + searchValue + "'");
            return dataRows;
        }

        public static DataRow GetExpectedDataRow(DataTable dataTable, int index)
        {
            return dataTable.Rows[index];
        }

        public static string GetExpectedColumValue(DataTable dataTable, int index, string columnName)
        {
            return GetExpectedDataRow(dataTable, index)[columnName].ToString();
        }

        public static Dictionary<int, string> GetClientIDAndClietnServerData()
        {
            Dictionary<int, string> clientIDVsClientSecret = new Dictionary<int, string>();
            try
            {
                int i = 2;

                int rowsAvailCount = GetTestDataForAuthTokenTask().Rows.Count;
                foreach (DataRow dataRow in GetTestDataForAuthTokenTask().Rows)
                {

                    string key = dataRow["clientId"].ToString();
                    string value = dataRow["clientSecret"].ToString();
                    clientIDVsClientSecret.Add(i, key + "||" + value);
                    i++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return clientIDVsClientSecret;
        }


        /// <summary>
        /// Getting Data table sheet info from Excel
        /// </summary>
        /// <param name="excelQuery"></param>
        /// <param name="excelConnection"></param>
        /// <returns></returns>
        public static DataTable GetTable(string excelQuery)
        {
            DataTable sheetInfo = new DataTable();
            OleDbCommand olDBcommand = new OleDbCommand();
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
            try
            {
                excelConn.Open();

                try
                {
                    olDBcommand = new OleDbCommand(excelQuery, excelConn);
                    oleDbDataAdapter = new OleDbDataAdapter(olDBcommand);
                }
                catch (Exception)
                {

                    throw new Exception("Excel sheet is opened please CLOSE IT AND TRY AGAIN");
                }

                oleDbDataAdapter.Fill(sheetInfo);
                excelConn.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
            return sheetInfo;
        }

        /// <summary>
        /// To get a value from a table which is not null
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetValueNotNullFromDBSOAP(string table, string columnName)
        {
            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select * from " + table + " where " + columnName + " is not null";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr[columnName].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

        /// <summary>
        /// Get value count form a table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GetCountFromDBSOAP(String table)
        {

            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = "select Count(*) from " + table;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["COUNT(*)"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }
        public static string GetCountFromDBBasedOnQuery(string Query)
        {

            string number = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                conn = GetDBConnectionSOAP();
                conn.Open();
                string sql = Query;
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    number = dr["COUNT(*)"].ToString();
                }
                else
                {
                    cmd.Dispose();
                    throw new Exception("No data is present in database with provided data");
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                conn.Close();
            }
            return number;
        }

    }
}
