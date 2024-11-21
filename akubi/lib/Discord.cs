using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace akubi.lib
{
    internal class Discord
    {
        /// <summary>
        /// http://the-takeo.com/?p=773
        /// </summary>
        private readonly string discordWebhookUrl;
        private readonly HttpClient client = new HttpClient();
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal Discord(string discordWebhookUrl)
        {
            this.discordWebhookUrl = discordWebhookUrl;
            // タイムアウトは3秒
            client.Timeout = TimeSpan.FromMilliseconds(3000);
        }

        internal async Task<bool> SendMessageAsync(string content)
        {
            string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var choicedEmoji = await ChoiceEmojiAsync();
            var data = new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "username", $"{choicedEmoji.Item1}{processName}{choicedEmoji.Item2}" },
                    { "content", $"{content}" },
                });

            logger.Info("Starting POST Discord.");
            try
            {
                HttpResponseMessage response = await client.PostAsync(this.discordWebhookUrl, data);
                var statusCode = (int)response.StatusCode;

                logger.Info($"Status Code: {statusCode}");
                if (200 <= statusCode && statusCode < 300)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            return false;
        }

        private async Task<(string, string)> ChoiceEmojiAsync()
        {
            var emojis = new List<string>();
            try
            {
                logger.Info($"Starting GET emojis from [{Settings.emojiApiUrl}].");
                HttpResponseMessage response = await client.GetAsync(Settings.emojiApiUrl);
                var statusCode = (int)response.StatusCode;

                logger.Info($"Status Code: {statusCode}");
                if (200 <= statusCode && statusCode < 300)
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<List<string>>();
                    logger.Info($"responseContent(Count: {responseContent.Count}): [{string.Join(", ", responseContent)}]");
                    emojis = responseContent;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                logger.Info($"Choice from local emojis.");
                emojis = Emoji.emojis;
            }

            //ランダムに並べ替えて、2つ返す
            emojis = emojis.OrderBy(a => Guid.NewGuid()).ToList();
            var choicedEmojis = (emojis[0], emojis[1]);
            logger.Info($"Choiced emojis: {string.Join(", ", choicedEmojis)}");
            return (choicedEmojis);
        }
    }
}
