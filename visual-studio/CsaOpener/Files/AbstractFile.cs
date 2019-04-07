namespace Grayscale.CsaOpener
{
    /// <summary>
    /// さまざまなファイル。
    /// </summary>
    public abstract class AbstractFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        protected AbstractFile(Config config, string expansionGoFilePath)
        {
            this.Config = config;
            this.ExpansionGoFilePath = expansionGoFilePath;
        }

        /// <summary>
        /// Gets or sets a 解凍を待っているファイルパス。。
        /// </summary>
        public string ExpansionGoFilePath { get; protected set; }

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

        /// <summary>
        /// エンコーディングを変換します。
        /// </summary>
        public virtual void ChangeEncoding()
        {
        }

        /// <summary>
        /// 棋譜を読み取る。
        /// </summary>
        public virtual void ReadGameRecord()
        {
        }
    }
}
