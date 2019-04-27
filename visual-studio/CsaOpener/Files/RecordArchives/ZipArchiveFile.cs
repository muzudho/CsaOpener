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
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> {ExpansionOutputDirectory.Instance.FullName}");
            ZipFile.ExtractToDirectory(this.ExpansionGoFilePath, ExpansionOutputDirectory.Instance.FullName);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(ExpansionWentDirectory.Instance.FullName, Path.GetFileName(this.ExpansionGoFilePath)));

            return true;
        }
    }
}
