namespace Grayscale.CsaOpener
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Codeplex.Data;
    using ICSharpCode.SharpZipLib.GZip;
    using ICSharpCode.SharpZipLib.Tar;

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

            return config2;
        }
    }
}
