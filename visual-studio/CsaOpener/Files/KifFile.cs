namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// KIF形式棋譜ファイル。
    /// </summary>
    public class KifFile : RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KifFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public KifFile(Config config, string filePath)
            : base(config, filePath)
        {
            // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
            this.OutDir = Path.Combine(config.ExpansionOutputPath, $"copied-{Path.GetFileNameWithoutExtension(this.FilePath)}");
            Program.CreateDirectory(this.OutDir);
            Trace.WriteLine($"Kif: {this.FilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            var out_path = Path.Combine(this.OutDir, Path.GetFileName(this.FilePath));

            Trace.WriteLine($"UnKif: {this.FilePath} -> {out_path}");
            File.Copy(this.FilePath, out_path, true);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.FilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.FilePath)));
        }
    }
}
