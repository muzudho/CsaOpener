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
                    IEnumerable<string> expansionGoFiles =
                        System.IO.Directory.EnumerateFiles(
                            config.ExpansionGoPath, "*", System.IO.SearchOption.AllDirectories);

                    Rest = 0;

                    // 圧縮ファイルを解凍する
                    foreach (string expansionGoFile in expansionGoFiles)
                    {
                        AbstractFile anyFile;
                        switch (Path.GetExtension(expansionGoFile).ToUpper())
                        {
                            case ".7Z":
                                anyFile = new SevenZipFile(config, expansionGoFile);
                                break;

                            case ".CSA":
                                anyFile = new CsaFile(config, expansionGoFile, string.Empty);
                                break;

                            case ".KIF":
                                anyFile = new KifFile(config, expansionGoFile, string.Empty);
                                break;

                            case ".LZH":
                                anyFile = new LzhFile(config, expansionGoFile);
                                break;

                            case ".TGZ":
                                anyFile = new TargzFile(config, expansionGoFile);
                                break;

                            case ".ZIP":
                                anyFile = new ZipArchiveFile(config, expansionGoFile);
                                break;

                            default:
                                anyFile = new UnexpectedFile(config, expansionGoFile);
                                Rest++;
                                break;
                        }

                        // 解凍する。
                        anyFile.Expand();

                        // エンコーディングを変える。
                        Commons.ChangeEncodingFile(config, expansionGoFile);
                    }

                    Trace.WriteLine($"むり1: {Rest}");
                }

                // 棋譜読取フェーズ
                {
                    // 指定ディレクトリ以下のファイルをすべて取得する
                    IEnumerable<string> eatingGoFiles =
                        System.IO.Directory.EnumerateFiles(
                            config.EatingGoPath, "*", System.IO.SearchOption.AllDirectories);

                    foreach (string expansionGoFile in eatingGoFiles)
                    {
                        AbstractFile anyFile;
                        switch (Path.GetExtension(expansionGoFile).ToUpper())
                        {
                            case ".CSA":
                                anyFile = new CsaFile(config, string.Empty, expansionGoFile);
                                break;

                            case ".KIF":
                                anyFile = new KifFile(config, string.Empty, expansionGoFile);
                                break;

                            default:
                                anyFile = new UnexpectedFile(config, string.Empty);
                                Rest++;
                                break;
                        }

                        // 棋譜読取フェーズ。
                        anyFile.ReadGameRecord();
                    }
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
    }
}
