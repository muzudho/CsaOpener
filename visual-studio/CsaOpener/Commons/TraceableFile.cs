namespace Grayscale.CsaOpener.Commons
{
    using System.Diagnostics;
    using System;

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
        /// 読込み。
        /// </summary>
        /// <returns>内容。</returns>
        public string ReadAllText()
        {
            Trace.WriteLine($"Read    : '{this.FullName}'...");
            return System.IO.File.ReadAllText(this.FullName);
        }

        /// <summary>
        /// 移動。
        /// </summary>
        /// <param name="destFile">移動先。</param>
        public void Move(TraceableFile destFile)
        {
            new TraceableFile(destFile.FullName).CreateParentDirectory();

            Trace.WriteLine($"Move    : '{this.FullName}' --> '{destFile.FullName}'...");
            System.IO.File.Move(this.FullName, destFile.FullName);
        }

        /// <summary>
        /// コピー。
        /// </summary>
        /// <param name="destFile">コピー先。</param>
        /// <param name="overwrite">上書き。</param>
        public void Copy(TraceableFile destFile, bool overwrite = false)
        {
            Trace.WriteLine($"Copy    : '{this.FullName}' --> '{destFile.FullName}'...");
            System.IO.File.Copy(this.FullName, destFile.FullName, overwrite);
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
            var parentFullName = string.Empty;

            try
            {
                parentFullName = System.IO.Directory.GetParent(this.FullName).FullName;
                var wentParentDir = new TraceableDirectory(parentFullName);
                wentParentDir.Create();
            }
            catch (NotSupportedException e)
            {
                Trace.WriteLine($"ThisFullName: '{this.FullName}'. FullName: '{parentFullName}'. {e}");
                throw;
            }
        }
    }
}
