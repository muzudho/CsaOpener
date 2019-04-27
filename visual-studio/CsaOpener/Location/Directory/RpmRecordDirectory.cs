namespace Grayscale.CsaOpener.Location
{
    using System.IO;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class RpmRecordDirectory
    {
        private static TraceableDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="RpmRecordDirectory"/> class.
        /// </summary>
        protected RpmRecordDirectory()
        {
        }

        /// <summary>
        /// Gets a このインスタンス。
        /// </summary>
        public static TraceableDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.rpm_record);
                    thisInstance.Create();
                }

                return thisInstance;
            }
        }
    }
}
