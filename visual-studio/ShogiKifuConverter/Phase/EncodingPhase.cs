namespace Grayscale.ShogiKifuConverter
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;

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
            Trace.WriteLine($"{LogHelper.Stamp}Encode  : '{LocationMaster.ConverterExpandDirectory.FullName}' directory.");

            var encodedCount = 0;

            // 指定ディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    LocationMaster.ConverterExpandDirectory.FullName, "*", System.IO.SearchOption.AllDirectories);

            // Trace.WriteLine("Expanding...");

            // 圧縮ファイルを 3つ 解凍する
            foreach (string file in files)
            {
                if (encodedCount > 3)
                {
                    goto next;
                }

                var fileW = FileWaitingToBeEncoded.FromFile(file);

                if (EncodingPhase.EncodingOfTextFile(fileW))
                {
                    encodedCount++;
                }
            }

        next:
            Trace.WriteLine("{LogHelper.Stamp}End     : Encoding.");
            return encodedCount;
        }

        /// <summary>
        /// CSAファイルは Shift-JIS と決めつけて、UTF8に変換する。
        /// </summary>
        /// <param name="fileW">棋譜のテキストファイル。圧縮ファイルではなく。</param>
        /// <returns>エンコーディング変換した。</returns>
        private static bool EncodingOfTextFile(FileWaitingToBeEncoded fileW)
        {
            var encoded = false;
            Trace.WriteLine($"{LogHelper.Stamp}Encode  : エンコーディング変換対象: {fileW.GoFile.FullName}");
            switch (Path.GetExtension(fileW.GoFile.FullName).ToUpperInvariant())
            {
                case ".CSA":
                case ".KIF":
                    {
                        byte[] bytesData;

                        // ファイルをbyte形で全て読み込み
                        using (FileStream fs1 = new FileStream(fileW.GoFile.FullName, FileMode.Open))
                        {
                            byte[] data = new byte[fs1.Length];
                            fs1.Read(data, 0, data.Length);
                            fs1.Close();

                            // Shift-JIS -> UTF-8 変換（byte形）
                            string sjisstr = Encoding.GetEncoding("Shift_JIS").GetString(data);
                            bytesData = System.Text.Encoding.UTF8.GetBytes(sjisstr);
                        }

                        // 出力ファイル
                        Trace.WriteLine($"{LogHelper.Stamp}outputFile: {fileW.OutputFile.FullName}");

                        using (FileStream fs2 = new FileStream(fileW.OutputFile.FullName, FileMode.Create))
                        {
                            // 書き込み設定（デフォルトはUTF-8）
                            BinaryWriter bw = new BinaryWriter(fs2);

                            // 出力ファイルへ全て書き込み
                            bw.Write(bytesData);
                            bw.Close();
                            fs2.Close();
                        }

                        // 終わったファイルを移動。
                        /*
                        // ExpantionGoPath = C:\shogi-record\go\hunting
                        // InputFilePath   = C:\shogi-record\go\cooking\floodgate\2008\wdoor+floodgate-900-0+a+gps500+20080803103002.csa とかいうファイルパスになっている。
                        var belowPath = textFile.Substring(FileSystem.FomationGoDirectory.FullName.Length);

                        // var wentDir = PathHelper.Combine(FormationWentPath, Directory.GetParent(inputFile).Name);
                        var wentFile = new TraceableFile(PathHelper.Combine(FileSystem.FomationWentDirectory.FullName, belowPath));

                        Trace.WriteLine($"FomationWentDirectory.Instance.FullName: '{FileSystem.FomationWentDirectory.FullName}'. belowPath: '{belowPath}'. wentFile.FullName: '{wentFile.FullName}'.");
                        wentFile.CreateParentDirectory();
                        */

                        try
                        {
                            fileW.GoFile.Move(fileW.WentFile);
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

            Trace.WriteLine("{LogHelper.Stamp}Encode  : End.");
            return encoded;
        }
    }
}
