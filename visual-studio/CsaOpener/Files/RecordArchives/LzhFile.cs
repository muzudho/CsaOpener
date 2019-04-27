namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.CommonAction;
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// LZH形式棋譜ファイル。
    /// </summary>
    public class LzhFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LzhFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        public LzhFile(TraceableFile expansionGoFile)
            : base(expansionGoFile)
        {
            // Trace.WriteLine($"Lzh: {this.ExpansionGoFilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFile.FullName} -> {ExpansionOutputDirectory.Instance.FullName}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                return false;
            }

            LzhManager.fnExtract(this.ExpansionGoFile.FullName, ExpansionOutputDirectory.Instance.FullName);

            // ディレクトリーを浅くします。
            PathFlat.Search(ExpansionOutputDirectory.Instance.FullName);

            // 解凍が終わった元ファイルを移動。
            this.ExpansionGoFile.Move(this.ExpansionWentFile);

            return true;
        }
    }
}
