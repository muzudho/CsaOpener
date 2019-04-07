namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
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
        /// <param name="config">設定。</param>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public TargzFile(KifuwarabeWcsc29Config config, string expansionGoFilePath)
            : base(config, expansionGoFilePath)
        {
            Trace.WriteLine($"TarGz: {this.ExpansionGoFilePath}");

            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.ExpansionOutputDir = Path.Combine(config.ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
            Commons.CreateDirectory(this.ExpansionOutputDir);
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"UnTarGz: {this.ExpansionGoFilePath} -> {this.ExpansionOutputDir}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return;
            }

            using (var inStream = File.OpenRead(this.ExpansionGoFilePath))
            {
                using (var gzipStream = new GZipInputStream(inStream))
                {
                    using (var tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                    {
                        tarArchive.ExtractContents(this.ExpansionOutputDir);
                    }
                }
            }

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.ExpansionGoFilePath)));
        }
    }
}
