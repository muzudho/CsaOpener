namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class ExpansionGoDirectory
    {
        private static ExpansionGoDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpansionGoDirectory"/> class.
        /// </summary>
        protected ExpansionGoDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static ExpansionGoDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new ExpansionGoDirectory();
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
            get { return KifuwarabeWcsc29Config.Instance.expansion.go; }
        }
    }
}
