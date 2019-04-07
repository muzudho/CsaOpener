using System;

[assembly: CLSCompliant(true)]
namespace Grayscale.CsaOpener
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Codeplex.Data;
    using ICSharpCode.SharpZipLib.GZip;
    using ICSharpCode.SharpZipLib.Tar;

    // [ソリューション エクスプローラー]のプロジェクトの下の[参照]を右クリック、
    // [参照の追加(R)...]をクリック。[アセンブリ] - [フレームワーク] と進み、
    // Microsoft.VisualBasic をチェックしてください。
    using VBFileIO = Microsoft.VisualBasic.FileIO;
    using VBLogging = Microsoft.VisualBasic.Logging;

    /// <summary>
    /// Entry point.
    ///
    /// [DynamicJSONを利用したJSONのパース (C#プログラミング)](https://www.ipentec.com/document/csharp-parse-json-by-using-dynamic-json)
    /// </summary>
    public class Program
    {
        /// <summary>
        /// [DLL Import] SevenZipのコマンドラインメソッド
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル(=0)</param>
        /// <param name="szCmdLine">コマンドライン</param>
        /// <param name="szOutput">実行結果文字列</param>
        /// <param name="dwSize">実行結果文字列格納サイズ</param>
        /// <returns>
        /// 0:正常、0以外:異常終了
        /// </returns>
        [DllImport("7-zip32.dll", CharSet = CharSet.Ansi)]
        private static extern int SevenZip(IntPtr hwnd, string szCmdLine, StringBuilder szOutput, int dwSize);

        /// <summary>
        /// Gets or sets a 処理できなかったファイル数。
        /// </summary>
        public static int Rest { get; set; }

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            LogRotation.Logging(() =>
            {
                /*
                // Command line arguments.
                var arguments = Arguments.Load(args);
                Trace.WriteLine($"Input directory: '{arguments.Input}', Output directory: '{arguments.Output}'.");
                 */

                // Config file.
                var config = Config.Load();

                // 解凍フェーズ。
                {
                    // 指定ディレクトリ以下のファイルをすべて取得する
                    IEnumerable<string> files =
                        System.IO.Directory.EnumerateFiles(
                            config.ExpansionGoPath, "*", System.IO.SearchOption.AllDirectories);

                    Rest = 0;

                    // 圧縮ファイルを解凍する
                    foreach (string f in files)
                    {
                        RecordArchiveFile recordArchiveFile;
                        switch (Path.GetExtension(f).ToUpper())
                        {
                            case ".7Z":
                                recordArchiveFile = new SevenZipFile(config, f);
                                break;

                            case ".CSA":
                                recordArchiveFile = new CsaFile(config, f);
                                break;

                            case ".KIF":
                                recordArchiveFile = new KifFile(config, f);
                                break;

                            case ".LZH":
                                recordArchiveFile = new LzhFile(config, f);
                                break;

                            case ".TGZ":
                                recordArchiveFile = new TargzFile(config, f);
                                break;

                            case ".ZIP":
                                recordArchiveFile = new ZipArchiveFile(config, f);
                                break;

                            default:
                                recordArchiveFile = new UnexpectedFile(config, f);
                                Rest++;
                                break;
                        }

                        // 解凍する。
                        recordArchiveFile.Expand();
                    }

                    Trace.WriteLine($"むり1: {Rest}");
                }

                // エンコーディング フェーズ
                {
                    // 指定ディレクトリ以下のファイルをすべて取得する
                    IEnumerable<string> files =
                        System.IO.Directory.EnumerateFiles(
                            config.FormationGoPath, "*", System.IO.SearchOption.AllDirectories);

                    Rest = 0;

                    // ファイルを列挙する
                    foreach (string f in files)
                    {
                        try
                        {
                            // エンコーディングを変える。
                            ChangeEncodingFile(config, f);
                        }
                        catch (DirectoryNotFoundException e)
                        {
                            Trace.WriteLine(e);
                        }
                    }

                    Trace.WriteLine($"むり2: {Rest}");
                }

                // 空の go のサブ・ディレクトリは削除。
                {
                    {
                        // このディレクトリ以下のディレクトリをすべて取得する
                        IEnumerable<string> subDirectories =
                            System.IO.Directory.EnumerateDirectories(config.ExpansionGoPath, "*", System.IO.SearchOption.TopDirectoryOnly);

                        foreach (string subDir in subDirectories)
                        {
                            DeleteEmptyDirectory(subDir);
                        }
                    }

                    {
                        // このディレクトリ以下のディレクトリをすべて取得する
                        IEnumerable<string> subDirectories =
                            System.IO.Directory.EnumerateDirectories(config.FormationGoPath, "*", System.IO.SearchOption.TopDirectoryOnly);

                        foreach (string subDir in subDirectories)
                        {
                            DeleteEmptyDirectory(subDir);
                        }
                    }
                }

                int sleepSeconds = 60;
                Trace.WriteLine($"Finished. sleep: {sleepSeconds} sec.");
                Thread.Sleep(sleepSeconds * 1000);
            });
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
        /// 中身が空のディレクトリーは削除するぜ☆（＾～＾）
        /// </summary>
        /// <param name="dir">ディレクトリー。</param>
        public static void DeleteEmptyDirectory(string dir)
        {
            // このディレクトリ以下のディレクトリをすべて取得する
            IEnumerable<string> subDirectories =
                System.IO.Directory.EnumerateDirectories(dir, "*", System.IO.SearchOption.TopDirectoryOnly);

            // 再帰。
            foreach (string subDir in subDirectories)
            {
                DeleteEmptyDirectory(subDir);
            }

            try
            {
                // このディレクトリのファイル数を取得する
                if (Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Length == 0)
                {
                    // ファイルがなければ削除する。
                    Trace.WriteLine($"削除: {dir}.");
                    Directory.Delete(dir);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Trace.WriteLine(e);
            }
        }

        /// <summary>
        /// CSAファイルは Shift-JIS と決めつけて、UTF8に変換する。
        /// </summary>
        /// <param name="config">設定。</param>
        /// <param name="inputFile">ファイル。</param>
        public static void ChangeEncodingFile(Config config, string inputFile)
        {
            Trace.WriteLine($"エンコーディング変換: {inputFile}");

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
                        var outputDir = Path.Combine(config.FormationOutputPath, Directory.GetParent(inputFile).Name);
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
                        var belowPath = inputFile.Substring(config.FormationGoPath.Length);

                        // var wentDir = Path.Combine(FormationWentPath, Directory.GetParent(inputFile).Name);
                        var wentDir = Path.Combine(config.FormationWentPath, belowPath);
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
