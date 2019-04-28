namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.CsaOpener.Commons;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// 何か所かで使うもの。
    /// </summary>
    public static class CommonsLib
    {
        /// <summary>
        /// 棋譜を読み取ります。
        /// </summary>
        /// <param name="inputFile">読み取る棋譜ファイル。</param>
        /// <param name="outputFile">出力先ファイル。</param>
        /// <returns>リターン コード。</returns>
        public static int ReadGameRecord(TraceableFile inputFile, TraceableFile outputFile)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            // 起動する実行ファイルのパスを設定する
            info.FileName = LocationMaster.Kw29ConfJson.kifuwarabe_wcsc29_exe_path_for_read_kifu;
            info.WorkingDirectory = Directory.GetParent(LocationMaster.Kw29ConfJson.kifuwarabe_wcsc29_exe_path_for_read_kifu).FullName;

            // コマンドライン引数を指定する
            info.Arguments = $@"--input ""{inputFile.FullName.Replace(@"\", "/")}"" --output ""{outputFile.FullName.Replace(@"\", "/")}""";

            // コンソール・ウィドウを開かない。
            info.CreateNoWindow = true;

            // シェル機能を使用しない
            info.UseShellExecute = false;

            Trace.WriteLine($"Go      : Process {info.FileName} {info.Arguments}");
            var p = Process.Start(info);

            // タイムアウト時間（秒）。１棋譜に 1分も かからないだろう。
            p.WaitForExit(60 * 1000);

            var returnCode = p.ExitCode;
            if (returnCode != 0)
            {
                Trace.WriteLine($"Error   : Process returnCode='{returnCode}' {info.FileName} {info.Arguments}");
            }

            return returnCode;
        }
    }
}
