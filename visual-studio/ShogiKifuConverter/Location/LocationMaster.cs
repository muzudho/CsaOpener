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
                MyAppConfJson = json.Deserialize<ExeConfigJson>();
            }

            // ゲームエンジンの設定ファイル。
            Kw29MasterConf = new TraceableFile(LocationMaster.MyAppConfJson.kifuwarabe_wcsc29_master_config_path);
            {
                var json = DynamicJson.Parse(LocationMaster.Kw29MasterConf.ReadAllText());
                Kw29MasterConfJson = json.Deserialize<KifuwarabeWcsc29MasterConfigJson>();
            }

            // 入力ディレクトリー。
            ConverterInputDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converter_input);
            ConverterInputDirectory.Create();

            // 解凍済みディレクトリー。
            ConverterExpandDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converter_expand);
            ConverterExpandDirectory.Create();

            // エンコーディング済みディレクトリー。
            ConverterWorkingDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converter_working);
            ConverterWorkingDirectory.Create();

            // 変換済みディレクトリー。
            ConverterOutputDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converter_output);
            ConverterOutputDirectory.Create();

            // 変換エラー出力ディレクトリー。
            ConverterErrorDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converter_error);
            ConverterErrorDirectory.Create();

            // 成果物ディレクトリー。
            TrainingDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.training);
            TrainingDirectory.Create();
        }

        /// <summary>
        /// Gets a 解凍待ちディレクトリー。
        /// </summary>
        public static TraceableDirectory ConverterInputDirectory { get; private set; }

        /// <summary>
        /// Gets a 解凍の成果ディレクトリー。
        /// </summary>
        public static TraceableDirectory ConverterExpandDirectory { get; private set; }

        /// <summary>
        /// Gets a 変換中ディレクトリー。
        /// </summary>
        public static TraceableDirectory ConverterWorkingDirectory { get; private set; }

        /// <summary>
        /// Gets a 変換出力ディレクトリー。
        /// </summary>
        public static TraceableDirectory ConverterOutputDirectory { get; private set; }

        /// <summary>
        /// Gets a 変換エラー出力ディレクトリー。
        /// </summary>
        public static TraceableDirectory ConverterErrorDirectory { get; private set; }

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
        public static ExeConfigJson MyAppConfJson { get; private set; }

        /// <summary>
        /// Gets a ゲームエンジンの設定ファイルの内容。
        /// </summary>
        public static KifuwarabeWcsc29MasterConfigJson Kw29MasterConfJson { get; private set; }
    }
}
