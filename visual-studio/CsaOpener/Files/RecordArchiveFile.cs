namespace Grayscale.CsaOpener
{
    /// <summary>
    /// 棋譜が入った圧縮ファイル。
    /// </summary>
    public abstract class RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordArchiveFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        protected RecordArchiveFile(Config config, string filePath)
        {
            this.Config = config;
            this.FilePath = filePath;
        }

        /// <summary>
        /// Gets or sets a ファイルパス。
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// Gets or sets a 出力ディレクトリー。
        /// </summary>
        public string OutDir { get; protected set; }

        /// <summary>
        /// Gets or sets 設定。
        /// </summary>
        public Config Config { get; protected set; }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public virtual void Expand()
        {
        }

        public virtual void ChangeEncoding()
        {
        }
    }
}
