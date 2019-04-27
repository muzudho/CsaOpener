namespace Grayscale.CsaOpener.Files.RpmObj
{
    /// <summary>
    /// 拡張子が .tapefrag のファイル。
    /// </summary>
    public class TapeFragmentFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TapeFragmentFile"/> class.
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        public TapeFragmentFile(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Gets a ファイルパス。
        /// </summary>
        public string FilePath { get; private set; }
    }
}
