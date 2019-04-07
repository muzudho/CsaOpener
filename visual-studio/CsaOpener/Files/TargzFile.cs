namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using ICSharpCode.SharpZipLib.GZip;
    using ICSharpCode.SharpZipLib.Tar;

    /// <summary>
    /// Tar.Gz形式棋譜ファイル。、
    /// </summary>
    public class TargzFile : RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargzFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public TargzFile(Config config, string filePath)
            : base(config, filePath)
        {
            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.OutDir = Path.Combine(config.ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(this.FilePath)}");
            Program.CreateDirectory(this.OutDir);
            Trace.WriteLine($"TarGz: {this.FilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"UnTarGz: {this.FilePath} -> {this.OutDir}");
            using (var inStream = File.OpenRead(this.FilePath))
            {
                using (var gzipStream = new GZipInputStream(inStream))
                {
                    using (var tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                    {
                        tarArchive.ExtractContents(this.OutDir);
                    }
                }
            }

            // 解凍が終わった元ファイルを移動。
            File.Move(this.FilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.FilePath)));
        }
    }
}
