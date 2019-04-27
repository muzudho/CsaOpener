namespace Grayscale.CsaOpener
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// .7z ファイル。
    /// </summary>
    public class SevenZipFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        public SevenZipFile(TraceableFile expansionGoFile)
            : base(expansionGoFile)
        {
            // Trace.WriteLine($"7zip: {this.ExpansionGoFilePath}");
            // TODO this.ExpansionGoFilePath = this.ExpansionGoFilePath.Replace(@"\", "/");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            try
            {
                Trace.WriteLine($"Expand  : {this.ExpansionGoFile.FullName} -> {ExpansionOutputDirectory.Instance.FullName}");
                if (string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
                {
                    return false;
                }

                SevenZManager.fnExtract(this.ExpansionGoFile.FullName, ExpansionOutputDirectory.Instance.FullName);

                /*
                var wentDir = new TraceableDirectory(PathHelper.Combine(ExpansionWentDirectory.Instance.FullName, Directory.GetParent(this.ExpansionGoFile.FullName).Name));
                wentDir.Create();
                var wentFile = PathHelper.Combine(wentDir.FullName, Path.GetFileName(this.ExpansionGoFile.FullName));
                */

                // 解凍が終わった元ファイルを移動。
                this.ExpansionGoFile.Move(this.ExpansionWentFile.FullName);
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
