namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// Zip圧縮ファイル。
    /// </summary>
    public class ZipArchiveFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveFile"/> class.
        /// </summary>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public ZipArchiveFile(string expansionGoFilePath)
            : base(expansionGoFilePath)
        {
            // Trace.WriteLine($"Zip: {this.ExpansionGoFilePath}");

            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.ExpansionOutputDir = Path.Combine(KifuwarabeWcsc29Config.Instance.expansion.output, $"extracted-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
            Commons.CreateDirectory(this.ExpansionOutputDir);
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            // Trace.WriteLine($"Unzip: {this.ExpansionGoFilePath} -> {this.ExpansionOutputDir}");
            ZipFile.ExtractToDirectory(this.ExpansionGoFilePath, this.ExpansionOutputDir);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(KifuwarabeWcsc29Config.Instance.expansion.went, Path.GetFileName(this.ExpansionGoFilePath)));
        }
    }
}
