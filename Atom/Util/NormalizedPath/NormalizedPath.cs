using System;
using System.IO;

#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Normalized path representation.
    /// Uses single slash character (<c>"/"</c>) for separating path segments and at the beginning of the path.
    /// Path having slash character at the end represents a directory, otherwise it's a file.
    /// </summary>
    public class NormalizedPath : IEquatable<NormalizedPath>, IComparable<NormalizedPath>
    {
        /// <summary>
        /// Empty relative path that corresponds to the "root".
        /// </summary>
        public static NormalizedPath Root { get; } = new("/");

        /// <summary>
        /// Returns path as a string, e.g. <c>"/MyDirectory/MyFile.txt"</c> or <c>"/MyDirectory/"</c>.
        /// </summary>
        public string RawPath { get; }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NormalizedPath(string path)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(path);
#else
            if (path == null)
                throw new ArgumentNullException(nameof(path));
#endif

            const string hint = " Normalized path uses single slash character ('/') for separating path segments and at the beginning of the path."
                + " Path having slash character at the end represents a directory, otherwise it's a file.";

            if (!path.StartsWith("/", StringComparison.Ordinal))
                throw new ArgumentException($"Path '{path}' is expected to start with '/'." + hint, nameof(path));

            void CheckInvalidCharacters(string invalid)
            {
                if (path.Contains(invalid))
                    throw new ArgumentException($"Path '{path}' is not expected to contain '{invalid}'." + hint, nameof(path));
            }

            foreach (var invalid in new[] { "//", "\\", ":", "*", "?", "\"", "<", ">", "|" })
                CheckInvalidCharacters(invalid);

            RawPath = path;
        }

        /// <summary>
        /// Returns underlying path without trailing slashes, e.g. <c>"MyDirectory/MyFile.txt"</c> or <c>"MyDirectory"</c>.
        /// </summary>
        public string InnerPath => RawPath.Trim('/');

        /// <summary>
        /// Returns <c>true</c> when underlying path is "root", i.e. <c>"/"</c>.
        /// </summary>
        public bool IsRoot => String.Equals(RawPath, "/", StringComparison.Ordinal);

        /// <summary>
        /// Returns <c>true</c> when underlying path points to a directory root, e.g. <c>"/MyDirectory/"</c>.
        /// </summary>
        public bool IsDirectory => RawPath.EndsWith("/", StringComparison.Ordinal);

        /// <summary>
        /// Creates an instance of a "root" path.
        /// </summary>
        public static NormalizedPath New() => Root;

        /// <summary>
        /// Creates an instance of a normalized path.
        /// </summary>
        public static NormalizedPath New(string path) => new(path);

        /// <summary>
        /// Gets parent path, e.g. for <c>"/MyDirectory/MyFile.txt"</c> it will return <c>"/MyDirectory/"</c>.
        /// For <c>"/MyDirectory/"</c> it will return <c>"/"</c> (<see cref="Root"/>).
        /// For root path <c>"/"</c> it will return the same value.
        /// </summary>
        public NormalizedPath GetParent()
        {
            if (IsRoot)
                return this;

            var parent = Path.GetDirectoryName(InnerPath);
            if (String.IsNullOrEmpty(parent))
                return Root;

            return new NormalizedPath("/" + parent.Replace('\\', '/') + "/");
        }

        /// <summary>
        /// Appends directory name to an existing path (can also be a sub-path of multiple directories).
        /// Will throw if the current path already denotes a file, and not a directory.
        /// </summary>
        public NormalizedPath AppendDirectory(string directoryName)
        {
            EnsureIsDirectory();
            var pathToAdd = directoryName.Trim('/');
            return new NormalizedPath(RawPath + pathToAdd + '/');
        }

        /// <summary>
        /// Appends file name to an existing path (can also be a sub-path of a file within some directory).
        /// Will throw if the current path already denotes a file, and not a directory.
        /// </summary>
        public NormalizedPath AppendFile(string fileName)
        {
            EnsureIsDirectory();
            if (fileName.EndsWith("/", StringComparison.Ordinal))
                throw new ArgumentException($"The provided value '{fileName}' more looks like a directory rather than file name.", nameof(fileName));

            var pathToAdd = fileName.Trim('/');
            return new NormalizedPath(RawPath + pathToAdd);
        }

        /// <summary>
        /// Throws if the current path is not a directory.
        /// </summary>
        private void EnsureIsDirectory()
        {
            if (!IsDirectory)
                throw new InvalidOperationException($"This method can only be used for paths that represent a directory, but the '{RawPath}' does not.");
        }

        #region Implicit conversion

        public static implicit operator NormalizedPath(string path) => new(path);
        public static implicit operator string(NormalizedPath path) => path.RawPath;

        #endregion

        #region ToString implementation

        public override string ToString() => RawPath;

        #endregion

        #region IEquatable implementation

        public bool Equals(NormalizedPath? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return RawPath == other.RawPath;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != typeof(NormalizedPath))
                return false;

            return Equals((NormalizedPath)obj);
        }

        public override int GetHashCode() => RawPath.GetHashCode();
        public static bool operator ==(NormalizedPath? left, NormalizedPath? right) => Equals(left, right);
        public static bool operator !=(NormalizedPath? left, NormalizedPath? right) => !Equals(left, right);

        #endregion

        #region IComparable implementation

        public int CompareTo(NormalizedPath? other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (ReferenceEquals(null, other))
                return 1;

            return String.Compare(RawPath, other.RawPath, StringComparison.Ordinal);
        }

        #endregion
    }
}
