namespace Grayscale.ShogiKifuConverter.Commons
{
    using System.Diagnostics;

    /// <summary>
    /// ログを出すディレクトリー。
    /// </summary>
    public class TraceableDirectory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceableDirectory"/> class.
        /// </summary>
        /// <param name="fullName">ディレクトリーへのパス。</param>
        public TraceableDirectory(string fullName)
        {
            this.FullName = fullName;
        }

        /// <summary>
        /// Gets a フル・パス。
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// 削除。
        /// </summary>
        /// <param name="recursive">中身があっても消す。</param>
        public void Delete(bool recursive)
        {
            Trace.WriteLine($"{LogHelper.Stamp}Move    : '{this.FullName}' directory ...");
            System.IO.Directory.Delete(this.FullName, recursive);
        }

        /// <summary>
        /// ディレクトリーがなければ作るぜ☆（＾～＾）
        /// </summary>
        public void Create()
        {
            if (!System.IO.Directory.Exists(this.FullName))
            {
                Trace.WriteLine($"{LogHelper.Stamp}Create  : '{this.FullName}' directory ...");
                System.IO.Directory.CreateDirectory(this.FullName);
            }
        }
    }
}
