using System;

[assembly: CLSCompliant(true)]
namespace Grayscale.CsaOpener
{
    using System.Collections.Generic;
    using System.IO;
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

            // ファイルを列挙する
            foreach (string f in files)
            {
                switch (Path.GetExtension(f).ToUpper())
                {
                    case ".CSA":
                        Console.WriteLine($"そのままコピー: {f}");
                        var out_path = Path.Combine(arguments.Output, Path.GetFileName(f));
                        ChangeEncodingToUtf8(out_path);
                        File.Copy(f, out_path, true);
                        break;
                    default:
                        Console.WriteLine($"まだ: {f}");
                        break;
                }
            }

            int sleepSeconds = 15;
            Console.WriteLine($"Finished. sleep: '{sleepSeconds}' sec.");
            Thread.Sleep(sleepSeconds * 1000);
        }

        /// <summary>
        /// Shift-JIS と決めつけて、UTF8に変換する。
        /// </summary>
        /// <param name="file">ファイルパス。</param>
        public static void ChangeEncodingToUtf8(string file)
        {
            // ファイルをbyte形で全て読み込み
            FileStream fs1 = new FileStream(file, FileMode.Open);
            byte[] data = new byte[fs1.Length];
            fs1.Read(data, 0, data.Length);
            fs1.Close();

            // Shift-JIS -> UTF-8 変換（byte形）
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            string sjisstr = sjisEnc.GetString(data);
            byte[] bytesData = System.Text.Encoding.UTF8.GetBytes(sjisstr);

            // string型に変換したい場合はこんな感じに
            // Encoding utf8Enc = Encoding.GetEncoding("UTF-8");
            // string utf = utf8Enc.GetString(bytesData);
            // Console.WriteLine(utf);

            // 出力ファイルオープン（バイナリ形式）
            FileStream fs2 = new FileStream(file, FileMode.Create);

            // 書き込み設定（デフォルトはUTF-8）
            BinaryWriter bw = new BinaryWriter(fs2);

            // 出力ファイルへ全て書き込み
            bw.Write(bytesData);
            bw.Close();
            fs2.Close();
        }
    }
}
