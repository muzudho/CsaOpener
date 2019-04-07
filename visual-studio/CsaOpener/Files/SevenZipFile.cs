namespace Grayscale.CsaOpener
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// .7z ファイル。
    /// </summary>
    public class SevenZipFile : RecordArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public SevenZipFile(Config config, string filePath)
            : base(config, filePath)
        {
            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.OutDir = Path.Combine(
                this.Config.ExpansionOutputPath,
                Directory.GetParent(this.FilePath).Name,
                $"extracted-{Path.GetFileNameWithoutExtension(this.FilePath)}").Replace(@"\", "/");
            this.FilePath = this.FilePath.Replace(@"\", "/");
            Trace.WriteLine($"7zip: {this.FilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            try
            {
                Trace.WriteLine($"Un7z: {this.FilePath} -> {this.OutDir}");
                SevenZManager.fnExtract(this.FilePath, this.OutDir);

                var wentDir = Path.Combine(this.Config.ExpansionWentPath, Directory.GetParent(this.FilePath).Name);
                Program.CreateDirectory(wentDir);
                var wentFile = Path.Combine(wentDir, Path.GetFileName(this.FilePath));

                // 解凍が終わった元ファイルを移動。
                File.Move(this.FilePath, wentFile);
            }
            catch (BadImageFormatException e)
            {
                // 32ビットのプログラムを 64ビットで動かそうとしたときなど。
                Trace.WriteLine(e);
            }
            catch (InvalidOperationException e)
            {
                Trace.WriteLine(e);
            }
        }

        public override void ChangeEncoding()
        {
        }
    }
}
