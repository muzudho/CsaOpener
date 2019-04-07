namespace Grayscale.CsaOpener
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// .7z ファイル。
    /// </summary>
    public class SevenZipFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="filePath">ファイルパス。</param>
        public SevenZipFile(Config config, string filePath)
        {
            this.Config = config;
            this.FilePath = filePath;

            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.OutDir = Path.Combine(
                this.Config.ExpansionOutputPath,
                Directory.GetParent(this.FilePath).Name,
                $"extracted-{Path.GetFileNameWithoutExtension(this.FilePath)}").Replace(@"\", "/");
            this.FilePath = this.FilePath.Replace(@"\", "/");
        }

        /// <summary>
        /// Gets or sets a ファイルパス。
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets a 出力ディレクトリー。
        /// </summary>
        public string OutDir { get; set; }

        /// <summary>
        /// Gets or sets 設定。
        /// </summary>
        public Config Config { get; set; }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public void Expand()
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

        public void ChangeEncoding()
        {

        }
    }
}
