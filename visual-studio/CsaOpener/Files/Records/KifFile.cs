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
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        /// <param name="eatingGoFilePath">棋譜読取を待っているファイルパス。</param>
        public KifFile(string expansionGoFilePath, string eatingGoFilePath)
            : base(expansionGoFilePath, eatingGoFilePath)
        {
            // 解凍先ファイル。
            if (!string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                // Trace.WriteLine($"Kif exp: {this.ExpansionGoFilePath}");

                // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                var wrappingDir = new TraceableDirectory( Path.Combine(ExpansionOutputDirectory.Instance.Path, $"copied-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}"));
                wrappingDir.Create();
                this.ExpansionGoFilePath = Path.Combine(wrappingDir.FullName, Path.GetFileName(this.ExpansionGoFilePath));
            }

            // 棋譜読取を待っているファイルパス。
            if (!string.IsNullOrWhiteSpace(this.EatingGoFilePath))
            {
                // Trace.WriteLine($"Kif eat: {this.EatingGoFilePath}");

                this.EatingWentFilePath = Path.Combine(ExpansionWentDirectory.Instance.Path, Directory.GetParent(this.EatingGoFilePath).Name, Path.GetFileName(this.EatingGoFilePath));

                // 拡張子は .tapefrag
                var headLen = EatingGoDirectory.Instance.Path.Length;
                var footLen = Path.GetFileName(this.EatingGoFilePath).Length;
                var middlePath = this.EatingGoFilePath.Substring(headLen, this.EatingGoFilePath.Length - headLen - footLen).Replace(@"\", "/");
                if (middlePath[0] == '/')
                {
                    middlePath = middlePath.Substring(1);
                }

                this.EatingOutputFilePath = Path.Combine(EatingOutputDirectory.Instance.Path, middlePath, $"{Path.GetFileNameWithoutExtension(this.EatingGoFilePath)}.tapefrag").Replace(@"\", "/");

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
            Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> {this.ExpansionGoFilePath}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return false;
            }

            File.Copy(this.ExpansionGoFilePath, this.ExpansionGoFilePath, true);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(ExpansionWentDirectory.Instance.Path, Path.GetFileName(this.ExpansionGoFilePath)));

            return true;
        }

        /// <summary>
        /// 棋譜を読み取る。
        /// </summary>
        public override void ReadGameRecord()
        {
            int returnCode = CommonsLib.ReadGameRecord(this.EatingGoFilePath, this.EatingOutputFilePath);

            // 終わった元ファイルを移動。
            var dir = new TraceableDirectory( Path.Combine(EatingWentDirectory.Instance.Path, Directory.GetParent(this.EatingGoFilePath).Name));
            dir.Create();
            File.Move(this.EatingGoFilePath, Path.Combine(dir.FullName, Path.GetFileName(this.EatingGoFilePath)));
        }
    }
}
