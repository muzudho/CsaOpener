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
        private TraceableFile encodedFileInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGameRecordFile"/> class.
        /// </summary>
        /// <param name="inputFile">解凍を待っているファイル。</param>
        /// <param name="encodedFile">棋譜読取を待っているファイルパス。</param>
        public AbstractGameRecordFile(TraceableFile inputFile, TraceableFile encodedFile)
            : base(inputFile)
        {
            this.EncodedFile = encodedFile;
        }

        /// <summary>
        /// Gets or sets a 解凍先ファイル。
        /// </summary>
        public TraceableFile ExpansionOutputFile { get; protected set; }

        /// <summary>
        /// Gets a 棋譜読取を待っているファイル。
        /// </summary>
        public TraceableFile EncodedFile
        {
            get
            {
                return this.encodedFileInstance;
            }

            private set
            {
                this.encodedFileInstance = value;
                this.OutputFile = new TraceableFile(PathHelper.Combine(LocationMaster.ConverterOutputDirectory.FullName, $"{Path.GetFileNameWithoutExtension(value.FullName)}.tapesfrag").Replace(@"\", "/"));
            }
        }

        /// <summary>
        /// Gets a 出力先ファイルパス。
        /// </summary>
        public TraceableFile OutputFile { get; private set; }
    }
}
