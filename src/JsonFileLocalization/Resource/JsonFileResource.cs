using JsonFileLocalization.Caching;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Represents a json file localization resource
    /// </summary>
    public class JsonFileResource
    {
        private readonly JObject _content;
        private readonly ILogger<JsonFileResource> _logger;
        private readonly IJsonFileContentCache _contentCache;

        /// <summary>
        /// Path to a resource file
        /// </summary>
        public readonly string FilePath;

        /// <summary>
        /// Culture of a resource file
        /// </summary>
        public readonly CultureInfo Culture;

        /// <summary>
        /// Creates a <see cref="JsonFileResource"/>
        /// </summary>
        /// <param name="content">parsed content of a file</param>
        /// <param name="resourceName">resource name</param>
        /// <param name="filePath">file path to a resource</param>
        /// <param name="culture">culture of a resource</param>
        /// <param name="logger">logger</param>
        /// <param name="cacheFactory">file content cache factory</param>
        public JsonFileResource(
            JObject content,
            string resourceName,
            string filePath,
            CultureInfo culture,
            ILogger<JsonFileResource> logger,
            IJsonFileContentCacheFactory cacheFactory)
        {
            _content = content ?? throw new ArgumentNullException(nameof(content));
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
            _contentCache = cacheFactory.Create() ?? throw new ArgumentNullException(nameof(cacheFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ResourceName = resourceName;
        }

        /// <summary>
        /// Name of a resource
        /// </summary>
        public string ResourceName { get; }

        /// <summary>
        /// Gets a value on a specified path or a default value if can't convert value to a specified type
        /// </summary>
        /// <typeparam name="TValue">Return type</typeparam>
        /// <param name="path">Property path</param>
        /// <returns>Typed value on a path from a resource</returns>
        public ValueFromResource<TValue> GetValue<TValue>(string path)
        {
            try
            {
                var value = (TValue)_contentCache.GetOrAdd(
                    $"resource={ResourceName};path={path};type={typeof(TValue).Name}",
                    _ => _content.SelectToken(path).ToObject<TValue>());

                return new ValueFromResource<TValue>(value, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to retrieve object on path \"{path}\" in \"{FilePath}\"", path, FilePath);
                return new ValueFromResource<TValue>(default, false);
            }
        }

        /// <summary>
        /// Returns string values from properties that are direct root descendants
        /// </summary>
        /// <returns>Enumeration of string values from properties that are direct root descendants</returns>
        public IEnumerable<StringFromResource> GetRootStrings()
        {
            return (IEnumerable<StringFromResource>)_contentCache.GetOrAdd($"resource={ResourceName};all_strings", key =>
            {
                return _content.Properties()
                    .Where(property =>
                        property.Value.Type != JTokenType.Array
                        && property.Value.Type != JTokenType.Object
                        && property.Value.Type != JTokenType.Property)
                    .Select(x => new StringFromResource(x.Path, x.Value.Value<string>()))
                    .ToList();
            });
        }

        protected bool Equals(JsonFileResource other)
        {
            return string.Equals(FilePath, other.FilePath);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is JsonFileResource resource)
            {
                return Equals(resource);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (FilePath != null ? FilePath.GetHashCode() : 0);
        }
    }
}