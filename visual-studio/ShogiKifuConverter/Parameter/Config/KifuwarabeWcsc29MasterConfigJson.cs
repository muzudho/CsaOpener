namespace Grayscale.ShogiKifuConverter
{
    /// <summary>
    /// 設定。
    /// </summary>
    public class KifuwarabeWcsc29MasterConfigJson
    {
        /// <summary>
        /// Gets a 棋譜変換用ディレクトリー。
        /// </summary>
        public string converter_var_lib { get; private set; }

        /// <summary>
        /// Gets a 棋譜を読み取るための実行ファイルが置いてあるディレクトリーへのパス。
        /// </summary>
        public string kifuwarabe_wcsc29_opt { get; private set; }
    }
}
