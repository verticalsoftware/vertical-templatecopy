using Microsoft.Extensions.Logging;
using System;
using System.IO;
using static System.Environment;

namespace Vertical.TemplateCopy.Utilities
{
    public class FileSystem
    {
        public static readonly Func<string, string, FileOperation> CancelFileOperation = (_, __) => FileOperation.Cancel;
        public static readonly Func<string, string, FileOperation> ReplaceFileOperation = (_, __) => FileOperation.Replace;
        public static readonly Func<string, string, FileOperation> SkipFileOperation = (_, __) => FileOperation.Skip;

        private readonly ILogger<FileSystem> logger;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="logger">Logger</param>
        public FileSystem(ILogger<FileSystem> logger) => this.logger = logger;

        /// <summary>
        /// Copies content from one directory to another.
        /// </summary>
        /// <param name="source">The source directory.</param>
        /// <param name="dest">The target directory.</param>
        /// <param name="recurse">Whether to recurse.</param>
        /// <param name="contentTransform">Content transform function.</param>
        /// <param name="duplicateHandling">Duplicate handling function.</param>
        /// <remarks>
        /// Copies content from <paramref name="source"/> to <paramref name="dest"/>.
        /// Note <paramref name="dest"/> must exist.
        /// </remarks>
        public FileOperation CopyDirectory(string source
            , string dest
            , bool recurse
            , Func<string, string, FileOperation> duplicateHandling
            , Func<string, string> contentTransform = null)
        {
            logger.LogTrace("CopyDirectory {source} > {dest}", source, dest);

            foreach (var file in Directory.EnumerateFiles(source))
            {
                if (CopyFile(file, Path.Combine(dest, Path.GetFileName(file)), duplicateHandling, contentTransform) == FileOperation.Cancel)
                    return FileOperation.Cancel;
            }

            if (!recurse) return FileOperation.Replace;

            foreach (var directory in Directory.EnumerateDirectories(source))
            {
                var target = Path.Combine(dest, Path.GetFileName(directory));
                CreateDirectory(target);
                if (CopyDirectory(directory, target, recurse, duplicateHandling, contentTransform) == FileOperation.Cancel)
                    break;
            }

            return FileOperation.Replace;
        }

        /// <summary>
        /// Copies a file.
        /// </summary>
        /// <param name="source">Source file.</param>
        /// <param name="dest">Path to dest file.</param>
        /// <param name="duplicateHandling">Function that determines duplicate handling.</param>
        /// <param name="contentTransform">Content transform function or a null reference.</param>
        /// <returns></returns>
        public FileOperation CopyFile(string source
            , string dest
            , Func<string, string, FileOperation> duplicateHandling
            , Func<string, string> contentTransform = null)
        {
            var exists = File.Exists(dest);
            var operation = exists ? (duplicateHandling ?? CancelFileOperation)(source, dest) : FileOperation.Replace;

            logger.LogTrace("CopyFile (FileOperation.{op}, exists={exists}) {source} > {dest}"
                , operation, exists, source, dest);

            if (operation != FileOperation.Replace)
                return operation;

            if (contentTransform == null)
            {
                File.Copy(source, dest, true);
                return operation;
            }

            File.WriteAllText(dest, contentTransform(File.ReadAllText(source)));

            return operation;
        }

        /// <summary>
        /// Cleans a directory
        /// </summary>
        /// <param name="path">Path</param>
        public void CleanDirectory(string path)
        {
            logger.LogTrace("CleanDirectory {path}", path);

            foreach (var file in Directory.GetFiles(path))
                DeleteFile(file);

            foreach (var directory in Directory.GetDirectories(path))
                DeleteDirectoryFast(directory, true);
        }

        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <param name="path">Path</param>
        public void CreateDirectory(string path)
        {
            logger.LogTrace("CreateDirectory {path}", path);
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Deletes a directory
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="recurse">Whether to recurse</param>
        public void DeleteDirectory(string path, bool recurse)
        {
            logger.LogTrace("DeleteDirectory {path} (recurse={recurse})", path, recurse);

            foreach (var file in Directory.GetFiles(path))
                DeleteFile(path);

            if(!recurse) { return; }

            foreach (var directory in Directory.GetDirectories(path))
                DeleteDirectory(directory, true);
        }
        
        /// <summary>
        /// Deletes a directory without manual recursing.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="recurse">Whether to recurse</param>
        public void DeleteDirectoryFast(string path, bool recurse)
        {
            logger.LogTrace("DeleteDirectoryFast {path} (recurse={recurse})", path, recurse);
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="path">Path</param>
        public void DeleteFile(string path)
        {
            logger.LogTrace("DeleteFile {path}", path);
            File.SetAttributes(path, FileAttributes.Normal);
            File.Delete(path);
        }

        /// <summary>
        /// Gets a temporary directory name.
        /// </summary>
        /// <returns></returns>
        public string GetTemporaryDirectoryName()
        {
            for(; ; )
            {
                var path = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData)
                    , "Vertical.TemplateCopy"
                    , Path.GetRandomFileName());

                logger.LogTrace("Generate random file name = {path}", path);

                if (!Directory.Exists(path))
                    return path;
            }
        }
    }
}
