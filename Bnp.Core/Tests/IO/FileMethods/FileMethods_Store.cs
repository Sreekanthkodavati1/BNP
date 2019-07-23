using System;
using System.IO;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using BnPBaseFramework.IO.FileFunctions_Common;

namespace Bnp.Core.Tests.IO.FileMethods
{

    public static class FileMethods_Store
    {
        public static string SELQuery = null;
        public static string PLSQLresults = null;
        public static string Oracleresults = null;
        public static string libJobQuery = null;
        public static string libMsgQuery = null;
        public static string OracleReason = null;
        public static string JobNumber = null;
        public static string Reason = null;
        public static string StoreSch = ConfigurationManager.AppSettings["QADBSchema"];
        public static string StoreTbl = ConfigurationManager.AppSettings["StoreTable"];
        public static string LibJobTbl = ConfigurationManager.AppSettings["LibJob"];
        public static string LibmsglogTbl = ConfigurationManager.AppSettings["LibMSGLog"];
        public static OracleDataReader ORCDR = null;
        public static string PhoneNumError = "No value found for required attribute PhoneNumber in the message.";
        public static string AddressLineOneError = "No value found for required attribute AddressLineOne in the message.";
        public static string CityError = "No value found for required attribute City in the message.";
        public static string CountryError = "No value found for required attribute Country in the message.";
        public static string StateOrProvinceError = "No value found for required attribute StateOrProvince in the message.";
        public static string MandatoryAttributesPresent = "No Empty string";
        public static string StoreFilePath = ConfigurationManager.AppSettings["StoreFilePth"];
        public static string filePath = null;
        public static string fileName = null;
        public static string WhereClause = null;
        public static string ErrorPrefix = "This error message is from lw_libmessagelog table. ";
        public static string CorrectResultPrefix = "This result set is from lw_Storedef table. ";


        /// <summary>
        /// Used to read result set from OracleDataReader in Oracle DB by passing WhereClause in the parameter
        /// </summary>
        /// <param name="WhereClause">WhereClause as a part of SQL query to be executed through OracleDataReader</param>
        /// <returns>Oracleresults</returns>
        public static string ReadFromOracle(string WhereClause)
        {
            try
            {
                SELQuery = "SELECT * FROM " + StoreTbl + " WHERE " + WhereClause;
                ORCDR = DataReader.OracleDataReader(SELQuery);
                if (ORCDR.HasRows)
                {
                    while (ORCDR.Read())
                    {
                        Oracleresults = ORCDR[1].ToString() + ORCDR[2].ToString() + ORCDR[3].ToString() + ORCDR[4].ToString() + ORCDR[5].ToString() + ORCDR[6].ToString() + ORCDR[7].ToString() + ORCDR[8].ToString() + ORCDR[9].ToString() + ORCDR[10].ToString() + ORCDR[11].ToString() + ORCDR[12].ToString() + ORCDR[13].ToString() + ORCDR[14].ToString() + ORCDR[17].ToString() + ORCDR[18].ToString() + ORCDR[19].ToString() + ORCDR[20].ToString() + ORCDR[21].ToString() + ORCDR[22].ToString();
                    }
                    ORCDR.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Oracleresults;
        }

        /// <summary>
        /// Used to read Reason from OracleDataReader in Oracle DB by passing WhereClause in the parameter
        /// </summary>
        /// <param name="OracleReason">OracleReason as a part of SQL query to be executed through OracleDataReader</param>
        /// <returns>Reason</returns>
        public static string ReadReasonFromOracle(string OracleReason)
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
                        Console.WriteLine(JobNumber);
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

        /// <summary>
        /// Used to write result set to PLSQL DB by passing InsQuery in the parameter
        /// </summary>
        /// <param name="InsQuery">InsQuery to be executed through PLSQLDataReader</param>
        /// <returns>InsQuery</returns>
        public static string WriteToPLSQL(string InsQuery)
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

        /// <summary>
        /// Used to read result set from input files by passing fileName in the parameter.
        /// </summary>
        /// <param name="fileName">fileName of the file to be tested</param>
        /// <returns>PLSQLresults</returns>
        public static string ReadMandatoryValuesFrmPLSQL(string fileName)
        {
            try
            {
                filePath = StoreFilePath + "\\" + fileName;


                foreach (string line in File.ReadLines(filePath))
                {
                    string value = line.Replace('|', ',');
                    string[] MandatoryAttribute = value.ToString().Split(',');
                    //int FieldCount = PP.Length;
                    if (MandatoryAttribute.Length == 23)
                    {
                        string PhoneNumber = MandatoryAttribute[6].ToString();
                        string AddressLineOne = MandatoryAttribute[7].ToString();
                        string City = MandatoryAttribute[9].ToString();
                        string StateOrProvince = MandatoryAttribute[10].ToString(); ;
                        string Country = MandatoryAttribute[13].ToString();

                        string error = PhoneNumber == "" ? PhoneNumError : AddressLineOne == "" ? AddressLineOneError : City == "" ? CityError : StateOrProvince == "" ? StateOrProvinceError : Country == "" ? CountryError : MandatoryAttributesPresent;
                        if (string.Equals(error, PhoneNumError))
                        {
                            PLSQLresults = ErrorPrefix + PhoneNumError;
                        }
                        if (string.Equals(error, AddressLineOneError))
                        {
                            PLSQLresults = PLSQLresults + "\n" + ErrorPrefix + AddressLineOneError;
                        }
                        if (string.Equals(error, CityError))
                        {
                            PLSQLresults = PLSQLresults + "\n" + ErrorPrefix + CityError;
                        }
                        if (string.Equals(error, CountryError))
                        {
                            PLSQLresults = PLSQLresults + "\n" + ErrorPrefix + CountryError;
                        }
                        if (string.Equals(error, StateOrProvinceError))
                        {
                            PLSQLresults = PLSQLresults + "\n" + ErrorPrefix + StateOrProvinceError;
                        }
                        if (string.Equals(error, MandatoryAttributesPresent))
                        {
                            string FileOutput = MandatoryAttribute[2].ToString() + MandatoryAttribute[3].ToString() + MandatoryAttribute[4].ToString() + MandatoryAttribute[5].ToString() + MandatoryAttribute[6].ToString() + MandatoryAttribute[7].ToString() + MandatoryAttribute[8].ToString() + MandatoryAttribute[9].ToString() + MandatoryAttribute[10].ToString() + MandatoryAttribute[11].ToString() + MandatoryAttribute[12].ToString() + MandatoryAttribute[13].ToString() + MandatoryAttribute[14].ToString() + MandatoryAttribute[15].ToString() + MandatoryAttribute[18].ToString() + MandatoryAttribute[19].ToString() + MandatoryAttribute[20].ToString() + MandatoryAttribute[21].ToString() + MandatoryAttribute[22].ToString();
                            PLSQLresults = PLSQLresults + "\n" + CorrectResultPrefix + FileOutput;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return PLSQLresults;
        }

        /// <summary>
        /// Used to read result set from Oracle DB by passing fileName in the parameter.
        /// </summary>
        /// <param name="fileName">fileName of the file to be tested</param>
        /// <returns>Oracleresults</returns>
        public static string VerifyMandatoryFields(string fileName)
        {
            try
            {
                filePath = StoreFilePath + "\\" + fileName;

                foreach (string line in File.ReadLines(filePath))
                {
                    string value = line.Replace('|', ',');
                    string[] MandatoryAttributeDB = value.ToString().Split(',');
                    // int FieldCount = PP.Length;
                    if (MandatoryAttributeDB.Length == 23)
                    {
                        string PhoneNumber = MandatoryAttributeDB[6].ToString();
                        string AddressLineOne = MandatoryAttributeDB[7].ToString();
                        string City = MandatoryAttributeDB[9].ToString();
                        string StateOrProvince = MandatoryAttributeDB[10].ToString(); ;
                        string Country = MandatoryAttributeDB[13].ToString();

                        string error = PhoneNumber == "" ? PhoneNumError : AddressLineOne == "" ? AddressLineOneError : City == "" ? CityError : StateOrProvince == "" ? StateOrProvinceError : Country == "" ? CountryError : MandatoryAttributesPresent;

                        if (string.Equals(error, PhoneNumError))
                        {

                            string INSQueryPhone = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + PhoneNumError + "'" + ")";

                            OracleReason = "No value found for required attribute PhoneNumber in the message.";
                            WriteToPLSQL(INSQueryPhone);

                            Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }

                        if (string.Equals(error, AddressLineOneError))
                        {
                            string INSQueryAddOne = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + AddressLineOneError + "'" + ")";

                            OracleReason = "No value found for required attribute AddressLineOne in the message.";
                            WriteToPLSQL(INSQueryAddOne);


                            Oracleresults = Oracleresults + "\n" + ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }

                        if (string.Equals(error, CityError))
                        {
                            string INSQueryCity = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + CityError + "'" + ")";

                            OracleReason = "No value found for required attribute City in the message.";
                            WriteToPLSQL(INSQueryCity);

                            Oracleresults = Oracleresults + "\n" + ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }

                        if (string.Equals(error, StateOrProvinceError))
                        {
                            string INSQueryStateOrProvince = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + StateOrProvinceError + "'" + ")";

                            OracleReason = "No value found for required attribute StateOrProvince in the message.";
                            WriteToPLSQL(INSQueryStateOrProvince);

                            Oracleresults = Oracleresults + "\n" + ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }

                        if (string.Equals(error, CountryError))
                        {
                            string INSQueryCountry = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + CountryError + "'" + ")";

                            OracleReason = "No value found for required attribute Country in the message.";
                            WriteToPLSQL(INSQueryCountry);

                            Oracleresults = Oracleresults + "\n" + ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }

                        if (string.Equals(error, MandatoryAttributesPresent))
                        {
                            string values = "";
                            values = "'" + value.Replace(",", "','") + "'";
                            string INSQueryPositive = "INSERT INTO " + StoreSch + "." + "\"" + StoreTbl + "\"" + " VALUES" + "(" + values + ");";

                            WriteToPLSQL(INSQueryPositive);
                            WhereClause = "PhoneNumber=" + "'" + MandatoryAttributeDB[6].ToString() + "'" + " AND " + "AddressLineOne=" + "'" + MandatoryAttributeDB[7].ToString() + "'" + " AND " + "City=" + "'" + MandatoryAttributeDB[9].ToString() + "'" + " AND " + "StateOrProvince=" + "'" + MandatoryAttributeDB[10].ToString() + "'" + " AND " + "Country=" + "'" + MandatoryAttributeDB[13].ToString() + "'";

                            Oracleresults = Oracleresults + "\n" + CorrectResultPrefix + ReadFromOracle(WhereClause);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Oracleresults;
        }


    }
}