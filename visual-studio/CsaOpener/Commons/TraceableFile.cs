namespace Grayscale.CsaOpener.Commons
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// ログを出すファイル。
    /// </summary>
    public class TraceableFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceableFile"/> class.
        /// </summary>
        /// <param name="file">ファイル・パス。</param>
        public TraceableFile(string file)
        {
            this.File = file;
        }

        private string File { get; set; }

        /// <summary>
        /// 書き込み。
        /// </summary>
        /// <param name="contents">内容。</param>
        public void WriteAllText(string contents)
        {
            Trace.WriteLine($"Write   : '{this.File}'.");
            System.IO.File.WriteAllText(this.File, contents);
        }
    }
}
