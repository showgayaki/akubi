using System.Configuration;

namespace akubi.lib
{
    internal class Settings
    {
        // using System.Configurationの記述だけでうまくいかない場合、ソリューションエクスプローラーにある参照を右クリックして、参照の追加を選択。
        // System.Configurationを検索して、チェックボックスにチェックを入れる
        internal static readonly string connectCheckURl = ConfigurationManager.AppSettings["CONNECT_CHECK_URL"];
        internal static readonly string discordWebhookUrl = ConfigurationManager.AppSettings["DISCORD_WEBHOOK_URL"];
        internal static readonly string emojiApiUrl = ConfigurationManager.AppSettings["EMOJI_API_URL"];
    }
}
