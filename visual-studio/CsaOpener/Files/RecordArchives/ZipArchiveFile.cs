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
        /// <param name="config">設定。</param>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public ZipArchiveFile(Config config, string expansionGoFilePath)
            : base(config, expansionGoFilePath)
        {
            Trace.WriteLine($"Zip: {this.ExpansionGoFilePath}");

            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.ExpansionOutputDir = Path.Combine(config.ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
            Commons.CreateDirectory(this.ExpansionOutputDir);
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"Unzip: {this.ExpansionGoFilePath} -> {this.ExpansionOutputDir}");
            ZipFile.ExtractToDirectory(this.ExpansionGoFilePath, this.ExpansionOutputDir);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.ExpansionGoFilePath)));
        }
    }
}
