namespace Grayscale.CsaOpener.Location
{
    using System.IO;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class RpmRecordDirectory
    {
        private static RpmRecordDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="RpmRecordDirectory"/> class.
        /// </summary>
        protected RpmRecordDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static RpmRecordDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new RpmRecordDirectory();
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
            get { return KifuwarabeWcsc29Config.Instance.rpm_record; }
        }
    }
}
