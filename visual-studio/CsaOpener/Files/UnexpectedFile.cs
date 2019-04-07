using System.Diagnostics;
using System.IO;

namespace Grayscale.CsaOpener
{
    /// <summary>
    /// 予期しない形式のファイル。
    /// </summary>
    public class UnexpectedFile : RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public UnexpectedFile(Config config, string filePath)
            : base(config, filePath)
        {
            // .exe とか解凍できないやつが入っている☆（＾～＾）！
            Trace.WriteLine($"むり: {this.FilePath}");

            // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
            this.OutDir = Path.Combine(config.ExpansionOutputPath, $"copied-{Path.GetFileNameWithoutExtension(this.FilePath)}");
            Program.CreateDirectory(this.OutDir);
            Trace.WriteLine($"Unexpected file: {this.FilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            var out_path = Path.Combine(this.OutDir, Path.GetFileName(this.FilePath));

            Trace.WriteLine($"Copy: {this.FilePath} -> {out_path}");
            File.Copy(this.FilePath, out_path, true);

            // 無理だった元ファイルを移動。
            File.Move(this.FilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.FilePath)));
        }
    }
}
