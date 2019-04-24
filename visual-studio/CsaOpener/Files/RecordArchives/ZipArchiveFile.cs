namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using Grayscale.CsaOpener.Location;

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
            this.ExpansionOutputDir = Path.Combine(ExpansionOutputDirectory.Instance.Path, $"extracted-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
            Commons.CreateDirectory(this.ExpansionOutputDir);
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> {this.ExpansionOutputDir}");
            ZipFile.ExtractToDirectory(this.ExpansionGoFilePath, this.ExpansionOutputDir);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(ExpansionWentDirectory.Instance.Path, Path.GetFileName(this.ExpansionGoFilePath)));

            return true;
        }
    }
}
