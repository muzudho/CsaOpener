namespace Grayscale.CsaOpener
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
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
        /// <param name="directory">ディレクトリー。</param>
        /// <returns>ループが回った回数。</returns>
        public static int Encode(string directory)
        {
            // 指定ディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    directory, "*", System.IO.SearchOption.AllDirectories);

            // Trace.WriteLine("Expanding...");

            // 圧縮ファイルを 3つ 解凍する
            var count = 0;
            foreach (string file in files)
            {
                if (count > 3)
                {
                    break;
                }

                Commons.ChangeEncodingOfTextFile(file);
                count++;
            }

            return count;
        }
    }
}
