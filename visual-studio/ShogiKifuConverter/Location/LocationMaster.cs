namespace Grayscale.ShogiKifuConverter.Location
{
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
            ExeConf = new TraceableFile(PathHelper.Combine(ZerothSettings.ExeDirectory, "./exe-config.json"));
            {
                var json = DynamicJson.Parse(LocationMaster.ExeConf.ReadAllText());
                ExeConfJson = json.Deserialize<ExeConfigJson>();
            }

            // ゲームエンジンの設定ファイル。
            Kw29MasterConf = new TraceableFile(LocationMaster.ExeConfJson.kifuwarabe_wcsc29_converter_master_config_path);
            {
                var json = DynamicJson.Parse(LocationMaster.Kw29MasterConf.ReadAllText());
                Kw29MasterConfJson = json.Deserialize<KifuwarabeWcsc29ConverterMasterConfigJson>();
            }

            // 入力ディレクトリー。
            InputDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.input);
            InputDirectory.Create();

            // 解凍済みディレクトリー。
            ExpandedDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.expanded);
            ExpandedDirectory.Create();

            // エンコーディング済みディレクトリー。
            EncodedDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.encoded);
            EncodedDirectory.Create();

            // 変換済みの棋譜ディレクトリー。
            ConvertedDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.converted);
            ConvertedDirectory.Create();

            // 棋譜を１つのファイルに詰め込んだファイルのディレクトリー。
            JammedDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.jammed);
            JammedDirectory.Create();

            // 変換エラー出力ディレクトリー。
            ErrorDirectory = new TraceableDirectory(LocationMaster.Kw29MasterConfJson.error);
            ErrorDirectory.Create();
        }

        /// <summary>
        /// Gets a 解凍待ちディレクトリー。
        /// </summary>
        public static TraceableDirectory InputDirectory { get; private set; }

        /// <summary>
        /// Gets a 解凍済みの棋譜ディレクトリー。
        /// </summary>
        public static TraceableDirectory ExpandedDirectory { get; private set; }

        /// <summary>
        /// Gets a エンコード済みの棋譜ディレクトリー。
        /// </summary>
        public static TraceableDirectory EncodedDirectory { get; private set; }

        /// <summary>
        /// Gets a 棋譜変換済みの棋譜ディレクトリー。
        /// </summary>
        public static TraceableDirectory ConvertedDirectory { get; private set; }

        /// <summary>
        /// Gets a 棋譜を１つのファイルに詰め込んだファイルのディレクトリー。
        /// </summary>
        public static TraceableDirectory JammedDirectory { get; private set; }

        /// <summary>
        /// Gets a 変換エラー出力ディレクトリー。
        /// </summary>
        public static TraceableDirectory ErrorDirectory { get; private set; }

        /// <summary>
        /// Gets a 設定ファイル。
        /// </summary>
        public static TraceableFile Kw29MasterConf { get; private set; }

        /// <summary>
        /// Gets a このアプリケーション.exeと同じディレクトリに置いてある設定ファイル。
        /// </summary>
        public static TraceableFile ExeConf { get; private set; }

        /// <summary>
        /// Gets a このアプリケーション.exeと同じディレクトリに置いてある設定ファイルの内容。
        /// </summary>
        public static ExeConfigJson ExeConfJson { get; private set; }

        /// <summary>
        /// Gets a ゲームエンジンの設定ファイルの内容。
        /// </summary>
        public static KifuwarabeWcsc29ConverterMasterConfigJson Kw29MasterConfJson { get; private set; }
    }
}
