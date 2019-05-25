namespace Grayscale.ShogiKifuConverter
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;

    /// <summary>
    /// 解凍フェーズ。
    /// </summary>
    public class ExpansionPhase
    {
        /// <summary>
        /// Gets or sets a 処理できなかったファイル数。
        /// </summary>
        public static int Rest { get; set; }

        /// <summary>
        /// 少し解凍。
        /// </summary>
        /// <returns>ループが回った回数。</returns>
        public static int ExpandLittleIt()
        {
            Trace.WriteLine($"{LogHelper.Stamp}Expand  : Start...");

            // 指定ディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> expansionGoFiles =
                System.IO.Directory.EnumerateFiles(
                    LocationMaster.ConverterInputDirectory.FullName, "*", System.IO.SearchOption.AllDirectories);

            Rest = 0;

            // 圧縮ファイルを 3つ 解凍する
            var count = 0;
            foreach (string expansionGoFileFullName in expansionGoFiles)
            {
                var expansionGoFile = new TraceableFile(expansionGoFileFullName);

                if (count > 3)
                {
                    break;
                }

                AbstractFile anyFile;
                switch (Path.GetExtension(expansionGoFile.FullName).ToUpperInvariant())
                {
                    case ".7Z":
                        anyFile = new SevenZipFile(expansionGoFile);
                        break;

                    case ".CSA":
                        anyFile = new CsaFile(expansionGoFile, new TraceableFile(string.Empty));
                        break;

                    case ".KIF":
                        anyFile = new KifFile(expansionGoFile, new TraceableFile(string.Empty));
                        break;

                    case ".LZH":
                        anyFile = new LzhFile(expansionGoFile);
                        break;

                    case ".TGZ":
                        anyFile = new TargzFile(expansionGoFile);
                        break;

                    case ".ZIP":
                        anyFile = new ZipArchiveFile(expansionGoFile);
                        break;

                    default:
                        anyFile = new UnexpectedFile(expansionGoFile);
                        Rest++;
                        break;
                }

                // 解凍する。
                if (anyFile.Expand())
                {
                }

                count++;
            }

            // Trace.WriteLine($"{LogHelper.Stamp}むり1: {Rest}");
            return (count);
        }
    }
}
