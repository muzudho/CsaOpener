namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class FomationOutputDirectory
    {
        private static FomationOutputDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="FomationOutputDirectory"/> class.
        /// </summary>
        protected FomationOutputDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static FomationOutputDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new FomationOutputDirectory();
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
            get { return KifuwarabeWcsc29Config.Instance.formation.output; }
        }
    }
}
