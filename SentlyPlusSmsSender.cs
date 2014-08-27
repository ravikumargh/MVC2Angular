using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MVC2Angular
{
    public static class StringExtension
    {
        static public string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }

    public class AuthResponse
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }

    public class SendSmsRequest
    {
        public string[] phoneNumbers { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string senderId { get; set; }
        public string callbackUrl { get; set; }
        public int sendWhenMilliseconds { get; set; }
        public DateTime clientDate { get; set; }
        public bool isUnsubscribeEnabled { get; set; }
    }

    //This is sample code that shows how to use RestSharp to call our APIs
    //This code does not handle errors etc. For proper documentation of our API
    //please visit http://docs.sentlyplus.apiary.io/
    public class SentlyPlusSmsSender
    {
        private static string bearerToken = null;
        private static object lockObj = new object();

        private static void GetBearerToken()
        {
            var baseUrl = "https://plus.sent.ly/api/";
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/token", Method.POST);

            var concat = ConfigurationManager.AppSettings["SentlyPlusConsumerKey"] + ":" +
                 ConfigurationManager.AppSettings["SentlyPlusConsumerSecret"];

            string encodeTo64 = concat.EncodeTo64();

            request.AddHeader("Authorization", "Basic " + encodeTo64);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
            request.AddParameter("grant_type", "client_credentials");
            var restResponse = client.Execute<AuthResponse>(request);

            bearerToken = restResponse.Data.access_token;
        }

        public static void SendMessage(string to, string text)
        {
            lock (lockObj)
            {
                if (bearerToken == null)
                    GetBearerToken();
            }


            var client = new RestClient("https://plus.sent.ly/api/");
            var request = new RestRequest("sms", Method.POST);

            request.AddHeader("Authorization", "Bearer " + bearerToken);

            SendSmsRequest smsRequest = new SendSmsRequest();
            smsRequest.callbackUrl = null;
            smsRequest.clientDate = DateTime.Now;
            smsRequest.description = null;
            smsRequest.isUnsubscribeEnabled = false;
            smsRequest.message = text;
            smsRequest.phoneNumbers = new string[1];
            smsRequest.phoneNumbers[0] = to;
            smsRequest.senderId = ConfigurationManager.AppSettings["SentlyPlusSenderId"];
            smsRequest.sendWhenMilliseconds = 0;

            request.AddObject(smsRequest);

            var response = client.Execute(request);
        }
    }
}