namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class EatingGoDirectory
    {
        private static EatingGoDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EatingGoDirectory"/> class.
        /// </summary>
        protected EatingGoDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static EatingGoDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new EatingGoDirectory();
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
            get { return KifuwarabeWcsc29Config.Instance.eating.go; }
        }
    }
}
