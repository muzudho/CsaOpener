namespace Grayscale.ShogiKifuConverter
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;

    /// <summary>
    /// KIF形式棋譜ファイル。
    /// </summary>
    public class KifFile : AbstractGameRecordFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KifFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        /// <param name="eatingGoFile">棋譜読取を待っているファイルパス。</param>
        public KifFile(TraceableFile expansionGoFile, TraceableFile eatingGoFile)
            : base(expansionGoFile, eatingGoFile)
        {
            /*
            // 解凍先ファイル。
            if (!string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                // Trace.WriteLine($"Kif exp: {this.ExpansionGoFilePath}");

                // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                var wrappingDir = new TraceableDirectory(PathHelper.Combine(ExpansionOutputDirectory.Instance.FullName, $"copied-{Path.GetFileNameWithoutExtension(this.ExpansionGoFile.FullName)}"));
                wrappingDir.Create();
                this.ExpansionGoFile = new TraceableFile(PathHelper.Combine(wrappingDir.FullName, Path.GetFileName(this.ExpansionGoFile.FullName)));
            }
            */
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            if (string.IsNullOrWhiteSpace(this.InputFile.FullName))
            {
                return false;
            }

            Trace.WriteLine($"{LogHelper.Stamp}Expand  : [{this.InputFile.FullName}].");

            this.InputFile.Copy(new TraceableFile(PathHelper.Combine(LocationMaster.ExpandedDirectory.FullName, Path.GetFileName(this.InputFile.FullName))), true);

            // 解凍が終わった元ファイルは削除。
            this.InputFile.Delete();

            return true;
        }

        /// <summary>
        /// 棋譜を読み取る。
        /// </summary>
        /// <returns>成功。</returns>
        public override bool ConvertAnyFileToRpm()
        {
            int returnCode = RustExe.ConvertAnyFileToRpm(this.EncodedFile, this.ConvertedFile);

            if (returnCode == 0)
            {
                // 終わった元ファイルを削除。
                this.EncodedFile.Delete();
                return true;
            }

            this.EncodedFile.MoveTo(LocationMaster.ErrorDirectory, true);
            return false;
        }
    }
}
