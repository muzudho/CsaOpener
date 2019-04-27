namespace Grayscale.CsaOpener
{
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;
    using System.IO;

    /// <summary>
    /// エンコードされるのを待っているファイル。
    /// </summary>
    public class FileWaitingToBeEncoded
    {
        private FileWaitingToBeEncoded(TraceableFile file)
        {
            this.GoFile = file;
            this.WentFile = new TraceableFile(PathHelper.Combine(FileSystem.FomationWentDirectory.FullName, Path.GetFileName(file.FullName)));
            this.OutputFile = new TraceableFile(PathHelper.Combine(FileSystem.FomationOutputDirectory.FullName, Path.GetFileName(file.FullName)));
        }

        /// <summary>
        /// Gets a 操作するとログを取られる待ちファイル。
        /// </summary>
        public TraceableFile GoFile { get; private set; }

        /// <summary>
        /// Gets a 操作するとログを取られる済みファイル。
        /// </summary>
        public TraceableFile WentFile { get; private set; }

        /// <summary>
        /// Gets a 操作するとログを取られる成果ファイル。
        /// </summary>
        public TraceableFile OutputFile { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="file">ファイル。</param>
        /// <returns>このインスタンス。</returns>
        public static FileWaitingToBeEncoded FromFile(string file)
        {
            return new FileWaitingToBeEncoded(new TraceableFile(file));
        }
    }
}
