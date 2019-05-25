namespace Grayscale.ShogiKifuConverter.Commons
{
    using System.Diagnostics;
    using System;
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
            Trace.WriteLine($"{LogHelper.Stamp}Write   : '{this.FullName}'...");
            System.IO.File.WriteAllText(this.FullName, contents);
        }

        /// <summary>
        /// 読込み。
        /// </summary>
        /// <returns>内容。</returns>
        public string ReadAllText()
        {
            Trace.WriteLine($"{LogHelper.Stamp}Read    : '{this.FullName}'...");
            return System.IO.File.ReadAllText(this.FullName);
        }

        /// <summary>
        /// 移動。
        /// </summary>
        /// <param name="destFile">移動先。</param>
        /// <param name="overwrite">上書き可。</param>
        public void MoveTo(TraceableFile destFile, bool overwrite)
        {
            new TraceableFile(destFile.FullName).CreateParentDirectory();

            Trace.WriteLine($"{LogHelper.Stamp}Move    : '{this.FullName}' --> '{destFile.FullName}'...");
            if (overwrite)
            {
                destFile.Delete();
            }

            System.IO.File.Move(this.FullName, destFile.FullName);
        }

        /// <summary>
        /// 移動。
        /// </summary>
        /// <param name="destDir">移動先。</param>
        /// <param name="overwrite">上書き可。</param>
        public void MoveTo(TraceableDirectory destDir, bool overwrite)
        {
            destDir.Create();
            Trace.WriteLine($"{LogHelper.Stamp}Move    : '{this.FullName}' --into--> '{destDir.FullName}' directory...");

            var dstFile = new TraceableFile(PathHelper.Combine(destDir.FullName, Path.GetFileName(this.FullName)));

            this.MoveTo(dstFile, overwrite);
        }

        /// <summary>
        /// コピー。
        /// </summary>
        /// <param name="destFile">コピー先。</param>
        /// <param name="overwrite">上書き。</param>
        public void Copy(TraceableFile destFile, bool overwrite = false)
        {
            Trace.WriteLine($"{LogHelper.Stamp}Copy    : '{this.FullName}' --> '{destFile.FullName}'...");
            System.IO.File.Copy(this.FullName, destFile.FullName, overwrite);
        }

        /// <summary>
        /// 削除。
        /// </summary>
        public void Delete()
        {
            Trace.WriteLine($"{LogHelper.Stamp}Delete  : '{this.FullName}'...");
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
                Trace.WriteLine($"{LogHelper.Stamp}ThisFullName: '{this.FullName}'. FullName: '{parentFullName}'. {e}");
                throw;
            }
        }
    }
}
