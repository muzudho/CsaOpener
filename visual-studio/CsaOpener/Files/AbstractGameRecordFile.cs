namespace Grayscale.CsaOpener
{
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;
    using System.IO;

    /// <summary>
    /// 棋譜ファイル。
    /// </summary>
    public class AbstractGameRecordFile : AbstractFile
    {
        private TraceableFile eatingGoFileInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGameRecordFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        /// <param name="eatingGoFile">棋譜読取を待っているファイルパス。</param>
        public AbstractGameRecordFile(TraceableFile expansionGoFile, TraceableFile eatingGoFile)
            : base(expansionGoFile)
        {
            this.EatingGoFile = eatingGoFile;
        }

        /// <summary>
        /// Gets or sets a 解凍先ファイル。
        /// </summary>
        public TraceableFile ExpansionOutputFile { get; protected set; }

        /// <summary>
        /// Gets a 棋譜読取を待っているファイル。
        /// </summary>
        public TraceableFile EatingGoFile
        {
            get
            {
                return this.eatingGoFileInstance;
            }

            private set
            {
                this.eatingGoFileInstance = value;
                this.EatingWentFile = new TraceableFile(PathHelper.Combine(EatingWentDirectory.Instance.FullName, Path.GetFileName(value.FullName)));
            }
        }

        /// <summary>
        /// Gets a 棋譜読取済ファイル。
        /// </summary>
        public TraceableFile EatingWentFile { get; private set; }

        /// <summary>
        /// Gets or sets a 棋譜読取出力先ファイルパス。
        /// </summary>
        public string EatingOutputFilePath { get; protected set; }
    }
}
