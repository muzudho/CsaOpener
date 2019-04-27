namespace Grayscale.CsaOpener.Location
{
    using System.IO;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class ExpansionWentDirectory
    {
        private static TraceableDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpansionWentDirectory"/> class.
        /// </summary>
        protected ExpansionWentDirectory()
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
                    thisInstance = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.expansion.went);
                    thisInstance.Create();
                }

                return thisInstance;
            }
        }
    }
}
