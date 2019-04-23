namespace Grayscale.CsaOpener
{
    using System.IO;
    using Codeplex.Data;

    /// <summary>
    /// 設定。
    /// </summary>
    public class KifuwarabeWcsc29Config
    {
        private static KifuwarabeWcsc29Config thisInstance;

        /// <summary>
        /// Gets a ゲームエンジンの設定ファイル。
        /// </summary>
        /// <returns>ゲームエンジンの設定。</returns>
        public static KifuwarabeWcsc29Config Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    var json = DynamicJson.Parse(File.ReadAllText(OpenerConfig.Instance.KifuwarabeWcsc29ConfigPath));
                    thisInstance = json.Deserialize<KifuwarabeWcsc29Config>();
                }

                return thisInstance;
            }
        }

        /// <summary>
        /// ディレクトリー。
        /// </summary>
        public class Directories
        {
            /// <summary>
            /// これからやるパス。
            /// </summary>
            public string go { get; set; }

            /// <summary>
            /// やり終わったパス。
            /// </summary>
            public string went { get; set; }

            /// <summary>
            /// 出力先のパス。
            /// </summary>
            public string output { get; set; }
        }

        /// <summary>
        /// Gets a 解凍。
        /// </summary>
        public Directories expansion { get; private set; }

        /// <summary>
        /// Gets a エンコーディング変換。
        /// </summary>
        public Directories formation { get; private set; }

        /// <summary>
        /// Gets a PRM棋譜に変換。
        /// </summary>
        public Directories eating { get; private set; }

        /// <summary>
        /// Gets a 学習済みパス。
        /// </summary>
        public string learning { get; private set; }

        /// <summary>
        /// Gets a マージ済み棋譜ファイル置き場。
        /// </summary>
        public string rpm_record { get; private set; }
    }
}
