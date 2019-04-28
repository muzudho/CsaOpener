namespace Grayscale.CsaOpener.Location
{
    using System;
    using Codeplex.Data;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// 固定のディレクトリなど一元管理。
    /// </summary>
    public static class LocationMaster
    {
        static LocationMaster()
        {
            // このアプリケーション.exeと同じディレクトリに置いてある設定ファイル。
            MyAppConf = new TraceableFile(PathHelper.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), "./config.json"));
            {
                var json = DynamicJson.Parse(LocationMaster.MyAppConf.ReadAllText());
                MyAppConfJson = json.Deserialize<OpenerConfigJson>();
            }

            // ゲームエンジンの設定ファイル。
            Kw29Conf = new TraceableFile(LocationMaster.MyAppConfJson.kifuwarabe_wcsc29_config_path);
            {
                var json = DynamicJson.Parse(LocationMaster.Kw29Conf.ReadAllText());
                Kw29ConfJson = json.Deserialize<KifuwarabeWcsc29ConfigJson>();
            }

            // 解凍フェーズ。
            ExpansionGoDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.expansion.go);
            ExpansionGoDirectory.Create();

            ExpansionWentDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.expansion.went);
            ExpansionWentDirectory.Create();

            ExpansionOutputDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.expansion.output);
            ExpansionOutputDirectory.Create();

            // エンコーディング・フェーズ。
            FomationGoDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.formation.go);
            FomationGoDirectory.Create();

            FomationWentDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.formation.went);
            FomationWentDirectory.Create();

            FomationOutputDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.formation.output);
            FomationOutputDirectory.Create();

            // 棋譜読取フェーズ。
            EatingGoDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.eating.go);
            EatingGoDirectory.Create();

            EatingWentDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.eating.went);
            EatingWentDirectory.Create();

            EatingOutputDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.eating.output);
            EatingOutputDirectory.Create();

            // 成果物ディレクトリー。
            TrainingDirectory = new TraceableDirectory(LocationMaster.Kw29ConfJson.training);
            TrainingDirectory.Create();
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

        /// <summary>
        /// Gets a 成果物ディレクトリー。
        /// </summary>
        public static TraceableDirectory TrainingDirectory { get; private set; }

        /// <summary>
        /// Gets a 設定ファイル。
        /// </summary>
        public static TraceableFile Kw29Conf { get; private set; }

        /// <summary>
        /// Gets a このアプリケーション.exeと同じディレクトリに置いてある設定ファイル。
        /// </summary>
        public static TraceableFile MyAppConf { get; private set; }

        /// <summary>
        /// Gets a このアプリケーション.exeと同じディレクトリに置いてある設定ファイルの内容。
        /// </summary>
        public static OpenerConfigJson MyAppConfJson { get; private set; }

        /// <summary>
        /// Gets a ゲームエンジンの設定ファイルの内容。
        /// </summary>
        public static KifuwarabeWcsc29ConfigJson Kw29ConfJson { get; private set; }
    }
}
