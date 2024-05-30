using System.Collections.Generic;
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
        public LineNotify(string apiUrl, string accessToken)
        {
            this.apiUrl = apiUrl;
            this.accessToken = accessToken;
        }
        internal void sendMessage(string message)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "message", $"\n{message}" },
                });

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);

                var result = client.PostAsync(this.apiUrl, content).Result;
            }
        }
    }
}
