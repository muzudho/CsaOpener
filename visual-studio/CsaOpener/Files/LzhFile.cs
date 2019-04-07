namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// LZH形式棋譜ファイル。
    /// </summary>
    public class LzhFile : RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LzhFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public LzhFile(Config config, string filePath)
            : base(config, filePath)
        {
            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.OutDir = Path.Combine(config.ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(this.FilePath)}");
            Program.CreateDirectory(this.OutDir);
            Trace.WriteLine($"Lzh: {this.FilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"UnLzh: {this.FilePath} -> {this.OutDir}");
            LzhManager.fnExtract(this.FilePath, this.OutDir);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.FilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.FilePath)));
        }
    }
}
