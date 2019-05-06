namespace Grayscale.ShogiKifuConverter
{
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// Command line arguments.
    /// </summary>
    public class RawArguments
    {
        /// <summary>
        /// Gets or sets a value indicating whether 解凍する。
        /// </summary>
        [Option('e', "expand", Required = false, HelpText = "Expand.")]
        public bool Expand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether エンコーディング変換する。
        /// </summary>
        [Option('n', "encode", Required = false, HelpText = "Encode.")]
        public bool Encode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RPM棋譜に変換する。
        /// </summary>
        [Option('c', "convert", Required = false, HelpText = "Convert.")]
        public bool Convert { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether テープ・フラグメントをマージする。
        /// </summary>
        [Option('c', "merge", Required = false, HelpText = "Merge.")]
        public bool Merge { get; set; }

        /// <summary>
        /// 読取。
        /// </summary>
        /// <param name="args">コマンドライン引数。</param>
        /// <returns>このオブジェクト。</returns>
        public static RawArguments Load(string[] args)
        {
            RawArguments instance = null;

            Parser.Default.ParseArguments<RawArguments>(args).WithParsed(ins =>
            {
                // パース成功時
                instance = (RawArguments)ins;
            }).WithNotParsed(err =>
            {
                // パース失敗時
            });

            return instance;
        }
    }
}
