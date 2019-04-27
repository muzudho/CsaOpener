using Grayscale.CsaOpener.Commons;

namespace Grayscale.CsaOpener
{
    /// <summary>
    /// 棋譜ファイル。
    /// </summary>
    public class AbstractGameRecordFile : AbstractFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGameRecordFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        /// <param name="eatingGoFilePath">棋譜読取を待っているファイルパス。</param>
        public AbstractGameRecordFile(TraceableFile expansionGoFile, string eatingGoFilePath)
            : base(expansionGoFile)
        {
            this.EatingGoFilePath = eatingGoFilePath;
        }

        /// <summary>
        /// Gets or sets a 解凍先ファイル。
        /// </summary>
        public string ExpansionOutputFile { get; protected set; }

        /// <summary>
        /// Gets a 棋譜読取を待っているファイルパス。
        /// </summary>
        public string EatingGoFilePath { get; private set; }

        /// <summary>
        /// Gets or sets a 棋譜読取済ファイルパス。
        /// </summary>
        public string EatingWentFilePath { get; protected set; }

        /// <summary>
        /// Gets or sets a 棋譜読取出力先ファイルパス。
        /// </summary>
        public string EatingOutputFilePath { get; protected set; }
    }
}
