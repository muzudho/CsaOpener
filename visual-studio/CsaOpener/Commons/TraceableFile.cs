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
            this.FullName = file;
        }

        /// <summary>
        /// Gets a フル・パス。
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// 書き込み。
        /// </summary>
        /// <param name="contents">内容。</param>
        public void WriteAllText(string contents)
        {
            Trace.WriteLine($"Write   : '{this.FullName}'...");
            System.IO.File.WriteAllText(this.FullName, contents);
        }

        /// <summary>
        /// 移動。
        /// </summary>
        /// <param name="destFile">移動先。</param>
        public void Move(string destFile)
        {
            new TraceableFile(destFile).CreateParentDirectory();

            Trace.WriteLine($"Move    : '{this.FullName}' --> '{destFile}'...");
            System.IO.File.Move(this.FullName, destFile);
        }

        /// <summary>
        /// コピー。
        /// </summary>
        /// <param name="destFile">コピー先。</param>
        /// <param name="overwrite">上書き。</param>
        public void Copy(string destFile, bool overwrite = false)
        {
            Trace.WriteLine($"Copy    : '{this.FullName}' --> '{destFile}'...");
            System.IO.File.Copy(this.FullName, destFile, overwrite);
        }

        /// <summary>
        /// 削除。
        /// </summary>
        public void Delete()
        {
            Trace.WriteLine($"Move    : '{this.FullName}'...");
            System.IO.File.Delete(this.FullName);
        }

        /// <summary>
        /// 親ディレクトリの作成。
        /// </summary>
        public void CreateParentDirectory()
        {
            var wentParentDir = new TraceableDirectory(System.IO.Directory.GetParent(this.FullName).FullName);
            wentParentDir.Create();
        }
    }
}
