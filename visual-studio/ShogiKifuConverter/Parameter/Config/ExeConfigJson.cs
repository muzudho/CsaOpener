namespace Grayscale.ShogiKifuConverter
{
    using System.IO;
    using Codeplex.Data;
    using Grayscale.ShogiKifuConverter.Location;

    /// <summary>
    /// 設定の内容。
    /// </summary>
    public class ExeConfigJson
    {
        /// <summary>
        /// Gets a 棋譜変換設定マスター・ファイルへのパス。
        /// </summary>
        public string kifuwarabe_wcsc29_converter_master_config_path { get; set; }
    }
}
