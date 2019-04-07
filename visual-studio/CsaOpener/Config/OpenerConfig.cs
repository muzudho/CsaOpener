namespace Grayscale.CsaOpener
{
    using System.IO;
    using Codeplex.Data;

    /// <summary>
    /// 設定。
    /// </summary>
    public class OpenerConfig
    {
        /// <summary>
        /// Gets a 設定ファイルへのパス。
        /// </summary>
        public string KifuwarabeWcsc29ConfigPath { get; private set; }

        /// <summary>
        /// Gets a 実行ファイルへのパス。
        /// </summary>
        public string KifuwarabeWcsc29ExePath { get; private set; }

        /// <summary>
        /// ファイル読み取り。
        /// </summary>
        /// <returns>設定。</returns>
        public static OpenerConfig Load()
        {
            var json = File.ReadAllText("./config.json");
            dynamic config1 = DynamicJson.Parse(json);

            var config2 = new OpenerConfig();
            config2.KifuwarabeWcsc29ConfigPath = config1.kifuwarabe_wcsc29_config_path;
            config2.KifuwarabeWcsc29ExePath = config1.kifuwarabe_wcsc29_exe_path;

            return config2;
        }
    }
}
