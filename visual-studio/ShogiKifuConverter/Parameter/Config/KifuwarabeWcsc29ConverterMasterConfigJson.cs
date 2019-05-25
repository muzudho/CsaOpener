namespace Grayscale.ShogiKifuConverter
{
    /// <summary>
    /// 設定。
    /// </summary>
    public class KifuwarabeWcsc29ConverterMasterConfigJson
    {
        /// <summary>
        /// Gets a 変換器の入力フォルダー。
        /// </summary>
        public string input { get; private set; }

        /// <summary>
        /// Gets a 解凍済み棋譜のフォルダー。
        /// </summary>
        public string expanded { get; private set; }

        /// <summary>
        /// Gets a エンコード済み棋譜のフォルダー。
        /// </summary>
        public string encoded { get; private set; }

        /// <summary>
        /// Gets a 棋譜変換済み棋譜のフォルダー。
        /// </summary>
        public string converted { get; private set; }

        /// <summary>
        /// Gets a 棋譜を１ファイルに詰め込んだフォルダー。
        /// </summary>
        public string jammed { get; private set; }

        /// <summary>
        /// Gets a 変換器のエラー出力フォルダー。
        /// </summary>
        public string error { get; private set; }

        /// <summary>
        /// Gets a 棋譜を読み取るための実行ファイルへのパス。
        /// </summary>
        public string kifuwarabe_wcsc29_exe_path_for_convert_kifu { get; private set; }
    }
}
