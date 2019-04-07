namespace Grayscale.CsaOpener.Files.RpmObj
{
    /// <summary>
    /// 拡張子が .rpmove のファイル。
    /// </summary>
    public class RpmObjFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RpmObjFile"/> class.
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        public RpmObjFile(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Gets a ファイルパス。
        /// </summary>
        public string FilePath { get; private set; }
    }
}
