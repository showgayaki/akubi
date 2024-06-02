using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace akubi.lib
{
    internal class LineNotify
    {
        /// <summary>
        /// http://the-takeo.com/?p=773
        /// </summary>
        private readonly string apiUrl;
        private readonly string accessToken;
        private static readonly HttpClient client = new HttpClient();
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal LineNotify(string apiUrl, string accessToken)
        {
            this.apiUrl = apiUrl;
            this.accessToken = accessToken;
        }

        internal async Task<bool> SendMessageAsync(string message)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "message", $"\n{message}" },
                });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);

            logger.Info("Start to post LINE Notify.");
            try
            {
                HttpResponseMessage response = await client.PostAsync(this.apiUrl, content);
                var statusCode = (int)response.StatusCode;

                if(statusCode == 200){
                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            return false;
        }
    }
}
