using Grayscale.CsaOpener.Commons;

namespace Grayscale.CsaOpener.Location
{
    /// <summary>
    /// 固定のディレクトリなど。
    /// </summary>
    public static class FileSystem
    {
        static FileSystem()
        {
            // 解凍フェーズ。
            ExpansionGoDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.expansion.go);
            ExpansionGoDirectory.Create();

            ExpansionWentDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.expansion.went);
            ExpansionWentDirectory.Create();

            ExpansionOutputDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.expansion.output);
            ExpansionOutputDirectory.Create();

            // エンコーディング・フェーズ。
            FomationGoDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.formation.go);
            FomationGoDirectory.Create();

            FomationWentDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.formation.went);
            FomationWentDirectory.Create();

            FomationOutputDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.formation.output);
            FomationOutputDirectory.Create();

            // 棋譜読取フェーズ。
            EatingGoDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.eating.go);
            EatingGoDirectory.Create();

            EatingWentDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.eating.went);
            EatingWentDirectory.Create();

            EatingOutputDirectory = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.eating.output);
            EatingOutputDirectory.Create();
        }

        /// <summary>
        /// Gets a 棋譜読取待ちディレクトリー。
        /// </summary>
        public static TraceableDirectory EatingGoDirectory { get; private set; }

        /// <summary>
        /// Gets a 棋譜読取済みディレクトリー。
        /// </summary>
        public static TraceableDirectory EatingWentDirectory { get; private set; }

        /// <summary>
        /// Gets a 棋譜読取成果ディレクトリー。
        /// </summary>
        public static TraceableDirectory EatingOutputDirectory { get; private set; }

        /// <summary>
        /// Gets a 解凍待ちディレクトリー。
        /// </summary>
        public static TraceableDirectory ExpansionGoDirectory { get; private set; }

        /// <summary>
        /// Gets a 解凍済みディレクトリー。
        /// </summary>
        public static TraceableDirectory ExpansionWentDirectory { get; private set; }

        /// <summary>
        /// Gets a 解凍の成果ディレクトリー。
        /// </summary>
        public static TraceableDirectory ExpansionOutputDirectory { get; private set; }

        /// <summary>
        /// Gets a エンコーディング待ちディレクトリー。
        /// </summary>
        public static TraceableDirectory FomationGoDirectory { get; private set; }

        /// <summary>
        /// Gets a エンコーディング済みディレクトリー。
        /// </summary>
        public static TraceableDirectory FomationWentDirectory { get; private set; }

        /// <summary>
        /// Gets a エンコーディング成果ディレクトリー。
        /// </summary>
        public static TraceableDirectory FomationOutputDirectory { get; private set; }
    }
}
