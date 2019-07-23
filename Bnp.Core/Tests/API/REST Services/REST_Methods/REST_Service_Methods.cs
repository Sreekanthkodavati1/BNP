using System;
using System.Collections.Generic;
using System.Text;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using BnpBaseFramework.API.Loggers;
using System.Net;
using Bnp.Core.Tests.API.REST_Services.REST_Methods;
using Bnp.Core.Tests.API.REST_Services.REST_Models;
using static Bnp.Core.Tests.API.REST_Services.REST_Models.Member_Model;
using System.Configuration;
using Bnp.Core.Tests.API.Enums;

namespace Bnp.Core.Tests.API.REST_Services
{
    public class REST_Service_Methods
    {
        public Common common;
        public REST_Service_Methods(Common common)
        {
            this.common = common;
        }

        /// <summary>
        /// Following method is to make a rest service call for posting an authentication token.
        /// </summary>
        /// <param></param>
        /// <returns>Json response object of an authentication token</returns>
        public JObject PostAuthToken()
        {
            BaseClass memberdetsails = new BaseClass();
            memberdetsails.body = common.ConvertToJsonBody(new Authtoken_Model());
            memberdetsails.Endpoint = "auth/token";
            Dictionary<String, String> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            memberdetsails.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(memberdetsails);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting an authentication token for Regression
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public JObject PostAuthTokenReg(string clientId,string clientSecret)
        {
            BaseClass memberdetsails = new BaseClass();
            memberdetsails.body = common.ConvertToJsonBody(new Authtoken_Model(clientId,clientSecret));
            memberdetsails.Endpoint = "auth/token";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Accept-Language", "en");
            memberdetsails.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(memberdetsails);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting an authentication token where accept Language is Japanese
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public JObject PostAuthTokenRegJa(string clientId, string clientSecret)
        {
            BaseClass memberdetsails = new BaseClass();
            memberdetsails.body = common.ConvertToJsonBody(new Authtoken_Model(clientId, clientSecret));
            memberdetsails.Endpoint = "auth/token";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Accept-Language", "ja");
            memberdetsails.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(memberdetsails);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting an authentication token without body where Accept language is Japanese
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public JObject PostAuthTokenRegWithoutBodyJa(string clientId, string clientSecret)
        {
            BaseClass memberdetsails = new BaseClass();
            if (string.IsNullOrEmpty(clientId) && string.IsNullOrEmpty(clientSecret))
            {
                memberdetsails.body = "\"\"";
            }
            else
            {
                memberdetsails.body = common.ConvertToJsonBody(new Authtoken_Model(clientId, clientSecret));
            }
            memberdetsails.Endpoint = "auth/token";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Accept-Language", "ja");
            memberdetsails.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(memberdetsails);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting an authentication token without body where Accept language is English
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public JObject PostAuthTokenRegWithoutBodyEN(string clientId, string clientSecret)
        {
            BaseClass memberdetsails = new BaseClass();
            if (string.IsNullOrEmpty(clientId) && string.IsNullOrEmpty(clientSecret))
            {
                memberdetsails.body = "\"\"";
            }
            else
            {
                memberdetsails.body = common.ConvertToJsonBody(new Authtoken_Model(clientId, clientSecret));
            }
            memberdetsails.Endpoint = "auth/token";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Accept-Language", "en");
            memberdetsails.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(memberdetsails);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting an authentication token with invalid language
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public JObject PostAuthTokenWithInvalidLanguage(string clientId, string clientSecret, string language)
        {
            BaseClass memberdetsails = new BaseClass();
            memberdetsails.body = common.ConvertToJsonBody(new Authtoken_Model(clientId, clientSecret));
            memberdetsails.Endpoint = "auth/token";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Accept-Language", language);
            memberdetsails.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(memberdetsails);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting an authentication token in Japanese
        /// </summary>
        /// <returns>Json response object of an authentication token</returns>
        public JObject PostAuthTokenLanguage(string language)
        {
            BaseClass memberdetsails = new BaseClass();
            memberdetsails.body = common.ConvertToJsonBody(new Authtoken_Model());
            memberdetsails.Endpoint = "auth/token";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Accept-Language", language);
            memberdetsails.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(memberdetsails);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for getting an authentication token.
        /// </summary>
        /// <param></param>
        /// <returns>authentication token</returns>
        public String GetAuthToken()
        {
            JObject auth;
            String newvalues;
            string tokenvalue;
            string currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string dpath = currentDirectoryPath.Substring(0, currentDirectoryPath.IndexOf("bin")) + "\\Tests\\API\\DataSource";
            // string directoryPath = @"D:\BNP_Autoamtion\BnPAutomation\Bnp.Core\Tests\API\DataSource";
            if (Directory.GetFiles(dpath).Length == 1)
            {
                String[] values = GetTokenfromcsv().Split(',');
                //Logger.Info(s);
                DateTime date = DateTime.ParseExact(values[0], "yyyyMMddHHmmss", null);
                DateTime now = System.DateTime.UtcNow;
                var difference = now - date;
                if (difference.TotalHours > 23)
                {
                    auth = PostAuthToken();
                    tokenvalue = auth["data"].Value<String>("accessToken");
                    Logger.Info(tokenvalue);
                    newvalues = now.ToString("yyyyMMddHHmmss") + "," + tokenvalue;
                    WriteTokentoCsv(newvalues, 1);
                }
                else
                {
                    tokenvalue = values[1];
                }
            }
            else
            {
                DateTime now = System.DateTime.UtcNow;
                auth = PostAuthToken();
                tokenvalue = auth["data"].Value<String>("accessToken");
                Logger.Info(tokenvalue);
                newvalues = now.ToString("yyyyMMddHHmmss") + "," + tokenvalue;
                WriteTokentoCsv(newvalues, 0);

            }
            return tokenvalue;
        }

        /// <summary>
        /// Following method is to make a rest service call for getting an authentication token from .csv file.
        /// </summary>
        /// <param></param>
        /// <returns>authentication token from .csv file.</returns>
        public string GetTokenfromcsv()
        {
            string tokenvalue = null;
            string currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            String path = currentDirectoryPath.Substring(0, currentDirectoryPath.IndexOf("bin")) + "\\Tests\\API\\DataSource\\AuthorizationToken.csv";
            StreamReader sr = new StreamReader(path);
            sr.ReadLine();
            tokenvalue = sr.ReadLine();
            sr.Close();
            return tokenvalue;
        }
        /// <summary>
		/// Following method is to make a rest service call for getting member rewards.
		/// </summary>
		/// <param name="IpCode">IpCode of the required member is sent as parameter</param>
		/// <returns>Json response object of member rewards.</returns>
		public Object GetMemberRewardsByIpCode(String IpCode)
        {
            HttpWebResponse response_req;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/members/" + IpCode + "/rewards";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                return e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
		/// Following method is to make a rest service call for getting activity summary.
		/// </summary>
		/// <param name="IpCode">IpCode of the required member is sent as parameter</param>
		/// <returns>Json response object of activity summary.</returns>
		public Object GetMemberActivitySummaryByIpCode(string IpCode)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/members/" + IpCode + "/activitySummary";
            Dictionary<string, string> header = new Dictionary<string, string>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("activitySummaryResponseFilter.GetPointsHistory", "true");
            parameter.Add("activitySummaryResponseFilter.GetOtherPointsHistory", "true");
            parameter.Add("activitySummaryResponseFilter.PageNumber", "1");
            parameter.Add("activitySummaryResponseFilter.ResultsPerPage", "1");
            parameter.Add("activitySummaryResponseFilter.additionalAttributes", "brandId");
            //parameter.Add("activitySummaryResponseFilter.additionalAttributes", "employeeId");
            parameter.Add("activitySummaryResponseFilter.orphanPointTypesFilter", "BasePoints");
            parameter.Add("activitySummaryResponseFilter.orphanPointEventsFilter", "AppeasementActivity");
            parameter.Add("activitySummaryResponseFilter.orphanTxnTypesFilter", "3");
            structure.headers = header;
            structure.parameters = parameter;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                response_req = e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

        /// <summary>
        /// Following method is to  authentication token to CSV file.
        /// </summary>
        public void WriteTokentoCsv(String newvalue, int type)
        {
            string currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            String path = currentDirectoryPath.Substring(0, currentDirectoryPath.IndexOf("bin")) + "\\Tests\\API\\DataSource\\AuthorizationToken.csv";
            if (type == 1)
            {
                
                //String path = currentDirectoryPath.Substring(0, currentDirectoryPath.IndexOf("bin")) + "\\Tests\\API\\DataSource\\AuthorizationToken.csv";
                string[] lines = File.ReadAllLines(path);
                lines[1] = newvalue;
                File.WriteAllText(path, String.Join("\n", lines));
            }
            else
            {
                //String path = @"D:\BNP_Autoamtion\BnPAutomation\Bnp.Core\Tests\API\DataSource\AuthorizationToken.csv";
                StringBuilder csvcontent = new StringBuilder();
                csvcontent.AppendLine("Date,Token");
                csvcontent.AppendLine(newvalue);
                File.WriteAllText(path, csvcontent.ToString());
            }
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member.
        /// </summary>
        /// <param></param>
        /// <returns>Json response object of member details.</returns>
        public Object PostMembergeneral()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForREST();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }


        /// <summary>
        /// Following method is to make a rest service call for posting a member.
        /// </summary>
        /// <param></param>
        /// <returns>Json response object of member details.</returns>
        public Object PostMember(Object member)
        {
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(member);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with all fields.
        /// </summary>
        /// <param></param>
        /// <returns>Json response object of member details.</returns>
        public Object PostMembergeneralWithAllFields(Object member)
        {
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(member);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with invalid auth token
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithInvalidAuthToken()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForREST();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + common.RandomString(12));
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with invalid accept language
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithInvalidAcceptLanguage()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForREST();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "English");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with invalid zip code
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithInvalidZipCode()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWithInvalidZipCode();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with invalid Is Employee
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithInvalidIsEmployee()
        {
            BaseClass structure = new BaseClass();
            InValidMember mem = common.GenerateAddMemberForRESTWithInvalidEmployee();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with invalid birthdate
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithInvalidBirthdate()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWithInvalidZipCode();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with invalid phone number
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithInvalidPhoneNumber()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWithInvalidPhoneNumber();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with alternate Ids in quotes
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithAlternateIdsInQuotes()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWhereAlternateIdInQuotes();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with same username and password
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithSameUsernameAndPassword()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWithSameUsernameAndPassword();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member by providing password as number
        /// </summary>
        /// <returns></returns>
        public Object PostMemberByProvidingPasswordWithNumber()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTByProvidingPasswordAsNumber();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member by providing password less than 7 characters
        /// </summary>
        /// <returns></returns>
        public Object PostMemberByProvidingPasswordWithLessThan7Characters()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTByProvidingPasswordLessThan7Chars();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member by providing password only
        /// </summary>
        /// <returns></returns>
        public Object PostMemberByProvidingPasswordOnly()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTByProvidingPasswordOnly();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member by proving username only
        /// </summary>
        /// <returns></returns>
        public Object PostMemberByProvidingUsernameOnly()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTByProvidingUsernameOnly();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with values exceeding maximum characters
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithValuesExceedingMaximumCharacters()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWithValuesExceedingMaximumCharacters();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member where expiration date less than system date
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithWhereExpirationDateLessThanSysDate()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWhereExpirationDateLessThanSysDate();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member where expiration date greater than system date
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithWhereExpirationDateGreaterThanSysDate()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWhereExpirationDateGreaterThanSysDate();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member where expiration date equals system date
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithWhereExpirationDateEqualsToSysDate()
        {
            BaseClass structure = new BaseClass();
            Memberm mem = common.GenerateAddMemberForRESTWhereExpirationDateEqualsToSysDate();
            structure.body = common.ConvertToJsonBody(mem);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with invalid request
        /// </summary>
        /// <returns></returns>
        public Object PostMemberWithInvalidRequest()
        {
            BaseClass structure = new BaseClass();
            structure.body = " ";
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting a member with no VC Key.
        /// </summary>
        /// <param></param>
        /// <returns>Json response object of member details.</returns>
        public Object PostMembergeneralWithNoVCKey(Object member)
        {
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(member);
            structure.Endpoint = "loyalty/members";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        public Object PostMemberByProvidingStoreLocations()
        {
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/members";
            Dictionary<String, String> header = new Dictionary<string, string>();
            Dictionary<String, String> parameter = new Dictionary<string, string>();
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("memberIdentity.storeLocations", "1");
            structure.headers = header;
            structure.parameters = parameter;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting member with mandatory fields.
        /// </summary>
        /// <param></param>
        /// <returns>Json response object of member details with mandatory fields.</returns>
        public Object PostMemberMandatory()
        {
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(common.GenerateAddMemberForRESTMandatory());
            structure.Endpoint = "loyalty/members";
            Dictionary<String, String> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for posting member with existing virtual card.
        /// </summary>
        /// <param></param>
        /// <returns>Json response object of member details with existing virtual card</returns>
        public Object PostMemberExistingVirtualCard()
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(common.GenerateAddMemberForRESTExistingVC());
            structure.Endpoint = "loyalty/members";
            Dictionary<String, String> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            try
            {
                var data = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
                response_req = (JObject)JsonConvert.DeserializeObject(common.ResponseToString(data));
            }
            catch (WebException e)
            {
                response_req = e;
            }
            catch (Exception e)
            {
                response_req = e;
            }
            return response_req;
        }

        /// <summary>
        /// Following method is to make a rest service call for posting transactions with required fields.
        /// </summary>
        /// <param name="VCKey">VCKey of the required member is sent as parameter</param>
        /// <returns>Json response object of transaction details with required fields.</returns>
        public Object PostTransactionWithVCKeyWithRequiredFields(string VCKey)
        {
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(REST_DataGenerator.GenerateTransactionWithRequiredFields(common));
            structure.Endpoint = "loyalty/cards/" + VCKey + "/transactions";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            var response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }



        /// <summary>
        /// Following method is to make a rest service call for posting transactions with all fields.
        /// </summary>
        /// <param name="VCKey">VCKey of the required member is sent as parameter</param>
        /// <returns>Json response object of transaction details with all fields.</returns>
        public Object PostTransactionWithVCKeyWithAllFields(string VCKey)
        {
            HttpWebResponse response_req;
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(REST_DataGenerator.GenerateTransactionWithAllFields(common));
            structure.Endpoint = "loyalty/cards/" + VCKey + "/transactions";
            Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            }
            catch (WebException e)
            {
                var k = e.Response;
                return e;
            }
            catch (Exception e)
            {
                var k = e;
                return e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// the Following method is for updating member through rest service
        /// </summary>
        /// <param name="VCKey">vc key of the member</param>
        /// <returns>request response</returns>
        public Object PatchUpdateMemberwithIpcode(string Ipcode)
        {
            HttpWebResponse response_req;
            BaseClass structure = new BaseClass();
            structure.body = common.ConvertToJsonBody(REST_DataGenerator.GenerateUpdateMemberForREST(common));
            structure.Endpoint = "loyalty/members/" + Ipcode;
            Dictionary<String, String> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().PatchRestRequest(structure);
            }
            catch (WebException e)
            {
                var k = e.Response;
                return e;
            }
            catch (Exception e)
            {
                var k = e;
                return e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for getting member details.
        /// </summary>
        /// <param name="IpCode">IpCode of the required member is sent as parameter</param>
        /// <returns>Json response object of member details.</returns>
        public Object GetMemberDetailsByIpCode(String IpCode)
        {
            HttpWebResponse response_req;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/members/" + IpCode;
            Dictionary<String, String> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/json");
            header.Add("Accept-Language", "en");
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                return e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString(response_req));
        }

        /// <summary>
        /// following method is to make a rest service call for getting account summary.
        /// </summary>
        /// <param name="IpCode">IpCode of the required member is sent as parameter</param>
        /// <returns>Json response object of account summary.</returns>
        public Object GetAccountSummaryByIpCode(string IpCode)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/members/" + IpCode + "/accountSummary";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            structure.headers = header;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                response_req = e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for getting account details.
        /// </summary>
        /// <param name="TransactionId">TransactionId of the required member is sent as parameter</param>
        /// <returns>Json response object of account details.</returns>
        public Object GetAccountDetailsByFMS(string TransactionId)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/members/activityDetail";
            Dictionary<String, String> header = new Dictionary<string, string>();
            Dictionary<String, String> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("transactionid", TransactionId);
            parameter.Add("getPointsHistory", "true");
            parameter.Add("extendedHeaderFields", "itemId,retailAmount");
            parameter.Add("retrieveExpiredTransactions", "true");
            structure.headers = header;
            structure.parameters = parameter;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                response_req = e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

		/// <summary>
		/// Following method is to make a rest service call for getting attribute set of a member.
		/// </summary>
		/// <param name="FirstName">FirstName of the required member is sent as parameter</param>
		/// <returns>Json response object of attribute set.</returns>
		public Object PostMemberAttributeSets(string FirstName)
		{
			Object response_req = null;
			BaseClass structure = new BaseClass();
			structure.Endpoint = "loyalty/attributeSets/memberDetails";
			Dictionary<String, String> header = new Dictionary<string, string>();
			Dictionary<String, String> parameter = new Dictionary<string, string>();
			header.Add("Authorization", "bearer " + GetAuthToken());
			parameter.Add("member.firstName", FirstName);
			structure.body = common.ConvertToJsonBody(REST_DataGenerator.GenerateMemberAttributeSet(common));
			structure.headers = header;
			structure.parameters = parameter;
			try
			{
				response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
			}
			catch (WebException e)
			{
				response_req = (HttpWebResponse)e.Response;
			}
			catch (Exception e)
			{
				var k = e;
				response_req = e;
			}
			return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
		}

		public string Get_MP_Url
        {
            get
            {
                return AppURL(ConfigurationManager.AppSettings["MemberPortal"]);
            }
        }

        public string Get_CS_Url
        {
            get
            {
                return AppURL(ConfigurationManager.AppSettings["CSPortal"]);
            }
        }

        public string Get_API_Url
        {
            get
            {
                return AppURL(ConfigurationManager.AppSettings["CDIS"]);
            }
        }

        public string Get_REST_Url
        {
            get
            {
                return AppURL(ConfigurationManager.AppSettings["REST"]);
            }
        }

        public string Get_LN_Url
        {
            get
            {
                return AppURL(ConfigurationManager.AppSettings["LoyaltyNavigator"]);
            }
        }

        public string AppURL(string appType)
        {
            string URL = null;
            JObject response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "api/ApplicationURL";
            Dictionary<String, String> parameters = new Dictionary<string, string>();
            string env = ConfigurationManager.AppSettings["Environment"];
            string orgName = ConfigurationManager.AppSettings["Organization"];

            if (env.Contains("Dev"))
            {
                env = "Dev";
            }
            //header.Add("Content-Type", "application/json");
            parameters.Add("Client", orgName);
            parameters.Add("&env", env);
            parameters.Add("&application", appType);
            structure.parameters = parameters;
            if (appType.Equals("LN"))
            {
                URL = ConfigurationManager.AppSettings["Navigator_url"];
                return URL;
            }
            else
            {
                try
                {
                    var data = (HttpWebResponse)new BaseLibrary().GetAPPURLRequest(structure);
                    response_req = (JObject)JsonConvert.DeserializeObject(common.ResponseToString(data));
                    URL = response_req.Value<String>("Items").ToString();
                }
                catch (Exception e)
                {
                    e.GetBaseException();
                }
                return URL;
            }
        }

        /// <summary>
		/// This method is to retrieve all rewards in rewards definition
		/// </summary>
		/// <returns>Json response object of rewards in rewards definition</returns>
		public Object GetAllRewardsInRewardsDefinition()
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/rewards";
            Dictionary<String, String> header = new Dictionary<string, string>();
            Dictionary<String, String> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("rewardIdentity.activeOnly", "true");
            parameter.Add("responseFilter.pageNumber", "1");
            parameter.Add("responseFilter.resultsPerPage", "10");
            parameter.Add("rewardIdentity.storeLocations", "1");
            structure.headers = header;
            structure.parameters = parameter;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                response_req = e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

        /// <summary>
		/// Following method is to make a rest service call for adding member to targeted promotion.
		/// </summary>
		/// <param name="FirstName">FirstName of the required member is sent as parameter</param>
		/// <returns>Json response object of Targeted Promotion.</returns>
		public Object PostMemberToTargetedPromotion(string FirstName, string PromotionCode)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.body = string.Empty;
            structure.Endpoint = "loyalty/memberPromotions";
            Dictionary<string, string> header = new Dictionary<string, string>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("member.firstName", FirstName);
            parameter.Add("options.promotionCode", PromotionCode);
            structure.headers = header;
            structure.parameters = parameter;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                response_req = e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for getting member promotions.
        /// </summary>
        /// <param name="FirstName">FirstName of the required member is sent as parameter</param>
        /// <returns>Json response object of member promotions</returns>
        public Object GetMemberPromotions(String firstName)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/memberPromotions";
            Dictionary<string, string> header = new Dictionary<string, string>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("member.firstName", firstName);
            structure.headers = header;
            structure.parameters = parameter;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                return e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }
        /// <summary>
        /// Following method is to make a rest service call for enrolling member to targeted promotion.
        /// </summary>
        /// <param name="FirstName">FirstName of the required member is sent as parameter</param>
        /// <param name="PromotionCode">PromotionCode of the required member is sent as parameter</param>
        /// <returns>Json response object of Targeted Promotion.</returns>
        public Object EnrollMemberToTargetedPromotion(string FirstName, string PromotionCode)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.body = string.Empty;
            structure.Endpoint = "loyalty/memberPromotions/enroll";
            Dictionary<string, string> header = new Dictionary<string, string>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("member.firstName", FirstName);
            parameter.Add("options.promotionCode", PromotionCode);
            structure.headers = header;
            structure.parameters = parameter;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().PostRestRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                response_req = e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call for getting member rewards.
        /// </summary>
        /// <param name="identifier">FirstName of the required member is sent as parameter</param>
        /// <returns>Json response object of member rewards</returns>
        public Object GetMemberRewardsByFMS(MemberSearchIdentity type, string identifier)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/memberrewards";
            Dictionary<string, string> header = new Dictionary<string, string>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());

            if (type.ToString().ToLower().Equals("firstname"))
            {
                parameter.Add("firstname", identifier);
            }
            else if (type.ToString().ToLower().Equals("cardid"))
            {
                parameter.Add("number", identifier);
            }
            else
            {
                parameter.Add("email", identifier);
            }

            structure.headers = header;
            structure.parameters = parameter;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().GetRESTRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                return e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call to redeem member rewards.
        /// </summary>
        /// <param name="RewardId">RewardId of the required member is sent as a parameter</param>
        /// <returns>Json response object of redeemed member reward</returns>
        public Object PatchRedeemMemberRewardByUsingRewardId(string RewardId)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/memberRewards/" + RewardId + "/redeem";
            Dictionary<String, String> header = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());

            structure.headers = header;
            structure.body = string.Empty;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().PatchRestRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                response_req = e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }

        /// <summary>
        /// Following method is to make a rest service call to redeem member rewards using Certificate Number.
        /// </summary>
        /// <param name="CertificateNumber">CertificateNumber of the required member is sent as parameter</param>
        /// <returns>Json response object of redeemed member reward</returns>
        public Object PatchRedeemMemberRewardUsingCertificateNumber(string certificateNumber)
        {
            Object response_req = null;
            BaseClass structure = new BaseClass();
            structure.Endpoint = "loyalty/memberRewards/redeem";
            Dictionary<string, string> header = new Dictionary<string, string>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            header.Add("Authorization", "bearer " + GetAuthToken());
            parameter.Add("rewardidentity.certificateNumber", certificateNumber);
            structure.headers = header;
            structure.parameters = parameter;
            structure.body = string.Empty;
            try
            {
                response_req = (HttpWebResponse)new BaseLibrary().PatchRestRequest(structure);
            }
            catch (WebException e)
            {
                response_req = (HttpWebResponse)e.Response;
            }
            catch (Exception e)
            {
                var k = e;
                return e;
            }
            return (JObject)JsonConvert.DeserializeObject(common.ResponseToString((HttpWebResponse)response_req));
        }
    }
}
