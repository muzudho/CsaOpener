namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.CommonAction;
    using Grayscale.CsaOpener.Commons;
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
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        public TargzFile(TraceableFile expansionGoFile)
            : base(expansionGoFile)
        {
            // Trace.WriteLine($"TarGz: {this.ExpansionGoFilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFile.FullName} -> {FileSystem.ExpansionOutputDirectory.FullName}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                return false;
            }

            using (var inStream = File.OpenRead(this.ExpansionGoFile.FullName))
            {
                using (var gzipStream = new GZipInputStream(inStream))
                {
                    using (var tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                    {
                        tarArchive.ExtractContents(FileSystem.ExpansionOutputDirectory.FullName);
                    }
                }
            }

            // ディレクトリーを浅くします。
            PathFlat.Search(FileSystem.ExpansionOutputDirectory.FullName);

            // 解凍が終わった元ファイルを移動。
            this.ExpansionGoFile.Move(this.ExpansionWentFile);

            return true;
        }
    }
}
