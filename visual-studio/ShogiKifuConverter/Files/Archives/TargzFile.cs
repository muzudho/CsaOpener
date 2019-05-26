namespace Grayscale.ShogiKifuConverter
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.ShogiKifuConverter.CommonAction;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;
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
            Trace.WriteLine($"{LogHelper.Stamp}Expand  : {this.InputFile.FullName} -> {LocationMaster.ExpandedDirectory.FullName}");
            if (string.IsNullOrWhiteSpace(this.InputFile.FullName))
            {
                return false;
            }

            using (var inStream = File.OpenRead(this.InputFile.FullName))
            {
                using (var gzipStream = new GZipInputStream(inStream))
                {
                    using (var tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                    {
                        tarArchive.ExtractContents(LocationMaster.ExpandedDirectory.FullName);
                    }
                }
            }

            // ディレクトリーを浅くします。
            PathFlat.GoFlat(LocationMaster.ExpandedDirectory.FullName);

            // 解凍が終わった元ファイルは削除。
            this.InputFile.Delete();

            return true;
        }
    }
}
