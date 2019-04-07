namespace Grayscale.CsaOpener
{
    using System.IO;
    using Codeplex.Data;

    /// <summary>
    /// 設定。
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets a これからやるパス。
        /// </summary>
        public string ExpansionGoPath { get; private set; }

        /// <summary>
        /// Gets a やり終わったパス。
        /// </summary>
        public string ExpansionWentPath { get; private set; }

        /// <summary>
        /// Gets a 出力先のパス。
        /// </summary>
        public string ExpansionOutputPath { get; private set; }

        /// <summary>
        /// Gets a これからやるパス。
        /// </summary>
        public string FormationGoPath { get; private set; }

        /// <summary>
        /// Gets a やり終わったパス。
        /// </summary>
        public string FormationWentPath { get; private set; }

        /// <summary>
        /// Gets a 出力先のパス。
        /// </summary>
        public string FormationOutputPath { get; private set; }

        /// <summary>
        /// Gets a これからやるパス。
        /// </summary>
        public string EatingGoPath { get; private set; }

        /// <summary>
        /// Gets a やり終わったパス。
        /// </summary>
        public string EatingWentPath { get; private set; }

        /// <summary>
        /// Gets a 出力先のパス。
        /// </summary>
        public string EatingOutputPath { get; private set; }

        /// <summary>
        /// ファイル読み取り。
        /// </summary>
        /// <returns>設定。</returns>
        public static Config Load()
        {
            var json = File.ReadAllText("./config.json");
            dynamic config1 = DynamicJson.Parse(json);

            var config2 = new Config();
            config2.ExpansionGoPath = config1.expansion.go; // arguments.Input
            config2.ExpansionWentPath = config1.expansion.went;
            config2.ExpansionOutputPath = config1.expansion.output; // arguments.Output

            config2.FormationGoPath = config1.formation.go;
            config2.FormationWentPath = config1.formation.went;
            config2.FormationOutputPath = config1.formation.output;

            config2.EatingGoPath = config1.eating.go;
            config2.EatingWentPath = config1.eating.went;
            config2.EatingOutputPath = config1.eating.output;

            return config2;
        }
    }
}
