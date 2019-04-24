﻿namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// CSA形式棋譜ファイル。
    /// </summary>
    public class CsaFile : AbstractGameRecordFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsaFile"/> class.
        /// </summary>
        /// <param name="expansionGoFilePath">解凍を待っているファイルパス。</param>
        /// <param name="eatingGoFilePath">棋譜読取を待っているファイルパス。</param>
        public CsaFile(string expansionGoFilePath, string eatingGoFilePath)
            : base(expansionGoFilePath, eatingGoFilePath)
        {
            // 解凍先ファイル。
            if (!string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                // Trace.WriteLine($"Csa exp: {this.ExpansionGoFilePath}");

                // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                var wrappingDir = Path.Combine(ExpansionOutputDirectory.Instance.Path, $"copied-{Path.GetFileNameWithoutExtension(this.ExpansionGoFilePath)}");
                Commons.CreateDirectory(wrappingDir);
                this.ExpansionOutputFile = Path.Combine(wrappingDir, Path.GetFileName(this.ExpansionGoFilePath));
            }

            // 棋譜読取を待っているファイルパス。
            if (!string.IsNullOrWhiteSpace(this.EatingGoFilePath))
            {
                // Trace.WriteLine($"Kif eat: {this.EatingGoFilePath}");

                this.EatingWentFilePath = Path.Combine(EatingWentDirectory.Instance.Path, Directory.GetParent(this.EatingGoFilePath).Name, Path.GetFileName(this.EatingGoFilePath));

                // 拡張子は .rpmove
                var headLen = EatingGoDirectory.Instance.Path.Length;
                var footLen = Path.GetFileName(this.EatingGoFilePath).Length;
                var middlePath = this.EatingGoFilePath.Substring(headLen, this.EatingGoFilePath.Length - headLen - footLen).Replace(@"\", "/");
                if (middlePath[0] == '/')
                {
                    middlePath = middlePath.Substring(1);
                }

                this.EatingOutputFilePath = Path.Combine(EatingOutputDirectory.Instance.Path, middlePath, $"{Path.GetFileNameWithoutExtension(this.EatingGoFilePath)}.rpmove").Replace(@"\", "/");

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
            Trace.WriteLine($"Expand  : {this.ExpansionGoFilePath} -> {this.ExpansionOutputFile}");
            if (string.IsNullOrWhiteSpace(this.ExpansionGoFilePath))
            {
                return false;
            }

            // 成果物の作成。
            File.Copy(this.ExpansionGoFilePath, this.ExpansionOutputFile, true);

            // 解凍が終わった元ファイルを移動。
            File.Move(this.ExpansionGoFilePath, Path.Combine(ExpansionWentDirectory.Instance.Path, Path.GetFileName(this.ExpansionGoFilePath)));

            return true;
        }

        /// <summary>
        /// エンコーディングを変えます。
        /// </summary>
        public override void ChangeEncodingOfTextFile()
        {
            try
            {
                // エンコーディングを変えます。
                Commons.ChangeEncodingOfTextFile(this.ExpansionGoFilePath);
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
            var dir = Path.Combine(EatingWentDirectory.Instance.Path, Directory.GetParent(this.EatingGoFilePath).Name);
            Commons.CreateDirectory(dir);
            File.Move(this.EatingGoFilePath, Path.Combine(EatingWentDirectory.Instance.Path, Directory.GetParent(this.EatingGoFilePath).Name, Path.GetFileName(this.EatingGoFilePath)));
        }
    }
}
