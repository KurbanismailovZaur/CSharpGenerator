using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Redcode.CreateMenuContext
{
    public static class Extensions
    {
        public static string GenerateNamespace(this string path)
        {
            path = path[..^".meta".Length].Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            path = path[(path.IndexOf(Path.DirectorySeparatorChar) + 1)..];

            var index = path.LastIndexOf(Path.DirectorySeparatorChar);
            path = index == -1 ? string.Empty : path.Substring(0, index);

            index = path.LastIndexOf("Scripts");

            if (index != -1)
            {
                if ((index == 0 || path[index - 1] == Path.DirectorySeparatorChar) && (index + "Scripts".Length == path.Length || path[index + "Scripts".Length] == Path.DirectorySeparatorChar))
                    path = index + "Scripts".Length == path.Length ? string.Empty : path.Substring(index + "Scripts".Length + 1);
            }

            path = path.Replace(Path.DirectorySeparatorChar, '.');
            var @namespace = (path == string.Empty ? Application.productName : $"{Application.productName}.{path}").Replace(" ", "");

            return @namespace;
        }

        public static bool IsDirectory(this string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        public static bool IsCSharpFile(this string path) =>
            Path.GetExtension(path) == ".cs";


        public static List<string> GetFilePaths(this string path)
        {
            var files = new List<string>();
            var dirs = new Queue<string>();
            dirs.Enqueue(path);

            while (dirs.Count > 0)
            {
                int size = dirs.Count;

                for (int i = 0; i < size; i++)
                {
                    var node = dirs.Dequeue();
                    var dirPaths = Directory.GetDirectories(node);
                    files.AddRange(Directory.GetFiles(node));

                    foreach (var pathToDir in dirPaths)
                        dirs.Enqueue(pathToDir);
                }
            }

            return files;
        }
    }
}