namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class EatingWentDirectory
    {
        private static EatingWentDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EatingWentDirectory"/> class.
        /// </summary>
        protected EatingWentDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static EatingWentDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new EatingWentDirectory();
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
            get
            {
                return KifuwarabeWcsc29Config.Instance.eating.went;
            }
        }
    }
}
