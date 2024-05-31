using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace akubi.lib
{
    internal class LineNotify
    {
        /// <summary>
        /// http://the-takeo.com/?p=773
        /// </summary>
        private string apiUrl;
        private string accessToken;
        internal LineNotify(string apiUrl, string accessToken)
        {
            this.apiUrl = apiUrl;
            this.accessToken = accessToken;
        }

        internal int SendMessage(string message)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "message", $"\n{message}" },
                });

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);

                try
                {
                    HttpResponseMessage res = client.PostAsync(this.apiUrl, content).Result;
                    var statusCode = (int)res.StatusCode;

                    return statusCode;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return 0;
        }
    }
}
