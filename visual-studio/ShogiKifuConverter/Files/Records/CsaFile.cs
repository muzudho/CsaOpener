namespace Grayscale.ShogiKifuConverter
{
    using System.Diagnostics;
    using Grayscale.ShogiKifuConverter.Commons;

    /// <summary>
    /// CSA形式棋譜ファイル。
    /// </summary>
    public class CsaFile : AbstractGameRecordFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsaFile"/> class.
        /// </summary>
        /// <param name="expansionGoFile">解凍を待っているファイル。</param>
        /// <param name="eatingGoFile">棋譜読取を待っているファイルパス。</param>
        public CsaFile(TraceableFile expansionGoFile, TraceableFile eatingGoFile)
            : base(expansionGoFile, eatingGoFile)
        {
            /*
            // 解凍先ファイル。
            if (!string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                // Trace.WriteLine($"Csa exp: {this.ExpansionGoFilePath}");

                // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                var wrappingDir = new TraceableDirectory(PathHelper.Combine(ExpansionOutputDirectory.Instance.FullName, $"copied-{Path.GetFileNameWithoutExtension(this.ExpansionGoFile.FullName)}"));
                wrappingDir.Create();
                this.ExpansionOutputFile = new TraceableFile(PathHelper.Combine(wrappingDir.FullName, Path.GetFileName(this.ExpansionGoFile.FullName)));
            }
            */
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"{LogHelper.Stamp}Expand  : {this.ExpansionGoFile.FullName} -> {this.ExpansionOutputFile}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                return false;
            }

            // 成果物の作成。
            this.ExpansionGoFile.Copy(this.ExpansionOutputFile, true);

            // 解凍が終わった元ファイルは削除。
            this.ExpansionGoFile.Delete();

            return true;
        }

        /// <summary>
        /// 棋譜を読み取る。
        /// </summary>
        /// <returns>成功。</returns>
        public override bool ConvertAnyFileToRpm()
        {
            int returnCode = RustExe.ConvertAnyFileToRpm(this.EatingGoFile, this.EatingOutputFile);

            if (returnCode == 0)
            {
                // 終わった元ファイルを移動。
                this.EatingGoFile.Move(this.EatingWentFile, true);
                return true;
            }

            return false;
        }
    }
}
