using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace akubi.lib
{
    internal class Internet
    {
        /// <summary>
        /// インターネット接続チェック用クラス
        /// </summary>
        const int DELAY_MILLISECONDS = 5000;
        const int RETRY_COUNT = 10;

        private readonly string url;
        private readonly HttpClient client = new HttpClient();
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal Internet(string url) {
            this.url = url;
            // タイムアウトは3秒
            client.Timeout = TimeSpan.FromMilliseconds(3000);
        }

        internal async Task<bool> IsConnectedAsync()
        {
            for (int i = 0; i < RETRY_COUNT; i++)
            {
                logger.Info($"Starting check connect internet({i + 1}/{RETRY_COUNT})");
                try
                {
                    var response = await client.GetAsync(url);
                    logger.Info($"StatusCode: {(int)response.StatusCode}");
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    logger.Info($"Delay {DELAY_MILLISECONDS} seconds.");
                    await Task.Delay(DELAY_MILLISECONDS);
                }
            }
            return false;
        }
    }
}
