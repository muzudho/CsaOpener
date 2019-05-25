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

                if (EncodingPhase.EncodingOfTextFile(new TraceableFile(file)))
                {
                    encodedCount++;
                }
            }

        next:
            Trace.WriteLine($"{LogHelper.Stamp}End     : Encoding.");
            return encodedCount;
        }

        /// <summary>
        /// エンコーディング変換。
        /// </summary>
        /// <param name="expandedFile">解凍済み棋譜のテキストファイル。</param>
        /// <returns>エンコーディング変換した。</returns>
        private static bool EncodingOfTextFile(TraceableFile expandedFile)
        {
            Trace.WriteLine($"{LogHelper.Stamp}Encode  : エンコーディング変換対象: {expandedFile.FullName}");
            var (parentDirectory, stem, extensionWithDot) = PathHelper.DestructFileName(expandedFile.FullName);
            Trace.WriteLine($"{LogHelper.Stamp}ParentDirectory={parentDirectory}, Stem={stem}, ExtensionWithDot={extensionWithDot}.");

            // 出力先ディレクトリー。
            var outputDir = LocationMaster.ConverterWorkingDirectory;

            var encoded = false;
            switch (extensionWithDot.ToUpperInvariant())
            {
                case ".CSA":
                case ".KIF":
                    // TODO 両方試したい。 (1)変換なし (2)Shift-JIS -> UTF-8 変換
                    {
                        // 名前を変えて、そのまま保存します。
                        new Encords().ExecuteAsIt(expandedFile, outputDir);

                        // Shift-JIS と決めつけて、UTF8に変換する。
                        new Encords().ExecuteSjisToU8(expandedFile, outputDir);

                        // 終わったファイルは消す。
                        try
                        {
                            expandedFile.Delete();
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

            Trace.WriteLine($"{LogHelper.Stamp}Encode  : End.");
            return encoded;
        }
    }
}
