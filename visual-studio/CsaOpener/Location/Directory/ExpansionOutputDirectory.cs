namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class ExpansionOutputDirectory
    {
        private static ExpansionOutputDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpansionOutputDirectory"/> class.
        /// </summary>
        protected ExpansionOutputDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static ExpansionOutputDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new ExpansionOutputDirectory();
                    if (!Directory.Exists(thisInstance.Path))
                    {
                        Directory.CreateDirectory(thisInstance.Path);
                    }
                }

                return thisInstance;
            }
        }

        /// <summary>
        /// Gets a パス。
        /// </summary>
        public string Path
        {
            get { return KifuwarabeWcsc29Config.Instance.expansion.output; }
        }
    }
}
