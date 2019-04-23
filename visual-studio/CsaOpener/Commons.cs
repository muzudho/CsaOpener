namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// 何か所かで使うもの。
    /// </summary>
    public static class Commons
    {
        /// <summary>
        /// 棋譜を読み取ります。
        /// </summary>
        /// <param name="input_file">読み取る棋譜ファイルのパス。</param>
        /// <param name="output_file">出力先ファイルパス。</param>
        /// <returns>リターン コード。</returns>
        public static int ReadGameRecord(string input_file, string output_file)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            // 起動する実行ファイルのパスを設定する
            info.FileName = OpenerConfig.Instance.KifuwarabeWcsc29ExePath;
            info.WorkingDirectory = Directory.GetParent(OpenerConfig.Instance.KifuwarabeWcsc29ExePath).FullName;

            // コマンドライン引数を指定する
            info.Arguments = $@"--input ""{input_file.Replace(@"\", "/")}"" --output ""{output_file.Replace(@"\", "/")}""";

            // コンソール・ウィドウを開かない。
            info.CreateNoWindow = true;

            // シェル機能を使用しない
            info.UseShellExecute = false;

            var p = Process.Start(info);

            // タイムアウト時間（秒）。１棋譜に 1分も かからないだろう。
            p.WaitForExit(60 * 1000);

            var returnCode = p.ExitCode;
            if (returnCode != 0)
            {
                Trace.WriteLine($"Eat: returnCode='{returnCode}' {info.FileName} {info.Arguments}");
            }

            return returnCode;
        }

        /// <summary>
        /// ディレクトリーがなければ作るぜ☆（＾～＾）
        /// </summary>
        /// <param name="dir">パス。</param>
        public static void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// CSAファイルは Shift-JIS と決めつけて、UTF8に変換する。
        /// </summary>
        /// <param name="inputFile">ファイル。</param>
        public static void ChangeEncodingFile(string inputFile)
        {
            // Trace.WriteLine($"エンコーディング変換: {inputFile}");

            switch (Path.GetExtension(inputFile).ToUpper())
            {
                case ".CSA":
                case ".KIF":
                    {
                        byte[] bytesData;

                        // ファイルをbyte形で全て読み込み
                        using (FileStream fs1 = new FileStream(inputFile, FileMode.Open))
                        {
                            byte[] data = new byte[fs1.Length];
                            fs1.Read(data, 0, data.Length);
                            fs1.Close();

                            // Shift-JIS -> UTF-8 変換（byte形）
                            string sjisstr = Encoding.GetEncoding("Shift_JIS").GetString(data);
                            bytesData = System.Text.Encoding.UTF8.GetBytes(sjisstr);
                        }

                        // 出力ファイルオープン（バイナリ形式）
                        var outputDir = Path.Combine(FomationOutputDirectory.Instance.Path, Directory.GetParent(inputFile).Name);
                        CreateDirectory(outputDir);
                        var outputFile = Path.Combine(outputDir, Path.GetFileName(inputFile));
                        // Trace.WriteLine($"outputFile: {outputFile}");
                        using (FileStream fs2 = new FileStream(outputFile, FileMode.Create))
                        {
                            // 書き込み設定（デフォルトはUTF-8）
                            BinaryWriter bw = new BinaryWriter(fs2);

                            // 出力ファイルへ全て書き込み
                            bw.Write(bytesData);
                            bw.Close();
                            fs2.Close();
                        }

                        // 終わったファイルを移動。
                        // ExpantionGoPath = C:\shogi-record\go\hunting
                        // InputFilePath   = C:\shogi-record\go\cooking\floodgate\2008\wdoor+floodgate-900-0+a+gps500+20080803103002.csa とかいうファイルパスになっている。
                        var belowPath = inputFile.Substring(FomationGoDirectory.Instance.Path.Length);

                        // var wentDir = Path.Combine(FormationWentPath, Directory.GetParent(inputFile).Name);
                        var wentDir = Path.Combine(FomationWentDirectory.Instance.Path, belowPath);
                        CreateDirectory(wentDir);

                        var wentFile = Path.Combine(wentDir, Path.GetFileName(inputFile));

                        // Trace.WriteLine($"outputFile: {wentFile}");
                        File.Move(inputFile, wentFile);
                    }

                    break;
            }
        }
    }
}
