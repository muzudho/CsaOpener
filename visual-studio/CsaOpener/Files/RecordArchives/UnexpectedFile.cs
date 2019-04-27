namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.Location;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// 予期しない形式のファイル。
    /// </summary>
    public class UnexpectedFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        public UnexpectedFile(TraceableFile expansionGoFile)
            : base(expansionGoFile)
        {
            // Trace.WriteLine($"Unexpected file: {this.ExpansionGoFilePath}");

            // .exe とか解凍できないやつが入っている☆（＾～＾）！
            // Trace.WriteLine($"むり: {this.ExpansionGoFilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFile.FullName} -> None.");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                return false;
            }

            // 無理だった元ファイルを移動。
            this.ExpansionGoFile.Move(this.ExpansionWentFile.FullName);

            return true;
        }
    }
}
