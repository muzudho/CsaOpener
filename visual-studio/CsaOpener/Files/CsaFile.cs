using System.Diagnostics;
using System.IO;

namespace Grayscale.CsaOpener
{
    /// <summary>
    /// CSA形式棋譜ファイル。
    /// </summary>
    public class CsaFile : RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsaFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public CsaFile(Config config, string filePath)
            : base(config, filePath)
        {
            // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
            this.OutDir = Path.Combine(config.ExpansionOutputPath, $"copied-{Path.GetFileNameWithoutExtension(this.FilePath)}");
            Program.CreateDirectory(this.OutDir);
            Trace.WriteLine($"Csa: {this.FilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"UnCsa: {this.FilePath} -> {this.OutDir}");
            var out_path = Path.Combine(this.OutDir, Path.GetFileName(this.FilePath));

            // 成果物の作成。
            File.Copy(this.FilePath, out_path, true);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.FilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.FilePath)));
        }
    }
}
