using System.Configuration;

namespace akubi.lib
{
    internal class Settings
    {
        // using System.Configurationの記述だけでうまくいかない場合、ソリューションエクスプローラーにある参照を右クリックして、参照の追加を選択。
        // System.Configurationを検索して、チェックボックスにチェックを入れる
        internal string apiUrl = ConfigurationManager.AppSettings["API_URL"];
        internal string lineToken = ConfigurationManager.AppSettings["LINE_NOTIFY_ACCESS_TOKEN"];
    }
}
