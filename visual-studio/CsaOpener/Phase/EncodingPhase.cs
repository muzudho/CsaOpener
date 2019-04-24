namespace Grayscale.CsaOpener
{
    using System.Collections.Generic;

    /// <summary>
    /// エンコーディング・フェーズ。
    /// </summary>
    public class EncodingPhase
    {
        /// <summary>
        /// エンコーディングを変える。
        /// 解凍した先のディレクトリを検索すること。
        /// </summary>
        /// <param name="directories">ディレクトリー。</param>
        /// <returns>ループが回った回数。</returns>
        public static int Encode(List<string> directories)
        {
            var count = 0;
            foreach (var directory in directories)
            {
                // 指定ディレクトリ以下のファイルをすべて取得する
                IEnumerable<string> files =
                    System.IO.Directory.EnumerateFiles(
                        directory, "*", System.IO.SearchOption.AllDirectories);

                // Trace.WriteLine("Expanding...");

                // 圧縮ファイルを 3つ 解凍する
                foreach (string file in files)
                {
                    if (count > 3)
                    {
                        goto next;
                    }

                    Commons.ChangeEncodingOfTextFile(file);
                    count++;
                }
            }

            next:
            return count;
        }
    }
}
