namespace Grayscale.CsaOpener
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.CsaOpener.Location;

    /// <summary>
    /// 何か所かで使うもの。
    /// </summary>
    public static class CommonsLib
    {
        /// <summary>
        /// 棋譜を読み取ります。
        /// </summary>
        /// <param name="input_file">読み取る棋譜ファイルのパス。</param>
        /// <param name="output_file">出力先ファイルパス。</param>
        /// <returns>リターン コード。</returns>
        public static int ReadGameRecord(string input_file, string output_file)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            // 起動する実行ファイルのパスを設定する
            info.FileName = KifuwarabeWcsc29Config.Instance.kifuwarabe_wcsc29_exe_path_for_read_kifu;
            info.WorkingDirectory = Directory.GetParent(KifuwarabeWcsc29Config.Instance.kifuwarabe_wcsc29_exe_path_for_read_kifu).FullName;

            // コマンドライン引数を指定する
            info.Arguments = $@"--input ""{input_file.Replace(@"\", "/")}"" --output ""{output_file.Replace(@"\", "/")}""";

            // コンソール・ウィドウを開かない。
            info.CreateNoWindow = true;

            // シェル機能を使用しない
            info.UseShellExecute = false;

            var p = Process.Start(info);

            // タイムアウト時間（秒）。１棋譜に 1分も かからないだろう。
            p.WaitForExit(60 * 1000);

            var returnCode = p.ExitCode;
            if (returnCode != 0)
            {
                Trace.WriteLine($"Eat: returnCode='{returnCode}' {info.FileName} {info.Arguments}");
            }

            return returnCode;
        }
    }
}
