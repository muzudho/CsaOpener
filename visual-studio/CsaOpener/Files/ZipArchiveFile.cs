namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// Zip圧縮ファイル。
    /// </summary>
    public class ZipArchiveFile : RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public ZipArchiveFile(Config config, string filePath)
            : base(config, filePath)
        {
            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.OutDir = Path.Combine(config.ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(this.FilePath)}");
            Program.CreateDirectory(this.OutDir);
            Trace.WriteLine($"Zip: {this.FilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"Unzip: {this.FilePath} -> {this.OutDir}");
            ZipFile.ExtractToDirectory(this.FilePath, this.OutDir);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.FilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.FilePath)));
        }
    }
}
