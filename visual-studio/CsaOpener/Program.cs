using System;

[assembly: CLSCompliant(true)]
namespace Grayscale.CsaOpener
{
    using SevenZipExtractor;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            var arguments = Arguments.Load(args);
            Console.WriteLine($"Input directory: '{arguments.Input}', Output directory: '{arguments.Output}'.");

            // カレントディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    arguments.Input, "*", System.IO.SearchOption.AllDirectories);

            int rest = 0;

            // ファイルを列挙する
            foreach (string f in files)
            {
                switch (Path.GetExtension(f).ToUpper())
                {
                    case ".CSA":
                        {
                            // そのままコピーすると名前がぶつかってしまう☆（＾～＾）
                            var out_dir = Path.Combine(arguments.Output, $"copied-{Path.GetFileNameWithoutExtension(f)}");
                            CreateDirectory(out_dir);
                            var out_path = Path.Combine(out_dir, Path.GetFileName(f));
                            Console.WriteLine($"コピー: {f} -> {out_path}");
                            File.Copy(f, out_path, true);

                            // エンコーディングを変える。
                            ChangeEncodingToUtf8(out_path);
                        }

                        break;

                    case ".LZH":
                        {
                            // 中に何入ってるか分からん。名前が被るかもしれない。
                            var out_dir = Path.Combine(arguments.Output, $"extracted-{Path.GetFileNameWithoutExtension(f)}");
                            Console.WriteLine($"LZHを解凍: {f} -> {out_dir}");
                            Unlzh(f, out_dir);
                        }

                        break;

                    case ".ZIP":
                        {
                            // 中に何入ってるか分からん。名前が被るかもしれない。
                            var out_dir = Path.Combine(arguments.Output, $"extracted-{Path.GetFileNameWithoutExtension(f)}");
                            Console.WriteLine($"ZIPを解凍: {f} -> {out_dir}");
                            Unzip(f, out_dir);

                            // Extract(f, out_dir);
                        }

                        break;

                    default:
                        Console.WriteLine($"むり: {f}");
                        rest++;
                        break;
                }
            }

            Console.WriteLine($"まだ: {rest}");

            int sleepSeconds = 15;
            Console.WriteLine($"Finished. sleep: {sleepSeconds} sec.");
            Thread.Sleep(sleepSeconds * 1000);
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

        /*
        /// <summary>
        /// 解凍する。
        /// </summary>
        /// <param name="inputFile">入力ファイルパス。</param>
        /// <param name="outputDirectory">出力ディレクトリーパス。</param>
        public static void Extract(string inputFile, string outputDirectory)
        {
            var ar = new ArchiveFile(inputFile);

            try
            {
                // 解凍の開始☆（＾～＾）
                ar.Extract(outputDirectory, true);

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
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }
        */

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

        /// <summary>
        /// Shift-JIS と決めつけて、UTF8に変換する。
        /// </summary>
        /// <param name="file">ファイルパス。</param>
        public static void ChangeEncodingToUtf8(string file)
        {
            byte[] bytesData;
            // ファイルをbyte形で全て読み込み
            using (FileStream fs1 = new FileStream(file, FileMode.Open))
            {
                byte[] data = new byte[fs1.Length];
                fs1.Read(data, 0, data.Length);
                fs1.Close();

                // Shift-JIS -> UTF-8 変換（byte形）
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                string sjisstr = sjisEnc.GetString(data);
                bytesData = System.Text.Encoding.UTF8.GetBytes(sjisstr);

                // string型に変換したい場合はこんな感じに
                // Encoding utf8Enc = Encoding.GetEncoding("UTF-8");
                // string utf = utf8Enc.GetString(bytesData);
                // Console.WriteLine(utf);
            }

            // 出力ファイルオープン（バイナリ形式）
            using (FileStream fs2 = new FileStream(file, FileMode.Create))
            {
                // 書き込み設定（デフォルトはUTF-8）
                BinaryWriter bw = new BinaryWriter(fs2);

                // 出力ファイルへ全て書き込み
                bw.Write(bytesData);
                bw.Close();
                fs2.Close();
            }
        }
    }
}
