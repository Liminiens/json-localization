using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace JsonFileLocalization.Resource
{
    internal class FilePathCache
    {
        private readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

        private static string FindFile(string resource)
        {
            var fileName = Path.GetFileName(resource);
            var directory = Path.GetDirectoryName(resource);
            var parts = fileName.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                //single directory with dots in name
                var directoryPart = String.Join(".", parts.Take(i + 1));
                var filePart = String.Join(".", parts.Skip(i + 1));
                var filePath = Path.Combine(directory, directoryPart, filePart);
                if (File.Exists(filePath))
                {
                    return filePath;
                }
                //directory tree
                var directories = String.Join(Path.DirectorySeparatorChar, parts.Take(i + 1));
                filePath = Path.Combine(directory, directories, filePart);
                if (File.Exists(filePath))
                {
                    return filePath;
                }
            }
            return null;
        }

        public string GetOrFindFile(string resource)
        {
            if (!_cache.TryGetValue(resource, out var value))
            {
                var path = FindFile(resource);
                if (path != null)
                {
                    if (!_cache.TryAdd(resource, path))
                    {
                        return GetOrFindFile(resource);
                    }
                    return path;
                }
                return null;
            }
            return value;
        }
    }
}