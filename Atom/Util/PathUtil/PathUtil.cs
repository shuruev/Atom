using System;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Helper methods for working with I/O and local files.
    /// </summary>
    public static class PathUtil
    {
        /// <summary>
        /// Makes sure that specified path was absolute (i.e. non-relative),
        /// and throws <see cref="InvalidOperationException"/> otherwise.
        /// </summary>
        public static void EnsureAbsolute(string path)
        {
            if (!Path.IsPathFullyQualified(path))
                throw new InvalidOperationException($"Path should be absolute: {path}");
        }

        /// <summary>
        /// Scans specified path with various parameters. Can search for files and/or directories,
        /// and use custom predicate functions for whether an item (file or directory) should be returned,
        /// as well as whether the search should continue recursively.
        /// </summary>
        /// <param name="pathToScan">Base path to scan. All result items will get their relative paths based on this location.</param>
        /// <param name="recursive">Whether scan should continue recursively, or be limited to the specified directory only</param>
        /// <param name="includeFiles">Whether files should be included as result items</param>
        /// <param name="includeDirectories">Whether directories should be included as result items</param>
        /// <param name="matchPredicate">
        /// A function determining whether an item (file or directory) should be included into result or ignored, e.g.
        /// <code>
        /// // find files by extension (case-insensitive)
        /// matchPredicate: item =>
        ///     String.Equals(Path.GetExtension(item.Name), ".txt", StringComparison.OrdinalIgnoreCase)
        /// </code>
        /// </param>
        /// <param name="scanPredicate">
        /// A function determining whether the search should continue for this directory, e.g.
        /// <code>
        /// // do not search in certain folders
        /// scanPredicate: item =>
        ///     item.Name != ".git"
        ///     && item.Name != "bin"
        ///     && item.Name != "obj"
        ///     && item.Name != "node_modules"
        /// </code>
        /// Not used when <paramref name="recursive"/> is <c>false</c>.
        /// </param>
        /// <returns>A list of items (files and directories) that were found</returns>
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
        }
    }

    /// <summary>
    /// Holds file (or directory) position, relatively to some specific base path.
    /// </summary>
    public class PathItem
    {
        /// <summary>
        /// File (or directory name), e.g. "MyFile.txt".
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Indicates whether current item is a directory.
        /// </summary>
        public bool IsDirectory { get; set; }
        /// <summary>
        /// Absolute path, e.g. "C:\My\Files\MyFile.txt".
        /// </summary>
        public string AbsolutePath { get; set; } = null!;
        /// <summary>
        /// Relative path, e.g. "/Files/MyFile.txt".
        /// </summary>
        public string RelativePath { get; set; } = null!;
    }
}
