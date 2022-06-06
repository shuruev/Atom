using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atom.Util
{
    /// <summary>
    /// Helper methods for working with I/O and local files.
    /// </summary>
    public static class PathUtil
    {
        /*public static void EnsureAbsolute(string path)
        {
            if (!Path.IsPathFullyQualified(path))
                throw new InvalidOperationException($"Path should be absolute: {path}");
        }

        public static List<PathItem> Scan(
            string pathToScan,
            bool recursive = true,
            bool includeFiles = true,
            bool includeDirectories = true,
            Func<PathItem, bool>? matchPredicate = null,
            Func<PathItem, bool>? scanPredicate = null)
        {
            EnsureAbsolute(pathToScan);
            var result = new List<PathItem>();
            ScanPath("/", pathToScan, result, recursive, includeFiles, includeDirectories, matchPredicate, scanPredicate);
            return result;
        }

        private static void ScanPath(
            string relativePath,
            string currentPath,
            List<PathItem> result,
            bool recursive,
            bool includeFiles,
            bool includeDirectories,
            Func<PathItem, bool>? matchPredicate,
            Func<PathItem, bool>? scanPredicate)
        {
            if (includeFiles)
            {
                foreach (var file in Directory.GetFiles(currentPath, "*", SearchOption.TopDirectoryOnly))
                {
                    var fileName = Path.GetFileName(file);
                    var fileItem = new PathItem
                    {
                        Name = fileName,
                        IsDirectory = false,
                        AbsolutePath = file,
                        RelativePath = $"{relativePath}{fileName}"
                    };

                    if (matchPredicate == null || matchPredicate.Invoke(fileItem))
                    {
                        result.Add(fileItem);
                    }
                }
            }

            if (includeDirectories || recursive)
            {
                foreach (var dir in Directory.GetDirectories(currentPath, "*", SearchOption.TopDirectoryOnly))
                {
                    var dirName = Path.GetFileName(dir);
                    var dirItem = new PathItem
                    {
                        Name = dirName,
                        IsDirectory = true,
                        AbsolutePath = dir,
                        RelativePath = $"{relativePath}{dirName}/"
                    };

                    if (includeDirectories)
                    {
                        if (matchPredicate == null || matchPredicate.Invoke(dirItem))
                        {
                            result.Add(dirItem);
                        }
                    }

                    if (recursive)
                    {
                        if (scanPredicate == null || scanPredicate.Invoke(dirItem))
                        {
                            ScanPath(dirItem.RelativePath, dir, result, recursive, includeFiles, includeDirectories, matchPredicate, scanPredicate);
                        }
                    }
                }
            }
        }*/
    }

    public class PathItem
    {
        //public string Name { get; set; } = null!;
        //public bool IsDirectory { get; set; }
        //public string AbsolutePath { get; set; } = null!;
        //public string RelativePath { get; set; } = null!;
    }
}
