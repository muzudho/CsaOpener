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
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        protected AbstractFile(string expansionGoFilePath)
        {
            this.ExpansionGoFilePath = expansionGoFilePath;
        }

        /// <summary>
        /// Gets or sets a 解凍を待っているファイルパス。。
        /// </summary>
        public string ExpansionGoFilePath { get; protected set; }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public virtual bool Expand()
        {
            return false;
        }

        /// <summary>
        /// 棋譜を読み取る。
        /// </summary>
        public virtual void ReadGameRecord()
        {
            // OpenerConfig.Instance
        }
    }
}
