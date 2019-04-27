namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;

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
            // 解凍先ファイル。
            if (!string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                // Trace.WriteLine($"Kif exp: {this.ExpansionGoFilePath}");

                // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                var wrappingDir = new TraceableDirectory(PathHelper.Combine(ExpansionOutputDirectory.Instance.FullName, $"copied-{Path.GetFileNameWithoutExtension(this.ExpansionGoFile.FullName)}"));
                wrappingDir.Create();
                this.ExpansionGoFile = new TraceableFile(PathHelper.Combine(wrappingDir.FullName, Path.GetFileName(this.ExpansionGoFile.FullName)));
            }

            // 棋譜読取を待っているファイルパス。
            if (!string.IsNullOrWhiteSpace(this.EatingGoFile.FullName))
            {
                // Trace.WriteLine($"Kif eat: {this.EatingGoFilePath}");

                // 拡張子は .tapefrag
                var headLen = EatingGoDirectory.Instance.FullName.Length;
                var footLen = Path.GetFileName(this.EatingGoFile.FullName).Length;
                var middlePath = this.EatingGoFile.FullName.Substring(headLen, this.EatingGoFile.FullName.Length - headLen - footLen).Replace(@"\", "/");
                if (middlePath[0] == '/')
                {
                    middlePath = middlePath.Substring(1);
                }

                this.EatingOutputFilePath = PathHelper.Combine(EatingOutputDirectory.Instance.FullName, middlePath, $"{Path.GetFileNameWithoutExtension(this.EatingGoFile.FullName)}.tapefrag").Replace(@"\", "/");

                // Trace.WriteLine($"config.EatingOutputPath: {config.EatingOutputPath}.");
                // Trace.WriteLine($"headLen: {headLen}, footLen: {footLen}, middlePath: {middlePath}, Output: {this.EatingOutputFilePath}.");
            }
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <returns>展開に成功した。</returns>
        public override bool Expand()
        {
            Trace.WriteLine($"Expand  : {this.ExpansionGoFile.FullName} -> {this.ExpansionGoFile.FullName}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFile.FullName))
            {
                return false;
            }

            File.Copy(this.ExpansionGoFile.FullName, this.ExpansionGoFile.FullName, true);

            // 解凍が終わった元ファイルを移動。
            this.ExpansionGoFile.Move(this.ExpansionWentFile);

            return true;
        }

        /// <summary>
        /// 棋譜を読み取る。
        /// </summary>
        public override void ReadGameRecord()
        {
            int returnCode = CommonsLib.ReadGameRecord(this.EatingGoFile.FullName, this.EatingOutputFilePath);

            // 終わった元ファイルを移動。
            this.EatingGoFile.Move(this.EatingWentFile);
        }
    }
}
