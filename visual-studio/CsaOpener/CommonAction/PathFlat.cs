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
        /// ディレクトリーの中を全部実行。
        /// </summary>
        /// <param name="directory">ディレクトリー。</param>
        public static void Search(string directory)
        {
            Trace.WriteLine($"Search  : {directory}.");

            // 再帰呼出し。
            PathFlat.SearchSub(directory);

            Trace.WriteLine("Search  : End.");
        }

        /// <summary>
        /// 再帰サーチ。
        /// </summary>
        /// <param name="directory">ディレクトリー。</param>
        private static void SearchSub(string directory)
        {
            IEnumerable<string> childDirectories =
                System.IO.Directory.EnumerateDirectories(
                    directory, "*", System.IO.SearchOption.TopDirectoryOnly);

            foreach (var child in childDirectories)
            {
                // 再帰呼出し。
                PathFlat.SearchSub(child);
            }

            // この階層のファイル。
            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    directory, "*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                PathFlat.RenameFile(file);
            }
        }

        /// <summary>
        /// 実行。
        /// </summary>
        /// <param name="file">ファイル名。</param>
        private static void RenameFile(string file)
        {
            var joinedName = $"{Directory.GetParent(file).Name}$%{Path.GetFileName(file)}";
            var parentParentDirectory = Directory.GetParent(Directory.GetParent(file).FullName).FullName;
            var destination = Path.Combine(parentParentDirectory, joinedName);

            Trace.WriteLine($"Rename  : {file} -> {destination}.");
            File.Move(file, destination);
        }
    }
}
