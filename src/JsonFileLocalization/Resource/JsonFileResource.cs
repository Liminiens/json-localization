using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JsonFileLocalization.Caching;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Represents a json file localization resource
    /// </summary>
    public class JsonFileResource
    {
        private readonly JObject _content;
        private readonly string _baseName;
        private readonly string _location;
        private readonly ILogger<JsonFileResource> _logger;
        private readonly IJsonFileContentCache _contentCache = new JsonFileContentCache();

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
        /// <param name="baseName">resource name</param>
        /// <param name="location">location of a resource</param>
        /// <param name="filePath">file path to a resource</param>
        /// <param name="culture">culture of a resource</param>
        /// <param name="logger">logger</param>
        public JsonFileResource(
            JObject content,
            string baseName,
            string location,
            string filePath,
            CultureInfo culture,
            ILogger<JsonFileResource> logger)
        {
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _baseName = baseName;
            _location = location;
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
            _logger = logger;

            var resourceNameBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(_location))
            {
                resourceNameBuilder.Append(_location);
                resourceNameBuilder.Append(".");
            }
            resourceNameBuilder.Append(_baseName);
            ResourceName = resourceNameBuilder.ToString();
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
                var value = _contentCache.GetOrAdd($"path={path}",
                    _ => _content.SelectToken(path)).ToObject<TValue>();
                return new ValueFromResource<TValue>(value, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to retrieve object on path \"{path}\" in \"{FilePath}\"", path, FilePath);
                return new ValueFromResource<TValue>(default, false);
            }
        }

        /// <summary>
        /// Returns property names which are direct descendatns of the root object
        /// </summary>
        /// <returns>Enumeration of names</returns>
        public IEnumerable<string> GetRootPropertyNames()
        {
            return _content.Properties().Select(x => x.Path);
        }

        /// <summary>
        /// Returns string values from properties that are direct root descendants
        /// </summary>
        /// <returns>Enumeration of string values from properties that are direct root descendants</returns>
        public IEnumerable<StringValueResult> GetRootStrings()
        {
            var properties = _content.Properties()
                .Where(property =>
                    property.Value.Type != JTokenType.Array
                    && property.Value.Type != JTokenType.Object
                    && property.Value.Type != JTokenType.Property);
            return properties.Select(x => new StringValueResult(x.Path, x.Name, x.Value.Value<string>()));
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