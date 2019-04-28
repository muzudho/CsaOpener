namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO.Compression;
    using Grayscale.CsaOpener.CommonAction;
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;

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
            Trace.WriteLine($"Expand  : {this.ExpansionGoFile.FullName} -> {LocationMaster.ExpansionOutputDirectory.FullName}");
            ZipFile.ExtractToDirectory(this.ExpansionGoFile.FullName, LocationMaster.ExpansionOutputDirectory.FullName);

            // ディレクトリーを浅くします。
            PathFlat.GoFlat(LocationMaster.ExpansionOutputDirectory.FullName);

            // 解凍が終わった元ファイルを移動。
            this.ExpansionGoFile.Move(this.ExpansionWentFile);

            return true;
        }
    }
}
