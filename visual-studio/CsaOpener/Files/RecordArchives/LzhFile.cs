namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.CommonAction;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// LZH形式棋譜ファイル。
    /// </summary>
    public class LzhFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LzhFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイルパス。</param>
        public LzhFile(string expansionGoFile)
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
            Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> {ExpansionOutputDirectory.Instance.FullName}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return false;
            }

            LzhManager.fnExtract(this.ExpansionGoFilePath, ExpansionOutputDirectory.Instance.FullName);

            // ディレクトリーを浅くします。
            PathFlat.Search(ExpansionOutputDirectory.Instance.FullName);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(ExpansionWentDirectory.Instance.FullName, Path.GetFileName(this.ExpansionGoFilePath)));

            return true;
        }
    }
}
