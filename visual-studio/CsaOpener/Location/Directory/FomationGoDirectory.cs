namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class FomationGoDirectory
    {
        private static FomationGoDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="FomationGoDirectory"/> class.
        /// </summary>
        protected FomationGoDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static FomationGoDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new FomationGoDirectory();
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
            get { return KifuwarabeWcsc29Config.Instance.formation.go; }
        }
    }
}
