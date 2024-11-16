using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace akubi.lib
{
    internal class Discord
    {
        /// <summary>
        /// http://the-takeo.com/?p=773
        /// </summary>
        private readonly string discordWebhookUrl;
        private static readonly HttpClient client = new HttpClient();
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal Discord(string discordWebhookUrl)
        {
            this.discordWebhookUrl = discordWebhookUrl;
        }

        internal async Task<bool> SendMessageAsync(string content)
        {
            string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var choicedEmoji = ChoiceEmoji();
            var data = new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "username", $"{choicedEmoji.Item1}{processName}{choicedEmoji.Item2}" },
                    { "content", $"{content}" },
                });

            logger.Info("Start to post Discord.");
            try
            {
                HttpResponseMessage response = await client.PostAsync(this.discordWebhookUrl, data);
                var statusCode = (int)response.StatusCode;

                logger.Info($"Status Code: {statusCode}");
                if (200 <= statusCode && statusCode < 300){
                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            return false;
        }

        private (string, string) ChoiceEmoji() {
            var emojis = new List<string>  { 
                "😀", "😃", "😄", "😁", "😆", "😅", "🤣", "😂", "🙂", "🙃",
                "🫠", "😉", "😊", "😇", "🥰", "😍", "🤩", "😘", "😗", "☺️",
                "☺", "😚", "😙", "🥲", "😋", "😛", "😜", "🤪", "😝", "🤑",
                "🤗", "🤭", "🫢", "🫣", "🤫", "🤔", "🫡", "🤐", "🤨", "😐",
                "😑", "😶", "🫥", "😶‍🌫️", "😶‍🌫", "😏", "😒", "🙄", "😬", "😮‍💨",
                "🤥", "🫨", "😌", "😔", "😪", "🤤", "😴", "😷", "🤒", "🤕",
                "🤢", "🤮", "🤧", "🥵", "🥶", "🥴", "😵", "😵‍💫", "🤯", "🤠",
                "🥳", "🥸", "😎", "🤓", "🧐", "😕", "🫤", "😟", "🙁", "☹️",
                "☹", "😮", "😯", "😲", "😳", "🥺", "🥹", "😦", "😧", "😨",
                "😰", "😥", "😢", "😭", "😱", "😖", "😣", "😞", "😓", "😩",
                "😫", "🥱", "😤", "😡", "😠", "🤬", "😈", "👿", "💀", "☠️",
                "☠", "💩", "🤡", "👹", "👺", "👻", "👽", "👾", "🤖", "😺",
                "😸", "😹", "😻", "😼", "😽", "🙀", "😿", "😾", "🙈", "🙉",
                "🙊", "💋", "💯", "💢", "💥", "💫", "💦", "💨", "🕳️", "🕳",
                "💬", "👁️‍🗨️", "👁‍🗨️", "👁️‍🗨", "👁‍🗨", "🗨️", "🗨", "🗯️", "🗯", "💭",
                "💤"
            };

            //ランダムに並べ替えて、2つ返す
            emojis = emojis.OrderBy(a => Guid.NewGuid()).ToList();
            return (emojis[0], emojis[1]);
        }
    }
}
