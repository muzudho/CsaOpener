namespace Grayscale.ShogiKifuConverter
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Codeplex.Data;
    using ICSharpCode.SharpZipLib.GZip;
    using ICSharpCode.SharpZipLib.Tar;

    // [ソリューション エクスプローラー]のプロジェクトの下の[参照]を右クリック、
    // [参照の追加(R)...]をクリック。[アセンブリ] - [フレームワーク] と進み、
    // Microsoft.VisualBasic をチェックしてください。
    using VBFileIO = Microsoft.VisualBasic.FileIO;
    using VBLogging = Microsoft.VisualBasic.Logging;

    /// <summary>
    /// ログを取る対象の処理。
    /// </summary>
    public delegate void TargetCallback();

    /// <summary>
    /// ローテーションするログの作成を行う。
    /// </summary>
    public static class LogRotation
    {
        /// <summary>
        /// ログを取る。
        /// </summary>
        /// <param name="targetCallback">ログを取る対象の処理。</param>
        public static void Logging(TargetCallback targetCallback)
        {
            /*
             * 参考。
             * https://dobon.net/vb/dotnet/programing/myapplicationlog.html
             * https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.visualbasic.logging.filelogtracelistener?redirectedfrom=MSDN&view=netframework-4.7.2
             * https://dobon.net/vb/dotnet/file/filecopy.html
             * https://dobon.net/vb/dotnet/programing/tracelisteners.html
             */
            using (var fileLogTraceListener = new VBLogging.FileLogTraceListener("LogFile"))
            {
                // ディレクトリが無くても、自動で作成する。
                var logDirectory = "./logs";

                // 最初から設定されているリスナーを削除する。
                Trace.Listeners.Remove("Default");

                // ストリームを閉じるのを待たずに、書き込みを進める。
                fileLogTraceListener.AutoFlush = true;
                fileLogTraceListener.Location = VBLogging.LogFileLocation.Custom;
                fileLogTraceListener.CustomLocation = logDirectory;
                fileLogTraceListener.BaseFileName = "rotate";
                fileLogTraceListener.LogFileCreationSchedule = VBLogging.LogFileCreationScheduleOption.Daily;
                Trace.Listeners.Add(fileLogTraceListener);

                // コンソールにも出力するなら。
                Trace.Listeners.Add(new ConsoleTraceListener());

                targetCallback();

                // 古いログを削除する。
                OldLogRemover.RemoveOldLog(logDirectory);
            }
        }
    }
}
