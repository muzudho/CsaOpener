namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class EatingOutputDirectory
    {
        private static EatingOutputDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EatingOutputDirectory"/> class.
        /// </summary>
        protected EatingOutputDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static EatingOutputDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new EatingOutputDirectory();
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
            get { return KifuwarabeWcsc29Config.Instance.eating.output; }
        }
    }
}
