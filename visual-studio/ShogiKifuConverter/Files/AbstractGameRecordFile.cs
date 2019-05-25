namespace Grayscale.ShogiKifuConverter
{
    using System.IO;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;

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
        /// <param name="convertorWorkingFile">棋譜読取を待っているファイルパス。</param>
        public AbstractGameRecordFile(TraceableFile expansionGoFile, TraceableFile convertorWorkingFile)
            : base(expansionGoFile)
        {
            this.ConvertorWorkingFile = convertorWorkingFile;
        }

        /// <summary>
        /// Gets or sets a 解凍先ファイル。
        /// </summary>
        public TraceableFile ExpansionOutputFile { get; protected set; }

        /// <summary>
        /// Gets a 棋譜読取を待っているファイル。
        /// </summary>
        public TraceableFile ConvertorWorkingFile
        {
            get
            {
                return this.eatingGoFileInstance;
            }

            private set
            {
                this.eatingGoFileInstance = value;
                this.ConvertorOutputFile = new TraceableFile(PathHelper.Combine(LocationMaster.ConverterOutputDirectory.FullName, $"{Path.GetFileNameWithoutExtension(value.FullName)}.tapesfrag").Replace(@"\", "/"));
            }
        }

        /// <summary>
        /// Gets a 出力先ファイルパス。
        /// </summary>
        public TraceableFile ConvertorOutputFile { get; private set; }
    }
}
