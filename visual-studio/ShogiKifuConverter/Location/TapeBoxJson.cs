namespace Grayscale.ShogiKifuConverter.Location
{
    using Grayscale.ShogiKifuConverter.Commons;

    /// <summary>
    /// テープ・ボックス・ファイル。
    /// </summary>
    public static class TapeBoxJson
    {
        /// <summary>
        /// ランダム名のテープ・ボックス・ファイルを自動生成。
        /// </summary>
        /// <param name="dir">保存先ディレクトリー。</param>
        /// <returns>テープ・ボックス・ファイル。</returns>
        public static TraceableFile CreateTapeBoxFileAtRandom(TraceableDirectory dir)
        {
            // ランダムな正の数を４つ つなげて長くする。
            var rand = new System.Random();
            var num1 = rand.Next();
            var num2 = rand.Next();
            var num3 = rand.Next();
            var num4 = rand.Next();

            return new TraceableFile(PathHelper.Combine(dir.FullName, $"{num1}-{num2}-{num3}-{num4}-tape-box.json"));
        }
    }
}
