namespace Grayscale.CsaOpener.Location
{
    using System.IO;
    using Grayscale.CsaOpener.Commons;

    /// <summary>
    /// ディレクトリ。
    /// </summary>
    public class EatingGoDirectory
    {
        private static TraceableDirectory thisInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EatingGoDirectory"/> class.
        /// </summary>
        protected EatingGoDirectory()
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
                    thisInstance = new TraceableDirectory(KifuwarabeWcsc29Config.Instance.eating.go);
                    thisInstance.Create();
                }

                return thisInstance;
            }
        }
    }
}
