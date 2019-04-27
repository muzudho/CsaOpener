namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.Location;
    using ICSharpCode.SharpZipLib.GZip;
    using ICSharpCode.SharpZipLib.Tar;

    /// <summary>
    /// Tar.Gz形式棋譜ファイル。、
    /// </summary>
    public class TargzFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargzFile"/> class.
        /// </summary>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public TargzFile(string expansionGoFilePath)
            : base(expansionGoFilePath)
        {
            // Trace.WriteLine($"TarGz: {this.ExpansionGoFilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> {ExpansionOutputDirectory.Instance.FullName}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return false;
            }

            using (var inStream = File.OpenRead(this.ExpansionGoFilePath))
            {
                using (var gzipStream = new GZipInputStream(inStream))
                {
                    using (var tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                    {
                        tarArchive.ExtractContents(ExpansionOutputDirectory.Instance.FullName);
                    }
                }
            }

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(ExpansionWentDirectory.Instance.FullName, Path.GetFileName(this.ExpansionGoFilePath)));

            return true;
        }
    }
}
