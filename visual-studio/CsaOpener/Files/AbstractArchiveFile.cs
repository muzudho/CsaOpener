namespace Grayscale.CsaOpener
{
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// 棋譜を圧縮しているファイル。
    /// </summary>
    public class AbstractArchiveFile : AbstractFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractArchiveFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        public AbstractArchiveFile(TraceableFile expansionGoFile)
            : base(expansionGoFile)
        {
        }
    }
}
