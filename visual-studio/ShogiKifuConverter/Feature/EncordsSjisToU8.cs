namespace Grayscale.ShogiKifuConverter
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.ShogiKifuConverter.Commons;
    using Grayscale.ShogiKifuConverter.Location;

    /// <summary>
    /// Shift-JIS から UTF-8 に変換します。
    /// </summary>
    public class EncordsSjisToU8
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncordsSjisToU8"/> class.
        /// </summary>
        public EncordsSjisToU8()
        {
        }

        /// <summary>
        /// エンコーディング変換します。
        /// </summary>
        /// <param name="inputFile">棋譜のテキストファイル。圧縮ファイルではなく。</param>
        /// <param name="outputFile">エンコーディング変換後の棋譜のテキストファイル。</param>
        public void Execute(TraceableFile inputFile, TraceableFile outputFile)
        {
            byte[] bytesData;

            // ファイルをbyte形で全て読み込み
            using (FileStream fs1 = new FileStream(inputFile.FullName, FileMode.Open))
            {
                byte[] data = new byte[fs1.Length];
                fs1.Read(data, 0, data.Length);
                fs1.Close();

                // Shift-JIS -> UTF-8 変換（byte形）
                string sjisstr = Encoding.GetEncoding("Shift_JIS").GetString(data);
                bytesData = System.Text.Encoding.UTF8.GetBytes(sjisstr);
            }

            // 出力ファイル
            Trace.WriteLine($"{LogHelper.Stamp}outputFile: {outputFile.FullName}");

            using (FileStream fs2 = new FileStream(outputFile.FullName, FileMode.Create))
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
