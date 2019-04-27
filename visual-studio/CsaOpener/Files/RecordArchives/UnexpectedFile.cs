namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// 予期しない形式のファイル。
    /// </summary>
    public class UnexpectedFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFile"/> class.
        /// </summary>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public UnexpectedFile(string expansionGoFilePath)
            : base(expansionGoFilePath)
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
            Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> None.");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return false;
            }

            var wentDir = Path.Combine(ExpansionWentDirectory.Instance.FullName, Path.GetFileName(this.ExpansionGoFilePath));
            // Trace.WriteLine($"Evasion: {this.ExpansionGoFilePath} -> {wentDir}");

            // 無理だった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, wentDir);

            return true;
        }
    }
}
