namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class FomationWentDirectory
    {
        private static FomationWentDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="FomationWentDirectory"/> class.
        /// </summary>
        protected FomationWentDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static FomationWentDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new FomationWentDirectory();
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
            get { return KifuwarabeWcsc29Config.Instance.formation.went; }
        }
    }
}
