namespace Grayscale.CsaOpener
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// .7z ファイル。
    /// </summary>
    public class SevenZipFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipFile"/> class.
        /// </summary>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        public SevenZipFile(string expansionGoFilePath)
            : base(expansionGoFilePath)
        {
            // Trace.WriteLine($"7zip: {this.ExpansionGoFilePath}");

            // 中に何入ってるか分からん。名前が被るかもしれない。
            this.ExpansionOutputDir = Path.Combine(
                ExpansionOutputDirectory.Instance.Path,
                Directory.GetParent(this.ExpansionGoFilePath).Name,
                $"extracted-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}").Replace(@"\", "/");
            this.ExpansionGoFilePath = this.ExpansionGoFilePath.Replace(@"\", "/");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            try
            {
                Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> {this.ExpansionOutputDir}");
                if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
                {
                    return false;
                }

                SevenZManager.fnExtract(this.ExpansionGoFilePath, this.ExpansionOutputDir);

                var wentDir = Path.Combine(ExpansionWentDirectory.Instance.Path, Directory.GetParent(this.ExpansionGoFilePath).Name);
                Commons.CreateDirectory(wentDir);
                var wentFile = Path.Combine(wentDir, Path.GetFileName(this.ExpansionGoFilePath));

                // 解凍が終わった元ファイルを移動。
                File.Move(this.ExpansionGoFilePath, wentFile);
                return true;
            }
            catch (BadImageFormatException e)
            {
                // 32ビットのプログラムを 64ビットで動かそうとしたときなど。
                Trace.WriteLine(e);
                return false;
            }
            catch (InvalidOperationException e)
            {
                Trace.WriteLine(e);
                return false;
            }
        }
    }
}
