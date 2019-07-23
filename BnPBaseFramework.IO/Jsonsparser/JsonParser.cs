using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;

namespace JsonParser
{
    public static class JsonFileProcessor
    {
        static object locker = new object();
        public static string OracleConnection = ConfigurationManager.AppSettings["OracleConn"];

        /// <summary>
        /// method to generate random input data based on json template
        /// </summary>
        /// <param name="typeOfFile"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static string GenerateInputData(TypeOfFile typeOfFile, string templateName)

        {

            string schemaText = string.Empty;

            string result = "Error";

            StringBuilder jsonSchemaPath = new StringBuilder();

            string inputDataPath = string.Empty;



            try

            {

                if (ConfigurationManager.AppSettings["JsonSchema"] != null)

                {

                    jsonSchemaPath.Append(ConfigurationManager.AppSettings["JsonSchema"]);

                    jsonSchemaPath.Append(@"\");

                    jsonSchemaPath.Append(typeOfFile.ToString() + @"\");

                    jsonSchemaPath.Append(typeOfFile.ToString() + @"_JSONTemplates");



                    if (jsonSchemaPath != null)

                    {

                        inputDataPath = jsonSchemaPath.ToString().Replace("JSONTemplates", "InputTextFiles");
                        jsonSchemaPath.Append(@"\");
                        jsonSchemaPath.Append(templateName);
                        jsonSchemaPath.Append(".JSON");
                        string outPutFilePath = inputDataPath + "\\" + templateName + ".txt";
                        try

                        {
                            if (File.Exists(outPutFilePath))
                            {
                                File.Delete(outPutFilePath);
                            }

                        }

                        catch (FileNotFoundException ex)

                        {

                            result = ex.Message;

                        }

                        using (var r = new StreamReader(jsonSchemaPath.ToString()))

                        {

                            schemaText = r.ReadToEnd();

                        }

                        dynamic dynObj = JsonConvert.DeserializeObject(schemaText);

                        foreach (var data in dynObj.Files)

                        {

                            foreach (var data1 in data.TypeOfFiles)

                            {

                                JObject dObject = data1;
                                var delimiter = Convert.ToString(dObject["Delimiter"]);
                                var noOfRows = Convert.ToInt32(dObject["No_Of_Rows"]);
                                StringBuilder inputDataStore = null;
                                DataTable dataTable = null;
                                int loyalCount = 0;
                                while (noOfRows > 0)

                                {

                                    int propertiesCount = 2;
                                    inputDataStore = new StringBuilder();
                                    foreach (var item in dObject.Properties())
                                    {

                                        var attributeName = item.Name;

                                        string[] chkList = { "Delimiter", "No_Of_Rows", "Directive" };

                                        if (attributeName == "Directive")

                                        {

                                            var directiveItem = item.Value.Last;

                                            var directive = Convert.ToString(((Newtonsoft.Json.Linq.JValue)directiveItem.First).Value);

                                            inputDataStore.Append(directive);

                                            inputDataStore.Append(delimiter);

                                        }

                                        else if (Array.IndexOf(chkList, attributeName) < 0)

                                        {

                                            var first = item.Value.First;

                                            var last = item.Value.Last;

                                            var firstValue = Convert.ToString(((Newtonsoft.Json.Linq.JValue)first.First).Value);

                                            var lastValue = Convert.ToString(((Newtonsoft.Json.Linq.JValue)last.First).Value);

                                            int startIndex = firstValue.IndexOf('[');

                                            int endIndex = firstValue.IndexOf(']');

                                            var attributeType = firstValue.Substring(0, startIndex).ToUpper();

                                            var attributeRange = firstValue.Substring(startIndex + 1, endIndex - startIndex - 1);

                                            var attributeRequired = lastValue.Replace("{", "").Replace("}", "").ToUpper();



                                            int length = 0;

                                            uint minLength = 0, maxLength = 0;

                                            string regexExp = string.Empty;
                                            if (attributeType == "STRING" && attributeRange.Contains("-"))
                                            {
                                                minLength = Convert.ToUInt16(attributeRange.Split('-')[0]);
                                                maxLength = Convert.ToUInt16(attributeRange.Split('-')[1]);
                                            }

                                            else if (attributeType == "INT")
                                            {

                                                length = Convert.ToInt32(attributeRange);
                                            }
                                            else if (attributeType == "DATABASEVALUE")
                                            {
                                                //if (loyalCount == 0)
                                                //{
                                                dataTable = GetDBValue(attributeRange.Split('-')[0], attributeName, attributeRange.Split('-')[1], null);
                                                //}
                                            }
                                            else if (attributeType == "REGULAREXP")
                                            {
                                                string parse = @firstValue.Replace("RegularExp[", "");
                                                regexExp = parse.Substring(0, parse.Length - 1).Replace("\"", @"\");

                                            }
                                            else if (attributeType == "STRING" && !attributeRange.Contains('-'))

                                            {

                                                minLength = Convert.ToUInt32(attributeRange);

                                            }



                                            switch (attributeType)
                                            {

                                                #region  Integer
                                                case "INT":
                                                    if (attributeRequired == "TRUE")
                                                    {

                                                        switch (attributeName)

                                                        {

                                                            case "Status":

                                                                {

                                                                    inputDataStore.Append((int)Eric.Morrison.RandomValue.GetRandomEnumValue<Status>());

                                                                }

                                                                break;

                                                            case "StoreType":
                                                                {

                                                                    inputDataStore.Append((int)Eric.Morrison.RandomValue.GetRandomEnumValue<StoreType>());

                                                                }


                                                                break;
                                                            default:
                                                                {

                                                                    inputDataStore.Append(RandomNumber(length));

                                                                }

                                                                break;

                                                        }



                                                    }

                                                    break;
                                                #endregion
                                                #region RegularExpressionValidator
                                                case "REGULAREXP":
                                                    {
                                                        if (attributeRequired == "TRUE")
                                                        {
                                                            if (Regex.IsMatch(CreateEmail(), regexExp, RegexOptions.IgnoreCase))
                                                                inputDataStore.Append(CreateEmail());
                                                        }
                                                        break;

                                                    }

                                                #endregion
                                                #region string
                                                case "STRING":

                                                    if (attributeRequired == "TRUE")

                                                    {



                                                        if (maxLength == 0)

                                                        {

                                                            switch (attributeName)

                                                            {

                                                                case "Password":

                                                                    {

                                                                        inputDataStore.Append(CreatePassword(minLength));

                                                                        break;

                                                                    }




                                                                default:

                                                                    {

                                                                        inputDataStore.Append(Eric.Morrison.RandomString.Get(minLength, true));

                                                                        break;

                                                                    }

                                                            }

                                                        }

                                                        else

                                                        {

                                                            inputDataStore.Append(Eric.Morrison.RandomString.Get(minLength, maxLength, true));

                                                        }

                                                    }

                                                    break;
                                                #endregion
                                                #region Date
                                                case "DATE":

                                                    if (attributeRequired == "TRUE")

                                                    {

                                                        DateTime dateTime = System.DateTime.Now.AddDays(-1);

                                                        inputDataStore.Append(dateTime.ToString("dd-MMM-yy HH.mm.ss tt"));
                                                    }

                                                    break;
                                                #endregion
                                                #region Double

                                                case "DOUBLE":
                                                    {
                                                        if (attributeRequired == "TRUE")
                                                        {
                                                            System.Random random = new System.Random();

                                                            inputDataStore.Append(Math.Round(random.NextDouble(), 2).ToString("0.00"));
                                                        }

                                                    }

                                                    break;
                                                #endregion
                                                #region DbValue
                                                case "DATABASEVALUE":
                                                    {
                                                        if (attributeRequired == "TRUE")
                                                        {
                                                            if ((attributeRange.Split('-')[1]) == "ALL")
                                                            {
                                                                string number = (RandomNumber(10));
                                                                DataTable dtTxnHeaderId = GetDBValue(attributeRange.Split('-')[0], attributeName, attributeRange.Split('-')[1], number);

                                                                while (Convert.ToInt32(dtTxnHeaderId.Rows[0][0]) == 0)
                                                                {
                                                                    inputDataStore.Append(number);
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                inputDataStore.Append(dataTable.Rows[loyalCount][0]);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                    #endregion
                                            }

                                            if (propertiesCount - 1 < dObject.Properties().Count())

                                            {

                                                inputDataStore.Append(delimiter);

                                            }

                                        }

                                        propertiesCount++;

                                    }

                                    System.IO.File.AppendAllText(outPutFilePath, inputDataStore.ToString() + Environment.NewLine);

                                    result = "Success";

                                    noOfRows--;
                                    loyalCount++;

                                }

                            }

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                result = ex.Message;

            }

            return result;

        }

        /// <summary>
        /// method to generate random password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreatePassword(uint length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@!$#%^&*()?><{}|";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, Convert.ToInt32(length))
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// method to generate random email
        /// </summary>
        /// <returns></returns>
        public static string CreateEmail()

        {

            string address = string.Empty;

            address = string.Format("qa{0}@test.com", RadomString(Convert.ToInt32(10)));

            return address;

        }

        /// <summary>
        /// method to generate random string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RadomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*private static FileInfo[] GetFiles(string inPutJsonschemaPath)
        {
            FileInfo[] inPutJsonschemafiles = null;
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(inPutJsonschemaPath);
                inPutJsonschemafiles = dirInfo.GetFiles("*.Json");
            }
            catch (IOException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inPutJsonschemafiles;
        }*/


        /// <summary>
        ///method to generate random number
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumber(int length)
        {
            try
            {
                lock (locker)
                {
                    Thread.Sleep(100);
                    string number = DateTime.Now.ToString("yyyyMMddHHmmssf");
                    number = number.Substring(number.Length - length, length);
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(number);
                    if (stringBuilder.ToString().StartsWith("0"))
                    {
                        stringBuilder[0] = '1';
                    }
                    return stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// function to get database values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="noOfRows"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable GetDBValue(string tableName, string columnName, string noOfRows, string value)
        {
            DataTable dataTable = null;
            string query = string.Empty;
            try
            {
                if (noOfRows == "ALL")
                {
                    query = "select count(*) from " + tableName + " where " + columnName + "='" + value + "'";
                }
                else if (tableName.ToLower() == "lw_virtualcard")
                {
                    query = "select " + columnName + " from(select *from " + tableName + "  )  where rownum<=" + noOfRows + " and " + " Status = 1";
                }
                else
                {
                    query = "select " + columnName + " from(select *from " + tableName + "  )  where rownum<=" + noOfRows;
                }
                OracleConnection oracleConn = new OracleConnection(OracleConnection);
                oracleConn.Open();
                OracleCommand orclCommand = new OracleCommand(query, oracleConn);
                orclCommand.CommandType = CommandType.Text;
                OracleDataAdapter dataAdapter = new OracleDataAdapter(orclCommand);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataTable = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
            }

            return dataTable;
        }

    }

    public enum Status
    {
        zero = 0,
        one = 1
    };

    public enum StoreType
    {
        zero = 0,
        one = 1,
        two = 2
    };

    public enum TypeOfFile
    {
        Stores,
        Product,
        CouponCode,
        HistoryTransaction,
        Stage,
        Enrollment,
        Tlog
    }
}

