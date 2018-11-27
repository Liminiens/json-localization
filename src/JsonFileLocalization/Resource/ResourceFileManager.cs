using JsonFileLocalization.Resource.Utility;
using System;
using System.IO;
using System.Linq;

namespace JsonFileLocalization.Resource
{
    internal class ResourceFileManager
    {
        private readonly BiDictionary<string, string> _cache = new BiDictionary<string, string>();

        private static string FindFile(string resource)
        {
            var fileName = Path.GetFileName(resource);
            var directory = Path.GetDirectoryName(resource);
            var parts = fileName.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                var directoryPart = String.Join(".", parts.Take(i + 1));
                var filePart = String.Join(".", parts.Skip(i + 1));
                var filePath = Path.Combine(directory, directoryPart, filePart);
                if (File.Exists(filePath))
                {
                    return filePath;
                }
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
            lock (_cache)
            {
                if (!_cache.TryGetByFirst(resource, out var value))
                {
                    var path = FindFile(resource);
                    if (path != null)
                    {
                        _cache.Add(resource, path);
                        return path;
                    }
                    return null;
                }
                return value;
            }
        }

        public string GetResourceNameByPath(string path)
        {
            lock (_cache)
            {
                _cache.TryGetBySecond(path, out var resource);
                return resource;
            }
        }
    }
}