namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// LZH形式棋譜ファイル。
    /// </summary>
    public class LzhFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LzhFile"/> class.
        /// </summary>
        /// <param name="kw29Config">設定。</param>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public LzhFile(KifuwarabeWcsc29Config kw29Config, string expansionGoFilePath)
            : base(kw29Config, expansionGoFilePath)
        {
            Trace.WriteLine($"Lzh: {this.ExpansionGoFilePath}");

            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.ExpansionOutputDir = Path.Combine(kw29Config.expansion.output, $"extracted-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
            Commons.CreateDirectory(this.ExpansionOutputDir);

        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"UnLzh: {this.ExpansionGoFilePath} -> {this.ExpansionOutputDir}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return;
            }

            LzhManager.fnExtract(this.ExpansionGoFilePath, this.ExpansionOutputDir);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(this.Kw29Config.expansion.went, Path.GetFileName(this.ExpansionGoFilePath)));
        }
    }
}
