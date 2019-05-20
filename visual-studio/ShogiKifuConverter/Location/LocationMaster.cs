namespace Grayscale.ShogiKifuConverter.Location
{
    using System;
    using Codeplex.Data;
    using Grayscale.ShogiKifuConverter.Commons;

    /// <summary>
    /// 固定のディレクトリなど一元管理。
    /// </summary>
    public static class LocationMaster
    {
        static LocationMaster()
        {
            // このアプリケーション.exeと同じディレクトリに置いてある設定ファイル。
            MyAppConf = new TraceableFile(PathHelper.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), "./app-config.json"));
            {
                var json = DynamicJson.Parse(LocationMaster.MyAppConf.ReadAllText());
                MyAppConfJson = json.Deserialize<AppConfigJson>();
            }

            // ゲームエンジンの設定ファイル。
            Kw29MasterConf = new TraceableFile(LocationMaster.MyAppConfJson.kifuwarabe_wcsc29_master_config_path);
            {
                var json = DynamicJson.Parse(LocationMaster.Kw29MasterConf.ReadAllText());
                Kw29MasterConfJson = json.Deserialize<KifuwarabeWcsc29MasterConfigJson>();
            }

            // 解凍フェーズ。
            ConverterInputDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converter_input);
            ConverterInputDirectory.Create();

            ConverterExpandDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converter_expand);
            ConverterExpandDirectory.Create();

            // エンコーディング・フェーズ。
            FomationGoDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.formation.go);
            FomationGoDirectory.Create();

            FomationWentDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.formation.went);
            FomationWentDirectory.Create();

            FomationOutputDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.formation.output);
            FomationOutputDirectory.Create();

            // 棋譜読取フェーズ。
            EatingGoDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.eating.go);
            EatingGoDirectory.Create();

            EatingWentDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.eating.went);
            EatingWentDirectory.Create();

            EatingOutputDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.eating.output);
            EatingOutputDirectory.Create();

            // 成果物ディレクトリー。
            TrainingDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.training);
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
        public static TraceableDirectory ConverterInputDirectory { get; private set; }

        /// <summary>
        /// Gets a 解凍の成果ディレクトリー。
        /// </summary>
        public static TraceableDirectory ConverterExpandDirectory { get; private set; }

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
        public static TraceableFile Kw29MasterConf { get; private set; }

        /// <summary>
        /// Gets a このアプリケーション.exeと同じディレクトリに置いてある設定ファイル。
        /// </summary>
        public static TraceableFile MyAppConf { get; private set; }

        /// <summary>
        /// Gets a このアプリケーション.exeと同じディレクトリに置いてある設定ファイルの内容。
        /// </summary>
        public static AppConfigJson MyAppConfJson { get; private set; }

        /// <summary>
        /// Gets a ゲームエンジンの設定ファイルの内容。
        /// </summary>
        public static KifuwarabeWcsc29MasterConfigJson Kw29MasterConfJson { get; private set; }
    }
}
