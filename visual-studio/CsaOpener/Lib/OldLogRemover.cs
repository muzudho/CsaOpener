namespace Grayscale.CsaOpener
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    // [ソリューション エクスプローラー]のプロジェクトの下の[参照]を右クリック、
    // [参照の追加(R)...]をクリック。[アセンブリ] - [フレームワーク] と進み、
    // Microsoft.VisualBasic をチェックしてください。
    using VBFileIO = Microsoft.VisualBasic.FileIO;

    /// <summary>
    /// 古いログを削除します。
    /// </summary>
    public static class OldLogRemover
    {
        /// <summary>
        /// 古いログを削除します。
        /// </summary>
        /// <param name="logDirectory">ログ・ディレクトリー。</param>
        public static void RemoveOldLog(string logDirectory)
        {
            if (Directory.Exists(logDirectory))
            {
                var regex = new Regex(@"rotate-(\d{4})-(\d{2})-(\d{2}).*\.log");

                // 10日前までのファイルは残す。
                var tenDaysAgo = DateTime.Today.AddDays(-10);

                string[] files = Directory.GetFiles(
                    logDirectory,
                    "rotate-*.log",
                    SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    Trace.WriteLine("File: " + file);
                    var match = regex.Match(file);
                    if (match.Success)
                    {
                        var fileNameDate = new DateTime(
                            int.Parse(match.Groups[1].Value),
                            int.Parse(match.Groups[2].Value),
                            int.Parse(match.Groups[3].Value));
                        if (fileNameDate < tenDaysAgo)
                        {
                            // 完全に削除したいなら。
                            // File.Delete(file);
                            // ゴミ箱に入れたいなら。
                            VBFileIO.FileSystem.DeleteFile(
                                file,
                                VBFileIO.UIOption.OnlyErrorDialogs,
                                VBFileIO.RecycleOption.SendToRecycleBin);

                            Trace.WriteLine("Removed: " + file);
                        }
                    }
                }
            }
        }
    }
}
