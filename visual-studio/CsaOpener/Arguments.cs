namespace Grayscale.CsaOpener
{
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// Command line arguments.
    /// </summary>
    public class Arguments
    {
        /// <summary>
        /// Gets or sets a input directory.
        /// </summary>
        [Option('i', "input", Required = false, HelpText = "The input directory where the record is put.")]
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets a output directory.
        /// </summary>
        [Option('o', "output", Required = false, HelpText = "The output directory where the record is put.")]
        public string Output { get; set; }

        /// <summary>
        /// 読取。
        /// </summary>
        /// <param name="args">コマンドライン引数。</param>
        /// <returns>このオブジェクト。</returns>
        public static Arguments Load(string[] args)
        {
            Arguments instance = null;

            Parser.Default.ParseArguments<Arguments>(args).WithParsed(ins =>
            {
                // パース成功時
                instance = (Arguments)ins;
            }).WithNotParsed(err =>
            {
                // パース失敗時
            });

            return instance;
        }
    }
}
