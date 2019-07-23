using System;
using System.IO;
using System.Configuration;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using BnPBaseFramework.IO.FileFunctions_Common;


namespace Bnp.Core.Tests.IO.FileMethods
{


    public static class FileMethods_Enrollment
    {
        public static string MemberTbl = ConfigurationManager.AppSettings["MemberTable"];
        public static string LibJobTbl = ConfigurationManager.AppSettings["LibJob"];
        public static string LibmsglogTbl = ConfigurationManager.AppSettings["LibMSGLog"];
        public static string SELQuery = null;
        public static string libJobQuery = null;
        public static string libMsgQuery = null;
        public static string JobNumber = null;
        public static string Reason = null;
        public static string OracleReason = null;
        public static string filePath = null;
        public static string StoreSch = ConfigurationManager.AppSettings["QADBSchema"];
        public static string EnrollmentFilePath = ConfigurationManager.AppSettings["EnrollmntFilePth"];
        public static OracleDataReader ORCDR;
        public static NpgsqlDataReader NpgsqlDR;
        public static string Oracleresults = null;
        public static string PLSQLresults = null;
        public static string LoyalityIDError = "Empty Member/VirtualCard/LoyaltyIdNumber in input Xml for Member lookup.";
        public static string AlternateIDError = "Empty Member/AlternateId in input Xml for Member lookup.";
        public static string UserNameErrorFirstLine = "Value cannot be null.";
        public static string UserNameErrorSecondLine = "Parameter name: member.Username";
        public static string UserNameError = UserNameErrorFirstLine + "\r\n" + UserNameErrorSecondLine;
        public static string PrimaryEmailAddress = null;
        public static string EmailError = "Found existing member with same primary email = ";
        public static string MandatoryAttributesPresent = "No Empty string";
        public static string WhereClause = null;
        public static bool EmailExistsinPLSQL = false;
        public static bool EmailExistsinOracle = false;
        public static string ErrorPrefix = "This error message is from lw_libmessagelog table. ";
        public static string CorrectResultPrefix = "This result set is from lw_loyaltymember table. ";


        /// <summary>
        /// Used to read result set from OracleDataReader in Oracle DB by passing WhereClause in the parameter
        /// </summary>
        /// <param name="WhereClause">WhereClause as a part of SQL query to be executed through OracleDataReader</param>
        /// <returns>Oracleresults</returns>
        public static string ReadFromOracle(string WhereClause)
        {
            try
            {
                SELQuery = "SELECT * FROM " + MemberTbl + " WHERE " + WhereClause;
                ORCDR = DataReader.OracleDataReader(SELQuery);
                if (ORCDR.HasRows)
                {
                    while (ORCDR.Read())
                    {

                        Oracleresults = ORCDR[8].ToString() + ORCDR[9].ToString() + ORCDR[10].ToString() + ORCDR[11].ToString() + ORCDR[12].ToString() + ORCDR[13].ToString() + ORCDR[14].ToString() + ORCDR[15].ToString() + ORCDR[18].ToString() + ORCDR[19].ToString() + ORCDR[20].ToString() + ORCDR[21].ToString();
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

                    }
                    ORCDR.Close();
                }

                libMsgQuery = "SELECT REASON FROM " + LibmsglogTbl + " WHERE JOBNUMBER = " + JobNumber + "AND REASON like '" + OracleReason + "%'";
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

        public static string ReadFromPLSQL(string WhereClause)
        {
            try
            {
                SELQuery = "SELECT * FROM " + StoreSch + "." + MemberTbl + " WHERE " + WhereClause;
                Console.WriteLine("SelQuery is", SELQuery);
                DataReader.PLSQLDataReader2(SELQuery);
                if (NpgsqlDR.HasRows)
                {
                    while (NpgsqlDR.Read())
                    {

                        PLSQLresults = NpgsqlDR[10].ToString();
                    }
                    NpgsqlDR.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return PLSQLresults;
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
                filePath = EnrollmentFilePath + "\\" + fileName;
                foreach (string line in File.ReadLines(filePath))
                {
                    string value = line.Replace('|', ',');
                    string[] MandatoryAttribute = value.ToString().Split(',');

                    if (MandatoryAttribute.Length == 38)
                    {
                        string LoyalityID = MandatoryAttribute[0].ToString();
                        string AlternateID = MandatoryAttribute[7].ToString();
                        string UserName = MandatoryAttribute[8].ToString();
                        string error = LoyalityID == "" ? LoyalityIDError : AlternateID == "" ? AlternateIDError : UserName == "" ? UserNameError : MandatoryAttributesPresent;
                        if (string.Equals(error, LoyalityIDError))
                        {
                            PLSQLresults = ErrorPrefix + LoyalityIDError;
                        }
                        if (string.Equals(error, AlternateIDError))
                        {
                            PLSQLresults = PLSQLresults + "\n" + ErrorPrefix + AlternateIDError;
                        }
                        if (string.Equals(error, UserNameError))
                        {
                            PLSQLresults = PLSQLresults + "\n" + ErrorPrefix + UserNameError;

                        }


                        if (string.Equals(error, MandatoryAttributesPresent))
                        {
                            string PrimaryEmailAddress = MandatoryAttribute[10].ToString();

                            if (PrimaryEmailAddress == "")
                            {
                                string FileOutput = MandatoryAttribute[4].ToString() + MandatoryAttribute[1].ToString() + MandatoryAttribute[3].ToString() + MandatoryAttribute[2].ToString() + MandatoryAttribute[5].ToString() + MandatoryAttribute[6].ToString() + MandatoryAttribute[7].ToString() + MandatoryAttribute[8].ToString() + MandatoryAttribute[10].ToString() + MandatoryAttribute[11].ToString() + MandatoryAttribute[12].ToString() + MandatoryAttribute[35].ToString();
                                PLSQLresults = PLSQLresults + "\n" + CorrectResultPrefix + FileOutput;
                            }

                            else
                            {
                                EmailExistsinPLSQL = EmailAlreadyExistsinPLSQL(PrimaryEmailAddress);
                                if (EmailExistsinPLSQL == true)
                                {
                                    string EmailError = Emailerror(PrimaryEmailAddress);
                                    string FileOutput = MandatoryAttribute[4].ToString() + MandatoryAttribute[1].ToString() + MandatoryAttribute[3].ToString() + MandatoryAttribute[2].ToString() + MandatoryAttribute[5].ToString() + MandatoryAttribute[6].ToString() + MandatoryAttribute[7].ToString() + MandatoryAttribute[8].ToString() + MandatoryAttribute[10].ToString() + MandatoryAttribute[11].ToString() + MandatoryAttribute[12].ToString() + MandatoryAttribute[35].ToString();
                                    PLSQLresults = PLSQLresults + "\n" + CorrectResultPrefix + EmailError;

                                }
                                else
                                {
                                    string FileOutpt = MandatoryAttribute[4].ToString() + MandatoryAttribute[1].ToString() + MandatoryAttribute[3].ToString() + MandatoryAttribute[2].ToString() + MandatoryAttribute[5].ToString() + MandatoryAttribute[6].ToString() + MandatoryAttribute[7].ToString() + MandatoryAttribute[8].ToString() + MandatoryAttribute[10].ToString() + MandatoryAttribute[11].ToString() + MandatoryAttribute[12].ToString() + MandatoryAttribute[35].ToString();
                                    PLSQLresults = PLSQLresults + "\n" + CorrectResultPrefix + FileOutpt;
                                }

                            }
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
                filePath = EnrollmentFilePath + "\\" + fileName;

                foreach (string line in File.ReadLines(filePath))
                {
                    string value = line.Replace('|', ',');
                    string[] MandatoryAttributeDB = value.ToString().Split(',');

                    if (MandatoryAttributeDB.Length == 38)
                    {

                        string LoyalityId = MandatoryAttributeDB[0].ToString();
                        string FirstName = MandatoryAttributeDB[1].ToString();
                        string MiddleName = MandatoryAttributeDB[2].ToString();
                        string LastName = MandatoryAttributeDB[3].ToString();
                        string BirthDate = MandatoryAttributeDB[4].ToString();
                        string AlternateId = MandatoryAttributeDB[7].ToString();
                        string UserName = MandatoryAttributeDB[8].ToString();
                        string Password = MandatoryAttributeDB[9].ToString();
                        string PrimaryEmailAddress = MandatoryAttributeDB[10].ToString();
                        string PrimaryPhoneNumber = MandatoryAttributeDB[11].ToString();
                        string PrimaryPostalCode = MandatoryAttributeDB[12].ToString();
                        string AddressLineOne = MandatoryAttributeDB[13].ToString();
                        string City = MandatoryAttributeDB[17].ToString();
                        string StateOrProvince = MandatoryAttributeDB[18].ToString();
                        string ZipOrPostalCode = MandatoryAttributeDB[19].ToString();
                        string County = MandatoryAttributeDB[20].ToString();
                        string Country = MandatoryAttributeDB[21].ToString();
                        string Gender = MandatoryAttributeDB[33].ToString();

                        string error = LoyalityId == "" ? LoyalityIDError : AlternateId == "" ? AlternateIDError : UserName == "" ? UserNameError : MandatoryAttributesPresent;

                        if (string.Equals(error, LoyalityIDError))
                        {

                            string INSQueryLoyaltyID = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + LoyalityIDError + "'" + ")";

                            OracleReason = "Empty Member/VirtualCard/LoyaltyIdNumber in input Xml for Member";
                            WriteToPLSQL(INSQueryLoyaltyID);

                            Oracleresults = ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }

                        if (string.Equals(error, AlternateIDError))
                        {
                            string INSQueryAlternateID = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + AlternateIDError + "'" + ")";

                            OracleReason = "Empty Member/AlternateId in input Xml for Member lookup.";
                            WriteToPLSQL(INSQueryAlternateID);


                            Oracleresults = Oracleresults + "\n" + ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }

                        if (string.Equals(error, UserNameError))
                        {
                            string INSQueryUserName = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + UserNameError + "'" + ")";

                            OracleReason = "Value cannot be null.";
                            WriteToPLSQL(INSQueryUserName);

                            Oracleresults = Oracleresults + "\n" + ErrorPrefix + ReadReasonFromOracle(OracleReason);

                        }


                        if (string.Equals(error, MandatoryAttributesPresent))
                        {
                            if (PrimaryEmailAddress == "")
                            {
                                string values = "";
                                string Columns = "(" + "\"LoyaltyId\"" + "," + "\"FirstName\"" + "," + "\"MiddleName\"" + "," + "\"LastName\"" + "," + "\"BirthDate\"" + "," + "\"AlternateId\"" + "," + "\"UserName\"" + "," + "\"Password\"" + "," + "\"PrimaryEmailAddress\"" + "," + "\"PrimaryPhoneNumber\"" + "," + "\"PrimaryPostalCode\"" + "," + "\"AddressLineOne\"" + "," + "\"City\"" + "," + "\"StateOrProvince\"" + "," + "\"ZipOrPostalCode\"" + "," + "\"County\"" + "," + "\"Country\"" + "," + "\"Gender\"" + ")";
                                value = LoyalityId + "," + FirstName + "," + MiddleName + "," + LastName + "," + BirthDate + "," + AlternateId + "," + UserName + "," + Password + "," + PrimaryEmailAddress + "," + PrimaryPhoneNumber + "," + PrimaryPostalCode + "," + AddressLineOne + "," + City + "," + StateOrProvince + "," + ZipOrPostalCode + "," + County + "," + Country + "," + Gender;
                                values = "'" + value.Replace(",", "','") + "'";

                                string INSQueryPositive = "INSERT INTO " + StoreSch + "." + "\"" + MemberTbl + "\"" + Columns + " VALUES" + "(" + values + ")";
                                WriteToPLSQL(INSQueryPositive);
                                WhereClause = "AlternateId=" + "'" + MandatoryAttributeDB[7].ToString() + "'" + " AND " + "UserName=" + "'" + MandatoryAttributeDB[8].ToString() + "'";

                                Oracleresults = Oracleresults + "\n" + CorrectResultPrefix + ReadFromOracle(WhereClause);
                            }
                            else
                            {
                                EmailExistsinOracle = EmailAlreadyExistsinOracle(PrimaryEmailAddress);
                                if (EmailExistsinOracle == true)
                                {

                                    string EmailError = Emailerror(PrimaryEmailAddress);
                                    string INSQueryEmail = "INSERT INTO " + StoreSch + "." + LibmsglogTbl + " VALUES(" + "'" + EmailError + "'" + ")";
                                    WriteToPLSQL(INSQueryEmail);
                                    OracleReason = EmailError;

                                    Oracleresults = Oracleresults + "\n" + ErrorPrefix + ReadReasonFromOracle(OracleReason);
                                }

                                else
                                {
                                    string values = "";
                                    string Columns = "(" + "\"LoyaltyId\"" + "," + "\"FirstName\"" + "," + "\"MiddleName\"" + "," + "\"LastName\"" + "," + "\"BirthDate\"" + "," + "\"AlternateId\"" + "," + "\"UserName\"" + "," + "\"Password\"" + "," + "\"PrimaryEmailAddress\"" + "," + "\"PrimaryPhoneNumber\"" + "," + "\"PrimaryPostalCode\"" + "," + "\"AddressLineOne\"" + "," + "\"City\"" + "," + "\"StateOrProvince\"" + "," + "\"ZipOrPostalCode\"" + "," + "\"County\"" + "," + "\"Country\"" + "," + "\"Gender\"" + ")";
                                    value = LoyalityId + "," + FirstName + "," + MiddleName + "," + LastName + "," + BirthDate + "," + AlternateId + "," + UserName + "," + Password + "," + PrimaryEmailAddress + "," + PrimaryPhoneNumber + "," + PrimaryPostalCode + "," + AddressLineOne + "," + City + "," + StateOrProvince + "," + ZipOrPostalCode + "," + County + "," + Country + "," + Gender;
                                    values = "'" + value.Replace(",", "','") + "'";

                                    string INSQueryPositive = "INSERT INTO " + StoreSch + "." + "\"" + MemberTbl + "\"" + Columns + " VALUES" + "(" + values + ")";
                                    WriteToPLSQL(INSQueryPositive);
                                    WhereClause = "AlternateId=" + "'" + MandatoryAttributeDB[7].ToString() + "'" + " AND " + "UserName=" + "'" + MandatoryAttributeDB[8].ToString() + "'";
                                    string OracleOutput = ReadFromOracle(WhereClause);

                                    Oracleresults = Oracleresults + "\n" + CorrectResultPrefix + OracleOutput;
                                }

                            }
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



        /// <summary>
        /// Used to retrieve emailerror 
        /// </summary>
        /// <param name="PrimaryEmailAddress">Value of primary EmailAddress from input file</param>
        /// <returns>EmaiEerror</returns>
        public static string Emailerror(string PrimaryEmailAddress)
        {
            string EmailError = "Found existing member with same primary email = " + PrimaryEmailAddress + ".";
            return EmailError;
        }

        /// <summary>
        /// Used to check whether PrimaryEmailAddress is already present in PLSQL DB
        /// </summary>
        /// <param name="PrimaryEmailAddress">Value of primary EmailAddress from input file</param>
        /// <returns>true/false</returns>
        public static bool EmailAlreadyExistsinPLSQL(string PrimaryEmailAddress)
        {
            WhereClause = "\"PrimaryEmailAddress\"" + " = " + "'" + PrimaryEmailAddress + "'";
            SELQuery = "SELECT * FROM " + StoreSch + "." + MemberTbl + " WHERE " + WhereClause;
            Console.WriteLine(SELQuery);

            NpgsqlDataReader PLSQLDataReadr = DataReader.PLSQLDataReader2(SELQuery);
            if (PLSQLDataReadr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Used to check whether PrimaryEmailAddress is already present in Oracle DB
        /// </summary>
        /// <param name="PrimaryEmailAddress">Value of primary EmailAddress from input file</param>
        /// <returns>true/false</returns>
        public static bool EmailAlreadyExistsinOracle(string PrimaryEmailAddress)
        {
            WhereClause = "PrimaryEmailAddress =" + "'" + PrimaryEmailAddress + "'";
            SELQuery = "Select * FROM " + MemberTbl + " WHERE " + WhereClause;
            Console.WriteLine(SELQuery);
            OracleDataReader ORCLDataReadr = DataReader.OracleDataReader(SELQuery);
            if (ORCLDataReadr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }


        }




    }
}
