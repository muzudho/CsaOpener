namespace Grayscale.ShogiKifuConverter
{
    using System;

    /// <summary>
    /// 設定ファイル読込前に利用可能な設定。
    /// </summary>
    public static class ZerothSettings
    {
        /// <summary>
        /// Gets a .exeファイルがあるディレクトリー。
        /// </summary>
        public static string ExeDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'); }
        }
    }
}
