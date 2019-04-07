namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// CSA形式棋譜ファイル。
    /// </summary>
    public class CsaFile : AbstractGameRecordFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsaFile"/> class.
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        /// <param name="eatingGoFilePath">棋譜読取を待っているファイルパス。</param>
        public CsaFile(Config config, string expansionGoFilePath, string eatingGoFilePath)
            : base(config, expansionGoFilePath, eatingGoFilePath)
        {
            // 解凍先ファイル。
            if (!string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                Trace.WriteLine($"Csa exp: {this.ExpansionGoFilePath}");

                // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                var wrappingDir = Path.Combine(config.ExpansionOutputPath, $"copied-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
                Commons.CreateDirectory(wrappingDir);
                this.ExpansionOutputFile = Path.Combine(wrappingDir, Path.GetFileName(this.ExpansionGoFilePath));
            }

            // 棋譜読取を待っているファイルパス。
            if (!string.IsNullOrWhiteSpace(this.EatingGoFilePath))
            {
                Trace.WriteLine($"Kif eat: {this.EatingGoFilePath}");

                this.EatingWentFilePath = Path.Combine(config.EatingWentPath, Directory.GetParent(this.EatingGoFilePath).Name, Path.GetFileName(this.EatingGoFilePath));
                this.EatingOutputFilePath = Path.Combine(config.EatingOutputPath, Directory.GetParent(this.EatingGoFilePath).Name, Path.GetFileName(this.EatingGoFilePath));
            }
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        public override void Expand()
        {
            Trace.WriteLine($"Copy csa: {this.ExpansionGoFilePath} -> {this.ExpansionOutputFile}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return;
            }

            // 成果物の作成。
            File.Copy(this.ExpansionGoFilePath, this.ExpansionOutputFile, true);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(this.Config.ExpansionWentPath, Path.GetFileName(this.ExpansionGoFilePath)));
        }

        /// <summary>
        /// エンコーディングを変えます。
        /// </summary>
        public override void ChangeEncoding()
        {
            try
            {
                // エンコーディングを変えます。
                Commons.ChangeEncodingFile(this.Config, this.ExpansionGoFilePath);
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
            var dir = Path.Combine(this.Config.EatingWentPath, Directory.GetParent(this.EatingGoFilePath).Name);
            Commons.CreateDirectory(dir);
            File.Move(this.EatingGoFilePath, Path.Combine(this.Config.EatingWentPath, Directory.GetParent(this.EatingGoFilePath).Name, Path.GetFileName(this.EatingGoFilePath)));
        }
    }
}
