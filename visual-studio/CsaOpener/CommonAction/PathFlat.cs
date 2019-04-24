namespace Grayscale.CsaOpener.CommonAction
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// ディレクトリ階層が深くて困る。
    /// ファイル名に親ディレクトリ名を付けてフォルダーから出す。
    /// 代わりのファイル名の区切りは、適当に '$%' を使うことにする。
    /// </summary>
    public class PathFlat
    {
        /// <summary>
        /// 実行。
        /// </summary>
        /// <param name="file">ファイル名。</param>
        public static void Execute(string file)
        {
            var joinedName = $"{Directory.GetParent(file).Name}$%{Path.GetFileName(file)}";
            var parentParentDirectory = Directory.GetParent(Directory.GetParent(file).FullName).FullName;
            var destination = Path.Combine(parentParentDirectory, joinedName);

            Trace.WriteLine($"Execute : {file} -> {destination}.");
            File.Move(file, destination);
        }

        /// <summary>
        /// ディレクトリーの中を全部実行。
        /// </summary>
        /// <param name="directory">ディレクトリー。</param>
        public static void Search(string directory)
        {
            Trace.WriteLine($"Search  : {directory}.");

            // 指定ディレクトリ以下のファイルをすべて取得する
            IEnumerable<string> childDirectoies =
                System.IO.Directory.EnumerateDirectories(
                    directory, "*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var child in childDirectoies)
            {
                Trace.WriteLine($"Child   : {child}.");

                // 指定ディレクトリ以下のファイルをすべて取得する
                IEnumerable<string> someFiles =
                    System.IO.Directory.EnumerateFiles(
                        child, "*", System.IO.SearchOption.AllDirectories);

                foreach (var file in someFiles)
                {
                    PathFlat.Execute(file);
                }
            }

            Trace.WriteLine("Search  : End.");
        }
    }
}
