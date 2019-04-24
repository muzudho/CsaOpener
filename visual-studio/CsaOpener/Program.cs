using System;

[assembly: CLSCompliant(true)]
namespace Grayscale.CsaOpener
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using Grayscale.CsaOpener.Location;

    // [ソリューション エクスプローラー]のプロジェクトの下の[参照]を右クリック、
    // [参照の追加(R)...]をクリック。[アセンブリ] - [フレームワーク] と進み、
    // Microsoft.VisualBasic をチェックしてください。

    /// <summary>
    /// Entry point.
    ///
    /// [DynamicJSONを利用したJSONのパース (C#プログラミング)](https://www.ipentec.com/document/csharp-parse-json-by-using-dynamic-json)
    /// </summary>
    public class Program
    {
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

                // 同じフェーズをずっとやっていても１つも完成しないので、少しずつやって、ばらけさせる。
                var expandedCount = 1;
                var readCount = 0;
                var mergedCount = 0;

                // ループの回った回数が０回になるまで繰り返す。
                while (expandedCount+ readCount+ mergedCount > 0)
                {
                    // 解凍フェーズ。
                    expandedCount = ExpansionPhase.ExpandLittleIt();

                    // TODO フォルダーを探索して、棋譜のエンコーディングを変換。
                    // EncodingPhase.Encode();

                    // 棋譜読取フェーズ。
                    readCount = ReadLitterGameRecord();

                    // たまに行う程度。
                    //if (new System.Random().Next() % 3 == 0)
                    //{
                    // JSON作成フェーズ。
                    mergedCount = MergeLittleRpmoveObj(false);
                    //}

                    Trace.WriteLine($"expandedCount: {expandedCount}, readCount: {readCount}, mergedCount: {mergedCount}.");
                }

                // 最後の余りに対応する１回。
                {
                    // 解凍フェーズ。
                    expandedCount = ExpansionPhase.ExpandLittleIt();

                    // TODO フォルダーを探索して、棋譜のエンコーディングを変換。
                    // EncodingPhase.Encode();

                    // 棋譜読取フェーズ。
                    readCount = ReadLitterGameRecord();

                    // JSON作成フェーズ。
                    mergedCount = MergeLittleRpmoveObj(true);

                    Trace.WriteLine($"LAST: expandedCount: {expandedCount}, readCount: {readCount}, mergedCount: {mergedCount}.");
                }

                // 空の go のサブ・ディレクトリは削除。
                {
                    {
                        // このディレクトリ以下のディレクトリをすべて取得する
                        IEnumerable<string> subDirectories =
                            System.IO.Directory.EnumerateDirectories(ExpansionGoDirectory.Instance.Path, "*", System.IO.SearchOption.TopDirectoryOnly);

                        foreach (string subDir in subDirectories)
                        {
                            DeleteEmptyDirectory(subDir);
                        }
                    }

                    {
                        // このディレクトリ以下のディレクトリをすべて取得する
                        IEnumerable<string> subDirectories =
                            System.IO.Directory.EnumerateDirectories(FomationGoDirectory.Instance.Path, "*", System.IO.SearchOption.TopDirectoryOnly);

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
        /// RPM棋譜の断片（.rpmove ファイル）を600個ぐらい 適当にくっつけて JSONファイルにする。
        /// </summary>
        /// <param name="isLast">余り。</param>
        /// <returns>ループが回った回数。</returns>
        public static int MergeLittleRpmoveObj(bool isLast)
        {
            // Trace.WriteLine($"Merge rpmove obj(A) : kw29Config.eating.output: {kw29Config.eating.output}");

            // 指定ディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> rpmoveFiles =
                System.IO.Directory.EnumerateFiles(
                    EatingOutputDirectory.Instance.Path, "*.rpmove", System.IO.SearchOption.AllDirectories);

            var count = 0;

            // まず、ファイルを 1～600個集める。
            var fileGroup = new List<string>();
            foreach (string rpmoveFile in rpmoveFiles)
            {
                if (fileGroup.Count > 599)
                {
                    break;
                }

                fileGroup.Add(rpmoveFile);
                count++;
            }

            if (!isLast && count < 400)
            {
                Trace.WriteLine($"Break: 数: {fileGroup.Count} < 400。マージをパス。");

                // 400件も溜まってなければ、まだマージしない。
                return count;
            }

            // Trace.WriteLine("Merge rpmove obj(B)...");
            var builder = new StringBuilder();
            foreach (var file in fileGroup)
            {
                builder.AppendLine(File.ReadAllText(file));
            }

            if (builder.Length < 1)
            {
                // Trace.WriteLine($"fileGroup.Count: {fileGroup.Count}, builder.Length: {builder.Length}");

                // 空ファイルを読み込んでいたら無限ループしてしまう。 0 を返す。
                return 0;
            }

            // 最後のコンマを除去する。
            var content = builder.ToString();
            var lastComma = content.LastIndexOf(',');
            content = content.Substring(0, lastComma);

            // JSON形式として読めるように、配列のオブジェクトにする。
            content = string.Concat(@"{""book"": [", content, "]}");

            // 拡張子を .rpmrec にして保存する。ファイル名は適当。
            // ファイル名が被ってしまったら、今回はパス。
            {
                // ランダムな正の数を４つ つなげて長くする。
                var rand = new System.Random();
                var num1 = rand.Next();
                var num2 = rand.Next();
                var num3 = rand.Next();
                var num4 = rand.Next();

                // Trace.WriteLine("Merge rpmove obj(Write1)...");
                var path = Path.Combine(RpmRecordDirectory.Instance.Path, $"{num1}-{num2}-{num3}-{num4}-rpmrec.json");
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, content);

                    // 結合が終わったファイルは消す。
                    foreach (string rpmoveFile in rpmoveFiles)
                    {
                        // Trace.WriteLine($"Remove: {rpmoveFile}");
                        File.Delete(rpmoveFile);
                    }
                }
            }

            // Trace.WriteLine("Merge rpmove obj(End)...");
            return count;
        }

        /// <summary>
        /// 少し棋譜を読み取る。
        /// </summary>
        /// <returns>ループが回った回数。</returns>
        public static int ReadLitterGameRecord()
        {
            // 指定ディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> eatingGoFiles =
                System.IO.Directory.EnumerateFiles(
                    EatingGoDirectory.Instance.Path, "*", System.IO.SearchOption.AllDirectories);

            // 200件回す。
            var count = 0;
            foreach (string eatingGoFile in eatingGoFiles)
            {
                if (count > 199)
                {
                    break;
                }

                AbstractFile anyFile;
                switch (Path.GetExtension(eatingGoFile).ToUpper())
                {
                    case ".CSA":
                        anyFile = new CsaFile(string.Empty, eatingGoFile);
                        break;

                    case ".KIF":
                        anyFile = new KifFile(string.Empty, eatingGoFile);
                        break;

                    default:
                        anyFile = new UnexpectedFile(string.Empty);
                        Rest++;
                        break;
                }

                // 棋譜読取フェーズ。
                anyFile.ReadGameRecord();

                count++;
            }

            return count;
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
                    Trace.WriteLine($"Delete dir: {dir}.");
                    Directory.Delete(dir);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Trace.WriteLine(e);
            }
        }

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
    }
}
