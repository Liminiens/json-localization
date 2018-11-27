using System;
using System.IO;
using System.Linq;
using JsonFileLocalization.Caching;

namespace JsonFileLocalization.Resource
{
    public class ResourceFileManager
    {
        private readonly ConcurrentDictionaryCache<string, string> _pathCache =
            new ConcurrentDictionaryCache<string, string>();

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
            if (!_pathCache.TryGet(resource, out var value))
            {
                var path = FindFile(resource);
                if (path != null && !_pathCache.TryAdd(resource, path))
                {
                    return GetOrFindFile(resource);
                }
            }
            return value;
        }

        public string GetResourceNameByPath(string path)
        {
            _pathCache.TryGet(path, out var resourceName);
            return resourceName;
        }
    }
}