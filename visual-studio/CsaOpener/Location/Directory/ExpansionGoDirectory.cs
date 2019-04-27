namespace Grayscale.CsaOpener.Location
{
    using System.IO;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class ExpansionGoDirectory
    {
        private static TraceableDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpansionGoDirectory"/> class.
        /// </summary>
        protected ExpansionGoDirectory()
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
                    thisInstance = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.expansion.go);
                    thisInstance.Create();
                }

                return thisInstance;
            }
        }
    }
}
