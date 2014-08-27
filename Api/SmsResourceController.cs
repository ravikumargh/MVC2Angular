using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC2Angular.Api
{
    public class SmsMessage
    {
        public string to { get; set; }
        public string message { get; set; }
    }

    public class SmsResource : SmsMessage
    {
        public int smsId { get; set; }
    }

    public class SmsResourceController : ApiController
    {
        public static Dictionary<int, SmsResource> messages = new Dictionary<int, SmsResource>();
        public static int currentId = 0;

        // GET api/<controller>
        public List<SmsResource> Get()
        {
            List<SmsResource> result = new List<SmsResource>();
            foreach (int key in messages.Keys)
            {
                result.Add(messages[key]);
            }
            return result;
        }

        // GET api/<controller>/5
        public SmsResource Get(int id)
        {
            if (messages.ContainsKey(id))
                return messages[id];
            return null;
        }

        // POST api/<controller>
        public List<SmsResource> Post([FromBody] SmsMessage value)
        {
            //Synchronize on messages so we don't have id collisions
            lock (messages)
            {
                SmsResource res = new SmsResource();
                res.message = value.message;
                res.to = value.to;
                res.smsId = currentId++;
                messages.Add(res.smsId, res);
                SentlyPlusSmsSender.SendMessage(value.to, value.message);
                return Get();
            }
        }

        // PUT api/<controller>/5
        public List<SmsResource> Put(int id, [FromBody] SmsMessage value)
        {
            //Synchronize on messages so we don't have id collisions
            lock (messages)
            {
                if (messages.ContainsKey(id))
                {
                    //Update the message
                    messages[id].message = value.message;
                    messages[id].to = value.message;
                }
                return Get();
            }
        }

        // DELETE api/<controller>/5
        public List<SmsResource> Delete(int id)
        {
            if (messages.ContainsKey(id))
            {
                messages.Remove(id);
            }
            return Get();
        }
    }
}