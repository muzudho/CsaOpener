namespace Grayscale.ShogiKifuConverter.Commons
{
    using System;

    /// <summary>
    /// ログ・ヘルパー。
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Gets a 日付と時刻。
        /// </summary>
        public static string Stamp
        {
            get
            {
                DateTime dt = DateTime.Now;
                return $@"[{dt.Year:D4}-{dt.Month:D2}-{dt.Day:D2} {dt.Hour:D2}:{dt.Minute:D2}:{dt.Second:D2}] ";
            }
        }
    }
}
