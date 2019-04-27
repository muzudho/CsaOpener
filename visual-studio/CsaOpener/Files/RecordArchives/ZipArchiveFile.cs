namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using Grayscale.CsaOpener.Location;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// Zip圧縮ファイル。
    /// </summary>
    public class ZipArchiveFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        public ZipArchiveFile(TraceableFile expansionGoFile)
            : base(expansionGoFile)
        {
            // Trace.WriteLine($"Zip: {this.ExpansionGoFilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFile.FullName} -> {ExpansionOutputDirectory.Instance.FullName}");
            ZipFile.ExtractToDirectory(this.ExpansionGoFile.FullName, ExpansionOutputDirectory.Instance.FullName);

            // 解凍が終わった元ファイルを移動。
            this.ExpansionGoFile.Move(this.ExpansionWentFile.FullName);

            return true;
        }
    }
}
