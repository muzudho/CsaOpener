namespace Grayscale.CsaOpener
{
    /// <summary>
    /// 棋譜を圧縮しているファイル。
    /// </summary>
    public class AbstractArchiveFile : AbstractFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractArchiveFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public AbstractArchiveFile(Config config, string expansionGoFilePath)
            : base(config, expansionGoFilePath)
        {
        }

        /// <summary>
        /// Gets or sets a 解凍先ディレクトリー。
        /// </summary>
        public string ExpansionOutputDir { get; protected set; }
    }
}
