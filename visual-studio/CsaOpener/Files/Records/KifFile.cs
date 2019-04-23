namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;

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
                var wrappingDir = Path.Combine(KifuwarabeWcsc29Config.Instance.expansion.output, $"copied-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
                Commons.CreateDirectory(wrappingDir);
                this.ExpansionGoFilePath = Path.Combine(wrappingDir, Path.GetFileName(this.ExpansionGoFilePath));
            }

            // 棋譜読取を待っているファイルパス。
            if (!string.IsNullOrWhiteSpace(this.EatingGoFilePath))
            {
                // Trace.WriteLine($"Kif eat: {this.EatingGoFilePath}");

                this.EatingWentFilePath = Path.Combine(KifuwarabeWcsc29Config.Instance.expansion.went, Directory.GetParent(this.EatingGoFilePath).Name, Path.GetFileName(this.EatingGoFilePath));

                // 拡張子は .rpmove
                var headLen = KifuwarabeWcsc29Config.Instance.eating.go.Length;
                var footLen = Path.GetFileName(this.EatingGoFilePath).Length;
                var middlePath = this.EatingGoFilePath.Substring(headLen, this.EatingGoFilePath.Length - headLen - footLen).Replace(@"\", "/");
                if (middlePath[0] == '/')
                {
                    middlePath = middlePath.Substring(1);
                }

                this.EatingOutputFilePath = Path.Combine(KifuwarabeWcsc29Config.Instance.eating.output, middlePath, $"{Path.GetFileNameWithoutExtension(this.EatingGoFilePath)}.rpmove").Replace(@"\", "/");

                // Trace.WriteLine($"config.EatingOutputPath: {config.EatingOutputPath}.");
                // Trace.WriteLine($"headLen: {headLen}, footLen: {footLen}, middlePath: {middlePath}, Output: {this.EatingOutputFilePath}.");
            }
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            // Trace.WriteLine($"Copy kif: {this.ExpansionGoFilePath} -> {this.ExpansionGoFilePath}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return;
            }

            File.Copy(this.ExpansionGoFilePath, this.ExpansionGoFilePath, true);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(KifuwarabeWcsc29Config.Instance.expansion.went, Path.GetFileName(this.ExpansionGoFilePath)));
        }

        /// <summary>
        /// エンコーディングを変えます。
        /// </summary>
        public override void ChangeEncoding()
        {
            try
            {
                // エンコーディングを変えます。
                Commons.ChangeEncodingFile(this.ExpansionGoFilePath);
            }
            catch (DirectoryNotFoundException e)
            {
                Trace.WriteLine(e);
            }
        }

        /// <summary>
        /// 棋譜を読み取る。
        /// </summary>
        public override void ReadGameRecord()
        {
            int returnCode = Commons.ReadGameRecord(this.EatingGoFilePath, this.EatingOutputFilePath);

            // 終わった元ファイルを移動。
            var dir = Path.Combine(KifuwarabeWcsc29Config.Instance.eating.went, Directory.GetParent(this.EatingGoFilePath).Name);
            Commons.CreateDirectory(dir);
            File.Move(this.EatingGoFilePath, Path.Combine(dir, Path.GetFileName(this.EatingGoFilePath)));
        }
    }
}
