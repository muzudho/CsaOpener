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
                this.ConvertedFile = new TraceableFile(PathHelper.Combine(LocationMaster.ConvertedDirectory.FullName, $"{Path.GetFileNameWithoutExtension(value.FullName)}.tapesfrag").Replace(@"\", "/"));
            }
        }

        /// <summary>
        /// Gets a 出力先ファイルパス。
        /// </summary>
        public TraceableFile ConvertedFile { get; private set; }
    }
}
