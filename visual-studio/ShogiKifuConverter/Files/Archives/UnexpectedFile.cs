namespace Grayscale.ShogiKifuConverter
{
    using System.Diagnostics;
    using Grayscale.ShogiKifuConverter.CommonAction;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;

    /// <summary>
    /// 予期しない形式のファイル。
    /// </summary>
    public class UnexpectedFile : AbstractArchiveFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        public UnexpectedFile(TraceableFile expansionGoFile)
            : base(expansionGoFile)
        {
            // Trace.WriteLine($"Unexpected file: {this.ExpansionGoFilePath}");

            // .exe とか解凍できないやつが入っている☆（＾～＾）！
            // Trace.WriteLine($"むり: {this.ExpansionGoFilePath}");
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"{LogHelper.Stamp}Expand  : {this.InputFile.FullName} -> None.");
            if (string.IsNullOrWhiteSpace(this.InputFile.FullName))
            {
                return false;
            }

            // ディレクトリーを浅くします。
            PathFlat.GoFlat(LocationMaster.ExpandedDirectory.FullName);

            // 無理だった元ファイルは削除。
            this.InputFile.Delete();

            return true;
        }
    }
}
