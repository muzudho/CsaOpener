namespace Grayscale.ShogiKifuConverter
{
    using System.IO;
    using Codeplex.Data;
    using Grayscale.ShogiKifuConverter.Location;

    /// <summary>
    /// 設定。
    /// </summary>
    public class KifuwarabeWcsc29MasterConfigJson
    {
        /// <summary>
        /// Gets a 変換器の入力フォルダー。
        /// </summary>
        public string converter_input { get; private set; }

        /// <summary>
        /// Gets a 変換器の解凍中フォルダー。
        /// </summary>
        public string converter_expand { get; private set; }

        /// <summary>
        /// Gets a 変換器の作業中フォルダー。
        /// </summary>
        public string converter_working { get; private set; }

        /// <summary>
        /// Gets a 変換器の出力フォルダー。
        /// </summary>
        public string converter_output { get; private set; }

        /// <summary>
        /// Gets a 変換器のエラー出力フォルダー。
        /// </summary>
        public string converter_error { get; private set; }

        /// <summary>
        /// Gets a 記録用パス。
        /// </summary>
        public string learning { get; private set; }

        /// <summary>
        /// Gets a テープ断片用パス。
        /// </summary>
        public string tapes_fragments { get; private set; }

        /// <summary>
        /// Gets a マージ済み棋譜ファイル置き場。
        /// </summary>
        public string training { get; private set; }

        /// <summary>
        /// Gets a 定跡の置き場。
        /// </summary>
        public string book { get; private set; }

        /// <summary>
        /// Gets a 棋譜を読み取るための実行ファイルへのパス。
        /// </summary>
        public string kifuwarabe_wcsc29_exe_path_for_read_kifu { get; private set; }
    }
}
