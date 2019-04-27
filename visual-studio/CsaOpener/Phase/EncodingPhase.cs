namespace Grayscale.CsaOpener
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// エンコーディング・フェーズ。
    /// </summary>
    public class EncodingPhase
    {
        /// <summary>
        /// エンコーディングを変える。
        /// 解凍した先のディレクトリを検索すること。
        /// </summary>
        /// <returns>ループが回った回数。</returns>
        public static int ExecuteEncode()
        {
            var encodedCount = 0;
            Trace.WriteLine($"Entry   : '{ExpansionOutputDirectory.Instance.Path}' directory.");

            // 指定ディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    ExpansionOutputDirectory.Instance.Path, "*", System.IO.SearchOption.AllDirectories);

            // Trace.WriteLine("Expanding...");

            // 圧縮ファイルを 3つ 解凍する
            foreach (string file in files)
            {
                if (encodedCount > 3)
                {
                    goto next;
                }

                if (EncodingPhase.EncodingOfTextFile(file))
                {
                    encodedCount++;
                }
            }

        next:
            Trace.WriteLine("End     : Encoding.");
            return encodedCount;
        }

        /// <summary>
        /// CSAファイルは Shift-JIS と決めつけて、UTF8に変換する。
        /// </summary>
        /// <param name="textFile">棋譜のテキストファイル。圧縮ファイルではなく。</param>
        /// <returns>エンコーディング変換した。</returns>
        private static bool EncodingOfTextFile(string textFile)
        {
            var encoded = false;
            Trace.WriteLine($"Encode  : エンコーディング変換対象: {textFile}");
            switch (Path.GetExtension(textFile).ToUpperInvariant())
            {
                case ".CSA":
                case ".KIF":
                    {
                        byte[] bytesData;

                        // ファイルをbyte形で全て読み込み
                        using (FileStream fs1 = new FileStream(textFile, FileMode.Open))
                        {
                            byte[] data = new byte[fs1.Length];
                            fs1.Read(data, 0, data.Length);
                            fs1.Close();

                            // Shift-JIS -> UTF-8 変換（byte形）
                            string sjisstr = Encoding.GetEncoding("Shift_JIS").GetString(data);
                            bytesData = System.Text.Encoding.UTF8.GetBytes(sjisstr);
                        }

                        // 出力ファイル
                        var outputFile = Path.Combine(FomationOutputDirectory.Instance.Path, Path.GetFileName(textFile));
                        Trace.WriteLine($"outputFile: {outputFile}");

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
                        var belowPath = textFile.Substring(FomationGoDirectory.Instance.Path.Length);

                        // var wentDir = Path.Combine(FormationWentPath, Directory.GetParent(inputFile).Name);
                        var wentFile = new TraceableFile(Path.Combine(FomationWentDirectory.Instance.FullName, belowPath.TrimStart('/', '\\')));
                        Trace.WriteLine($"FomationWentDirectory.Instance.FullName: '{FomationWentDirectory.Instance.FullName}'. belowPath: '{belowPath}'. wentFile.FullName: '{wentFile.FullName}'.");

                        var wentParentDir = new TraceableDirectory(System.IO.Directory.GetParent(wentFile.FullName).FullName);
                        wentParentDir.Create();

                        try
                        {
                            new TraceableFile(textFile).Move(wentFile.FullName);
                        }
                        catch (IOException e)
                        {
                            Trace.WriteLine(e);
                        }
                    }

                    encoded = true;
                    break;

                default:
                    Trace.WriteLine("対象外。");
                    break;
            }

            Trace.WriteLine("Encode  : End.");
            return encoded;
        }
    }
}
