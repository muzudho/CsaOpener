using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// ここか？ http://www.csdinc.co.jp/archiver/lib/7-zip32.html

/// <summary>
/// 7z操作クラス(7-zip32.dllのラッパークラス)
/// </summary>
/// <remarks>
/// 【注意】
/// 7-zip32.dllをC:\Windows\などパスの通った場所に保存していること!!
///  DLL取得元：統合アーカイバプロジェクト
/// http://www.madobe.net/archiver/index.html
/// </remarks>
public static class SevenZManager
{
    /// <summary>
    /// [DLL Import] SevenZipのコマンドラインメソッド
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル(=0)</param>
    /// <param name="szCmdLine">コマンドライン</param>
    /// <param name="szOutput">実行結果文字列</param>
    /// <param name="dwSize">実行結果文字列格納サイズ</param>
    /// <returns>
    /// 0:正常、0以外:異常終了
    /// </returns>
    [DllImport("7-zip32.dll", CharSet = CharSet.Ansi)]
    private static extern int SevenZip(IntPtr hwnd, string szCmdLine, StringBuilder szOutput, int dwSize);

    /// <summary>
    /// 7z圧縮[複数ファイル]
    /// </summary>
    /// <param name="aryFilePath">圧縮対象ファイル一覧(フルパス指定)</param>
    /// <param name="s7zFilePath">7zファイル名</param>
    public static void fnCompressFiles(List<string> aryFilePath, string s7zFilePath)
    {
        lock (typeof(SevenZManager))
        {
            StringBuilder sbCmdLine = new StringBuilder(1024);   // コマンドライン文字列
            StringBuilder sbOutput = new StringBuilder(1024);    // 7-zip32.dll出力文字
            //---------------------------------------------------------------------------------
            // コマンドライン文字列の作成
            //---------------------------------------------------------------------------------
            // a:書庫にファイルを追加
            // -t7z:7形式を指定
            // -hide:処理状況ダイアログ表示の抑止
            // -mmt=on:マルチスレッドモードの設定
            // -y:全ての質問に yes を仮定
            sbCmdLine.AppendFormat("a -t7z -hide -mmt=on -y \"{0}\"", s7zFilePath);
            // 圧縮対象ファイルをコマンドライン化
            foreach (string sFilePath in aryFilePath)
            {
                sbCmdLine.AppendFormat(" \"{0}\"", sFilePath);
            }
            string sCmdLine = sbCmdLine.ToString();
            //---------------------------------------------------------------------------------
            // 圧縮実行
            //---------------------------------------------------------------------------------
            int iSevenZipRtn = SevenZip((IntPtr)0, sCmdLine, sbOutput, sbOutput.Capacity);
            //---------------------------------------------------------------------------------
            // 成功判定
            //---------------------------------------------------------------------------------
            fnCheckProc(iSevenZipRtn, sbOutput);
        }
    }
    /// <summary>
    /// ZIP圧縮[複数ファイル・パスワード指定]
    /// </summary>
    /// <param name="aryFilePath">圧縮対象ファイル一覧(フルパス指定)</param>
    /// <param name="s7zFilePath">7zファイル名</param>
    /// <param name="sPassword">パスワード(半角英数字)</param>
    public static void fnCompressFilesWithPassword(List<string> aryFilePath, string s7zFilePath, string sPassword)
    {
        lock (typeof(SevenZManager))
        {
            StringBuilder sbCmdLine = new StringBuilder(1024);   // コマンドライン文字列
            StringBuilder sbOutput = new StringBuilder(1024);    // 7-zip32.dll出力文字
            //---------------------------------------------------------------------------------
            // コマンドライン文字列の作成
            //---------------------------------------------------------------------------------
            // a:書庫にファイルを追加
            // -t7z:7形式を指定
            // -hide:処理状況ダイアログ表示の抑止
            // -mmt=on:マルチスレッドモードの設定
            // -y:全ての質問に yes を仮定
            // -p{password}:パスワードの設定
            sbCmdLine.AppendFormat(
                "a -t7z -hide -mmt=on -y -p{1} \"{0}\"", s7zFilePath, sPassword);
            // 圧縮対象ファイルをコマンドライン化
            foreach (string sFilePath in aryFilePath)
            {
                sbCmdLine.AppendFormat(" \"{0}\"", sFilePath);
            }
            string sCmdLine = sbCmdLine.ToString();
            //---------------------------------------------------------------------------------
            // 圧縮実行
            //---------------------------------------------------------------------------------
            int iSevenZipRtn = SevenZip((IntPtr)0, sCmdLine, sbOutput, sbOutput.Capacity);
            //---------------------------------------------------------------------------------
            // 成功判定
            //---------------------------------------------------------------------------------
            fnCheckProc(iSevenZipRtn, sbOutput);
        }
    }
    /// <summary>
    /// 7z圧縮[フォルダ指定]
    /// </summary>
    /// <param name="sFolderPath">圧縮対象フォルダ(フルパス指定)</param>
    /// <param name="s7zFilePath">7zファイル名</param>
    public static void fnCompressFolder(string sFolderPath, string s7zFilePath)
    {
        lock (typeof(SevenZManager))
        {
            StringBuilder sbOutput = new StringBuilder(1024);   // 7-zip32.dll出力文字
            //---------------------------------------------------------------------------------
            // sFolderPathの最後が\の場合、\を取り払う
            //---------------------------------------------------------------------------------
            while (sFolderPath[sFolderPath.Length - 1] == '\\')
            {
                sFolderPath = sFolderPath.Substring(0, sFolderPath.Length - 1);
            }
            //---------------------------------------------------------------------------------
            // コマンドライン文字列の作成
            //---------------------------------------------------------------------------------
            // a:書庫にファイルを追加
            // -t7z:7形式を指定
            // -hide:処理状況ダイアログ表示の抑止
            // -mmt=on:マルチスレッドモードの設定
            // -y:全ての質問に yes を仮定
            // -r:サブディレクトリの再帰的検索
            string sCmdLine = string.Format(
                "a -t7z -hide -mmt=on -y -r \"{0}\" \"{1}\\*\"", s7zFilePath, sFolderPath);
            //---------------------------------------------------------------------------------
            // 圧縮実行
            //---------------------------------------------------------------------------------
            int iSevenZipRtn = SevenZip((IntPtr)0, sCmdLine, sbOutput, sbOutput.Capacity);
            //---------------------------------------------------------------------------------
            // 成功判定
            //---------------------------------------------------------------------------------
            fnCheckProc(iSevenZipRtn, sbOutput);
        }
    }
    /// <summary>
    /// 7z圧縮[フォルダ指定・パスワード指定]
    /// </summary>
    /// <param name="sFolderPath">圧縮対象フォルダ(フルパス指定)</param>
    /// <param name="s7zFilePath">7zファイル名</param>
    /// <param name="sPassword">パスワード(半角英数字)</param>
    public static void fnCompressFolderWithPassword(string sFolderPath, string s7zFilePath, string sPassword)
    {
        lock (typeof(SevenZManager))
        {
            StringBuilder sbOutput = new StringBuilder(1024);   // 7-zip32.dll出力文字
            //---------------------------------------------------------------------------------
            // sFolderPathの最後が\の場合、\を取り払う
            //---------------------------------------------------------------------------------
            while (sFolderPath[sFolderPath.Length - 1] == '\\')
            {
                sFolderPath = sFolderPath.Substring(0, sFolderPath.Length - 1);
            }
            //---------------------------------------------------------------------------------
            // コマンドライン文字列の作成
            //---------------------------------------------------------------------------------
            // a:書庫にファイルを追加
            // -t7z:7形式を指定
            // -hide:処理状況ダイアログ表示の抑止
            // -mmt=on:マルチスレッドモードの設定
            // -y:全ての質問に yes を仮定
            // -r:サブディレクトリの再帰的検索
            // -p{password}:パスワードの設定
            string sCmdLine = string.Format(
                "a -t7z -hide -mmt=on -y -r -p{2} \"{0}\" \"{1}\\*\"",
                s7zFilePath, sFolderPath, sPassword);
            //---------------------------------------------------------------------------------
            // 圧縮実行
            //---------------------------------------------------------------------------------
            int iSevenZipRtn = SevenZip((IntPtr)0, sCmdLine, sbOutput, sbOutput.Capacity);
            //---------------------------------------------------------------------------------
            // 成功判定
            //---------------------------------------------------------------------------------
            fnCheckProc(iSevenZipRtn, sbOutput);
        }
    }
    /// <summary>
    /// 7z解凍
    /// </summary>
    /// <param name="s7zFilePath">7zファイル名</param>
    /// <param name="sDustFolder">出力先フォルダ</param>
    public static void fnExtract(string s7zFilePath, string sDustFolder)
    {
        lock (typeof(SevenZManager))
        {
            StringBuilder sbOutput = new StringBuilder(1024);   // 7-zip32.dll出力文字
            //---------------------------------------------------------------------------------
            // sDustFolderの最後が\の場合、\を取り払う
            //---------------------------------------------------------------------------------
            while (sDustFolder[sDustFolder.Length - 1] == '\\')
            {
                sDustFolder = sDustFolder.Substring(0, sDustFolder.Length - 1);
            }
            ////---------------------------------------------------------------------------------
            //// 出力先フォルダが存在しなければ作成
            ////---------------------------------------------------------------------------------
            //if (!Directory.Exists(sDustFolder))
            //{
            //    Directory.CreateDirectory(sDustFolder);
            //}
            //---------------------------------------------------------------------------------
            // コマンドライン文字列の作成
            //---------------------------------------------------------------------------------
            // x:解凍
            // -aoa:確認なしで上書き
            // -hide:処理状況ダイアログ表示の抑止
            // -y:全ての質問に yes を仮定
            // -r:サブディレクトリの再帰的検索
            // -o{dir_path}:出力先ディレクトリの設定
            string sCmdLine = string.Format(
                           "x -aoa -hide -y -r \"{0}\" -o\"{1}\"\\*", s7zFilePath, sDustFolder);
            //---------------------------------------------------------------------------------
            // 解凍実行
            //---------------------------------------------------------------------------------
            int iSevenZipRtn = SevenZip((IntPtr)0, sCmdLine, sbOutput, sbOutput.Capacity);
            //---------------------------------------------------------------------------------
            // 成功判定
            //---------------------------------------------------------------------------------
            fnCheckProc(iSevenZipRtn, sbOutput);
        }
    }
    /// <summary>
    /// 7z解凍[パスワード指定]
    /// </summary>
    /// <param name="s7zFilePath">7zファイル名</param>
    /// <param name="sDustFolder">出力先フォルダ</param>
    /// <param name="sPassword">パスワード(半角英数字)</param>
    public static void fnExtractWithPassword(string s7zFilePath, string sDustFolder, string sPassword)
    {
        lock (typeof(SevenZManager))
        {
            StringBuilder sbOutput = new StringBuilder(1024);   // 7-zip32.dll出力文字
            //---------------------------------------------------------------------------------
            // sDustFolderの最後が\の場合、\を取り払う
            //---------------------------------------------------------------------------------
            while (sDustFolder[sDustFolder.Length - 1] == '\\')
            {
                sDustFolder = sDustFolder.Substring(0, sDustFolder.Length - 1);
            }
            //---------------------------------------------------------------------------------
            // コマンドライン文字列の作成
            //---------------------------------------------------------------------------------
            // x:解凍
            // -aoa:確認なしで上書き
            // -hide:処理状況ダイアログ表示の抑止
            // -y:全ての質問に yes を仮定
            // -r:サブディレクトリの再帰的検索
            // -o{dir_path}:出力先ディレクトリの設定
            // -p{password}:パスワードの設定
            string sCmdLine = string.Format(
                "x -aoa -hide -y -r \"{0}\" -o\"{1}\"\\* -p{2}",
                s7zFilePath, sDustFolder, sPassword);
            //---------------------------------------------------------------------------------
            // 解凍実行
            //---------------------------------------------------------------------------------
            int iSevenZipRtn = SevenZip((IntPtr)0, sCmdLine, sbOutput, sbOutput.Capacity);
            //---------------------------------------------------------------------------------
            // 成功判定
            //---------------------------------------------------------------------------------
            fnCheckProc(iSevenZipRtn, sbOutput);
        }
    }
    /// <summary>
    /// SevenZipメソッド成功判定
    /// </summary>
    /// <param name="iSevenZipRtn">SevenZipメソッドの戻り値</param>
    /// <param name="sbLzhOutputString">SevenZipメソッドの第3引数</param>
    private static void fnCheckProc(int iSevenZipRtn, StringBuilder sbLzhOutputString)
    {
        //-------------------------------------------------------------------------------------
        // メソッドの戻り値=0なら正常終了
        //-------------------------------------------------------------------------------------
        if (iSevenZipRtn == 0)
            return;
        //-------------------------------------------------------------------------------------
        // 例外スロー
        //-------------------------------------------------------------------------------------
        string sMsg = string.Format(
            "7z圧縮/解凍処理に失敗:\nエラーコード={0}:\n{1}", iSevenZipRtn, sbLzhOutputString);
        throw new ApplicationException(sMsg);
    }
}
