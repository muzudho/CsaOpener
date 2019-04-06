using System;

[assembly: CLSCompliant(true)]
namespace Grayscale.CsaOpener
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using Codeplex.Data;
    using ICSharpCode.SharpZipLib.GZip;
    using ICSharpCode.SharpZipLib.Tar;

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
        /// Gets or sets a これからやるパス。
        /// </summary>
        public static string ExpansionGoPath { get; set; }

        /// <summary>
        /// Gets or sets a やり終わったパス。
        /// </summary>
        public static string ExpansionWentPath { get; set; }

        /// <summary>
        /// Gets or sets a 出力先のパス。
        /// </summary>
        public static string ExpansionOutputPath { get; set; }

        /// <summary>
        /// Gets or sets a これからやるパス。
        /// </summary>
        public static string FormationGoPath { get; set; }

        /// <summary>
        /// Gets or sets a やり終わったパス。
        /// </summary>
        public static string FormationWentPath { get; set; }

        /// <summary>
        /// Gets or sets a 出力先のパス。
        /// </summary>
        public static string FormationOutputPath { get; set; }

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
            /*
            // Command line arguments.
            var arguments = Arguments.Load(args);
            Console.WriteLine($"Input directory: '{arguments.Input}', Output directory: '{arguments.Output}'.");
             */

            // Config file.
            var json = File.ReadAllText("./config.json");
            dynamic config = DynamicJson.Parse(json);

            ExpansionGoPath = config.expansion.go; // arguments.Input
            ExpansionWentPath = config.expansion.went;
            ExpansionOutputPath = config.expansion.output; // arguments.Output
            FormationGoPath = config.formation.go;
            FormationWentPath = config.formation.went;
            FormationOutputPath = config.formation.output;

            // 解凍フェーズ。
            {
                // 指定ディレクトリ以下のファイルをすべて取得する
                IEnumerable<string> files =
                    System.IO.Directory.EnumerateFiles(
                        ExpansionGoPath, "*", System.IO.SearchOption.AllDirectories);

                Rest = 0;

                // ファイルを列挙する
                foreach (string f in files)
                {
                    // 解凍する。
                    Expand(f);
                }

                Console.WriteLine($"むり1: {Rest}");
            }

            // エンコーディング フェーズ
            {
                // 指定ディレクトリ以下のファイルをすべて取得する
                IEnumerable<string> files =
                    System.IO.Directory.EnumerateFiles(
                        FormationGoPath, "*", System.IO.SearchOption.AllDirectories);

                Rest = 0;

                // ファイルを列挙する
                foreach (string f in files)
                {
                    // エンコーディングを変える。
                    ChangeEncodingFile(f);
                }

                Console.WriteLine($"むり2: {Rest}");
            }

            int sleepSeconds = 15;
            Console.WriteLine($"Finished. sleep: {sleepSeconds} sec.");
            Thread.Sleep(sleepSeconds * 1000);
        }

        /// <summary>
        /// 解凍フェーズ。
        /// </summary>
        /// <param name="f">ファイル</param>
        public static void Expand(string f)
        {
            Console.WriteLine($"解凍: {f}");

            switch (Path.GetExtension(f).ToUpper())
            {
                case ".7Z":
                    {
                        // 中に何入ってるか分からん。名前が被るかもしれない。
                        var out_dir = Path.Combine(
                            ExpansionOutputPath,
                            Directory.GetParent(f).Name,
                            $"extracted-{Path.GetFileNameWithoutExtension(f)}").Replace(@"\", "/");
                        f = f.Replace(@"\", "/");

                        // Console.WriteLine($"7Zを解凍: {f} -> {out_dir}");
                        new Program().Un7z(f, out_dir);

                        var wentDir = Path.Combine(ExpansionWentPath, Directory.GetParent(f).Name);
                        CreateDirectory(wentDir);

                        var wentFile = Path.Combine(wentDir, Path.GetFileName(f));

                        // 解凍が終わった元ファイルを移動。
                        File.Move(f, wentFile);
                    }

                    break;

                case ".CSA":
                    {
                        // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                        var out_dir = Path.Combine(ExpansionOutputPath, $"copied-{Path.GetFileNameWithoutExtension(f)}");
                        CreateDirectory(out_dir);
                        var out_path = Path.Combine(out_dir, Path.GetFileName(f));

                        // 成果物の作成。
                        File.Copy(f, out_path, true);

                        // 解凍が終わった元ファイルを移動。
                        File.Move(f, Path.Combine(ExpansionWentPath, Path.GetFileName(f)));
                    }

                    break;

                case ".KIF":
                    {
                        // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                        var out_dir = Path.Combine(ExpansionOutputPath, $"copied-{Path.GetFileNameWithoutExtension(f)}");
                        CreateDirectory(out_dir);
                        var out_path = Path.Combine(out_dir, Path.GetFileName(f));

                        // Console.WriteLine($"コピー: {f} -> {out_path}");
                        File.Copy(f, out_path, true);

                        // 解凍が終わった元ファイルを移動。
                        File.Move(f, Path.Combine(ExpansionWentPath, Path.GetFileName(f)));
                    }

                    break;

                case ".LZH":
                    {
                        // 中に何入ってるか分からん。名前が被るかもしれない。
                        var out_dir = Path.Combine(ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(f)}");

                        // Console.WriteLine($"LZHを解凍: {f} -> {out_dir}");
                        Unlzh(f, out_dir);

                        // 解凍が終わった元ファイルを移動。
                        File.Move(f, Path.Combine(ExpansionWentPath, Path.GetFileName(f)));
                    }

                    break;

                case ".TGZ":
                    {
                        // 中に何入ってるか分からん。名前が被るかもしれない。
                        var out_dir = Path.Combine(ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(f)}");

                        // Console.WriteLine($"TGZを解凍: {f} -> {out_dir}");
                        Untgz(f, out_dir);

                        // 解凍が終わった元ファイルを移動。
                        File.Move(f, Path.Combine(ExpansionWentPath, Path.GetFileName(f)));
                    }

                    break;

                case ".ZIP":
                    {
                        // 中に何入ってるか分からん。名前が被るかもしれない。
                        var out_dir = Path.Combine(ExpansionOutputPath, $"extracted-{Path.GetFileNameWithoutExtension(f)}");

                        // Console.WriteLine($"ZIPを解凍: {f} -> {out_dir}");
                        Unzip(f, out_dir);

                        // 解凍が終わった元ファイルを移動。
                        File.Move(f, Path.Combine(ExpansionWentPath, Path.GetFileName(f)));
                    }

                    break;

                default:
                    {
                        // .exe とか解凍できないやつが入っている☆（＾～＾）！
                        Console.WriteLine($"むり: {f}");

                        // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                        var out_dir = Path.Combine(ExpansionOutputPath, $"copied-{Path.GetFileNameWithoutExtension(f)}");
                        CreateDirectory(out_dir);
                        var out_path = Path.Combine(out_dir, Path.GetFileName(f));

                        // Console.WriteLine($"コピー: {f} -> {out_path}");
                        File.Copy(f, out_path, true);

                        // 無理だった元ファイルを移動。
                        File.Move(f, Path.Combine(ExpansionWentPath, Path.GetFileName(f)));

                        Rest++;
                    }

                    break;
            }
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
        /// 解凍する。
        /// </summary>
        /// <param name="inputFile">入力ファイルパス。</param>
        /// <param name="outputDirectory">出力ディレクトリーパス。</param>
        public static void Untgz(string inputFile, string outputDirectory)
        {
            Stream inStream = File.OpenRead(inputFile);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(outputDirectory);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <param name="inputFile">入力ファイルパス。</param>
        /// <param name="outputDirectory">出力ディレクトリーパス。</param>
        public static void Unzip(string inputFile, string outputDirectory)
        {
            ZipFile.ExtractToDirectory(inputFile, outputDirectory);
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <param name="inputFile">入力ファイルパス。</param>
        /// <param name="outputDirectory">出力ディレクトリーパス。</param>
        public static void Unlzh(string inputFile, string outputDirectory)
        {
            LzhManager.fnExtract(inputFile, outputDirectory);
        }

        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <param name="inputFile">入力ファイルパス。</param>
        /// <param name="outputDirectory">出力ディレクトリーパス。</param>
        public void Un7z(string inputFile, string outputDirectory)
        {
            try
            {
                SevenZManager.fnExtract(inputFile, outputDirectory);

                /*
                var preCount = 0;

                // 一番でかい圧縮ファイルの解凍に必要な秒数☆（＾～＾）
                var sleepSeconds = 10;

                // ディレクトリの中のファイル数を監視☆（＾～＾）
                while (true)
                {
                    var curCount = CountFiles(outputDirectory);
                    if (preCount == curCount)
                    {
                        Console.WriteLine($"もう抜けていいだろうか☆（＾～＾）");
                        break;
                    }

                    Console.WriteLine($"Cur count: {curCount}.");
                    preCount = curCount;

                    // ゆっくり☆（＾～＾）！
                    Thread.Sleep(sleepSeconds * 1000);
                    sleepSeconds++;
                }
                */
            }
            catch (BadImageFormatException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }

        /*
        /// <summary>
        /// ディレクトリー下のファイル数をカウントだぜ☆（＾～＾）
        /// </summary>
        /// <param name="dir">ディレクトリー。</param>
        /// <returns>ファイル数</returns>
        public static int CountFiles(string dir)
        {
            int sum = 0;
            sum += Directory.GetFiles(dir, "*", System.IO.SearchOption.AllDirectories).Length;

            // ショートカット踏んで 無限ループするなよ☆（＾～＾）
            foreach (var subDir in Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine($"Sub dir: {subDir}. Sum: {sum}.");
                sum += CountFiles(subDir);
            }

            return sum;
        }
        */

        /// <summary>
        /// CSAファイルは Shift-JIS と決めつけて、UTF8に変換する。
        /// </summary>
        /// <param name="inputFile">ファイル。</param>
        public static void ChangeEncodingFile(string inputFile)
        {
            Console.WriteLine($"エンコーディング変換: {inputFile}");

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
                        var outputFile = Path.Combine(FormationOutputPath, Path.GetFileName(inputFile));
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
                        var wentFile = Path.Combine(FormationWentPath, Path.GetFileName(inputFile));
                        File.Move(inputFile, wentFile);
                    }

                    break;
            }
        }
    }
}
