using BnpBaseFramework.API.Enums;
using BnpBaseFramework.API.Loggers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace BnpBaseFramework.API.Utils
{
	public class BaseLibrary
	{
		public string ContentType { get; set; }
		public String url;

		
        /// <summary>
        /// This Method makes the GET Request
        /// </summary>
        /// <param name="instance">Base Class Object</param>
        /// <returns>response of the request</returns>
		public HttpWebResponse GetRESTRequest(BaseClass instance)
		{
			url = ConfigurationManager.AppSettings["BaseRestURL"];
			ContentType = "application/JSON";
			Logger.Info("Get Request method started");
			String uri = CompleteUrl(url, instance.Endpoint, instance.parameters);
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = Verbs.Get.ToString();
			request.ContentLength = 0;
			request.ContentType = ContentType;
			ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
			if (instance.headers != null)
			{
				foreach (var k in instance.headers)
				{
					request.Headers.Add(k.Key, k.Value);
				}
			}
			var response = (HttpWebResponse)request.GetResponse();
			var responseValue = string.Empty;
			if (response.StatusCode != HttpStatusCode.OK)
			{
				var message = String.Format("Failed: Received HTTP {0}", response.StatusCode);
				throw new Exception(message);
			}
			return response;
		}
        public HttpWebResponse GetJiraRESTRequest(BaseClass instance)
        {
            url = "https://xray.cloud.xpand-it.com/api/v1/";
            ContentType = "application/JSON";
            Logger.Info("Get Request method started");
            String uri = CompleteUrl(url, instance.Endpoint, instance.parameters);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = Verbs.Get.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            if (instance.headers != null)
            {
                foreach (var k in instance.headers)
                {
                    request.Headers.Add(k.Key, k.Value);
                }
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseValue = string.Empty;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = String.Format("Failed: Received HTTP {0}", response.StatusCode);
                throw new Exception(message);
            }
            return response;
        }
        /// <summary>
        /// this method is to make the Post request
        /// </summary>
        /// <param name="instance">Base clas object</param>
        /// <returns>response of the service</returns>
		public Object PostRestRequest(BaseClass instance)
		{
			HttpWebResponse respose = null;
			url = ConfigurationManager.AppSettings["BaseRestURL"];
			ContentType = "application/json";
			Logger.Info("post Request method started");
			String uri = CompleteUrl(url, instance.Endpoint, instance.parameters);
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = Verbs.Post.ToString();
			//request.ContentLength = instance.body.Length;
			request.ContentType = ContentType;
			ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
			if (instance.headers != null)
			{
				foreach (var k in instance.headers)
				{
					request.Headers.Add(k.Key, k.Value);
				}
			}
			
            if (!string.IsNullOrEmpty(instance.body))
            {
                request.SendChunked = true;
                byte[] postbytes = Encoding.UTF8.GetBytes(instance.body);
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(postbytes, 0, postbytes.Length);
                reqStream.Close();
            }
			request.AllowAutoRedirect = false;
			try
			{
				respose = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				return e.Response;
			}
			catch (Exception e)
			{
				Logger.Info(e.GetType());
				return e;
			}
			
			return respose;
		}
        public Object PostJiraRestRequest(BaseClass instance)
        {
            HttpWebResponse respose = null;
            url = "https://xray.cloud.xpand-it.com/api/v1/";
            ContentType = "application/json";
            Logger.Info("post Jira Request method started");
            String uri = CompleteUrl(url, instance.Endpoint, instance.parameters);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = Verbs.Post.ToString();
            //request.ContentLength = instance.body.Length;
            request.ContentType = ContentType;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            if (instance.headers != null)
            {
                foreach (var k in instance.headers)
                {
                    request.Headers.Add(k.Key, k.Value);
                }
            }

            if (!string.IsNullOrEmpty(instance.body))
            {
                //request.SendChunked = true;
                byte[] postbytes = Encoding.UTF8.GetBytes(instance.body);
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(postbytes, 0, postbytes.Length);
                reqStream.Close();
            }
            request.AllowAutoRedirect = false;
            try
            {
                respose = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                return e.Response;
            }
            catch (Exception e)
            {
                Logger.Info(e.GetType());
                return e;
            }

            return respose;
        }

        /// <summary>
        /// This method is to make the patch web request
        /// </summary>
        /// <param name="instance">base class instance</param>
        /// <returns>response of the service</returns>
        public Object PatchRestRequest(BaseClass instance)
        {
            HttpWebResponse respose = null;
            url = ConfigurationManager.AppSettings["BaseRestURL"];
            ContentType = "application/json";
            Logger.Info("post Request method started");
            String uri = CompleteUrl(url, instance.Endpoint, instance.parameters);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = Verbs.Patch.ToString().ToUpper();
            //request.ContentLength = instance.body.Length;
            request.ContentType = ContentType;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            if (instance.headers != null)
            {
                foreach (var k in instance.headers)
                {
                    request.Headers.Add(k.Key, k.Value);
                }
            }
            request.SendChunked = true;
            byte[] postbytes = Encoding.UTF8.GetBytes(instance.body);
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(postbytes, 0, postbytes.Length);
            reqStream.Close();
            request.AllowAutoRedirect = false;
            try
            {
                respose = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                return e.Response;
            }
            catch (Exception e)
            {
                Logger.Info(e.GetType());
                return e;
            }
            
            return respose;
        }

       
        /// <summary>
        /// this method is to make the  url from base, endpoint and the parameters
        /// </summary>
        /// <param name="url">Base URL</param>
        /// <param name="endpoint">End point of the service</param>
        /// <param name="parameters">parameters dictionary</param>
        /// <returns>complete url string</returns>
		public String CompleteUrl(String url, String endpoint, Dictionary<String, String> parameters)
		{
			String CompleteURL = url + endpoint;
			if (parameters != null)
			{
				CompleteURL += "?";
				foreach (KeyValuePair<string, string> param in parameters)
				{
					CompleteURL += param.Key + "=" + param.Value + "&";
				}
				CompleteURL = CompleteURL.Substring(0, CompleteURL.Length - 1);
			}
			return CompleteURL;
		}

		public HttpWebResponse GetAPPURLRequest(BaseClass instance)
		{
			url = ConfigurationManager.AppSettings["QADataURLSEndpoint"];
			ContentType = "application/JSON";
			Logger.Info("Get Request method started");
			String uri = CompleteUrl(url, instance.Endpoint, instance.parameters);
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = Verbs.Get.ToString();
			request.ContentLength = 0;
			request.ContentType = ContentType;
			ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

			if (instance.headers != null)
			{
				foreach (var k in instance.headers)
				{
					request.Headers.Add(k.Key, k.Value);
				}
			}
			var response = (HttpWebResponse)request.GetResponse();
			var responseValue = string.Empty;
			if (response.StatusCode != HttpStatusCode.OK)
			{
				var message = String.Format("Failed: Received HTTP {0}", response.StatusCode);
				throw new Exception(message);
			}
			return response;
		}

	}
}
