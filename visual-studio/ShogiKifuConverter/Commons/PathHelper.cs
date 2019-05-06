namespace Grayscale.ShogiKifuConverter.Commons
{
    using System.IO;

    /// <summary>
    /// パス関連。
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// 接続。
        /// </summary>
        /// <param name="left">左側。</param>
        /// <param name="right">右側。</param>
        /// <returns>接続後。</returns>
        public static string Combine(string left, string right)
        {
            return Path.Combine(left, right.TrimStart('/', '\\'));
        }

        /// <summary>
        /// 接続。
        /// </summary>
        /// <param name="left">左側。</param>
        /// <param name="middle">真ん中。</param>
        /// <param name="right">右側。</param>
        /// <returns>接続後。</returns>
        public static string Combine(string left, string middle, string right)
        {
            return Path.Combine(left, middle.TrimStart('/', '\\'), right.TrimStart('/', '\\'));
        }
    }
}
