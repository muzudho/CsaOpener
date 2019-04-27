namespace Grayscale.CsaOpener.Location
{
    using System.IO;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class EatingWentDirectory
    {
        private static TraceableDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EatingWentDirectory"/> class.
        /// </summary>
        protected EatingWentDirectory()
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
                    thisInstance = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.eating.went);
                    thisInstance.Create();
                }

                return thisInstance;
            }
        }
    }
}
