namespace Grayscale.ShogiKifuConverter
{
    using System.IO;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;

    /// <summary>
    /// さまざまなファイル。
    /// </summary>
    public abstract class AbstractFile
    {
        private TraceableFile inputFileInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        protected AbstractFile(TraceableFile expansionGoFile)
        {
            this.InputFile = expansionGoFile;
        }

        /// <summary>
        /// Gets or sets a 解凍を待っているファイル。
        /// </summary>
        public TraceableFile InputFile
        {
            get
            {
                return this.inputFileInstance;
            }

            protected set
            {
                this.inputFileInstance = value;
            }
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public virtual bool Expand()
        {
            return false;
        }

        /// <summary>
        /// 任意の棋譜をRPMに変換する。
        /// </summary>
        /// <returns>成功。</returns>
        public virtual bool ConvertAnyFileToRpm()
        {
            // TODO OpenerConfig.Instance
            return false;
        }
    }
}
