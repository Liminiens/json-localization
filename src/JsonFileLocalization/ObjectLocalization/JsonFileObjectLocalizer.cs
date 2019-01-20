using System;
using System.Globalization;
using JsonFileLocalization.Resource;
using Microsoft.Extensions.Logging;

namespace JsonFileLocalization.ObjectLocalization
{
    public class JsonFileObjectLocalizer : IObjectLocalizer
    {
        private readonly ILogger<JsonFileObjectLocalizer> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IJsonFileResourceManager _resourceManager;
        private readonly string _baseName;
        /// <summary>
        /// Resource assembly name.
        /// <para>
        ///     <example>
        ///     Example: MyAssembly.MyType.en.json has location of MyAssembly and baseName of MyType
        ///     </example>
        /// </para>
        /// </summary>
        private readonly string _location;

        public JsonFileObjectLocalizer(
            ILoggerFactory loggerFactory,
            IJsonFileResourceManager resourceManager,
            JsonFileResource resource,
            string baseName,
            string location)
        {
            _baseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            _location = location ?? throw new ArgumentNullException(nameof(location));

            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<JsonFileObjectLocalizer>();

            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
        
        private JsonFileResource Resource { get; }

        /// <inheritdoc />
        public LocalizedObject<TValue> GetLocalizedObject<TValue>(string name)
        {
            var result = Resource.GetValue<TValue>(name);
            var value = result.ParseSuccess ? result.Value : default;
            if (result.ParseSuccess)
            {
                _logger.LogDebug(
                    "Retrieved object \"{name}\" of type \"{type}\" with value \"{value}\" from a resource \"{resource}\"",
                    name, typeof(TValue).FullName, result.Value, Resource.FilePath);
            }
            return new LocalizedObject<TValue>(name, value, !result.ParseSuccess, Resource.ResourceName);
        }

        /// <inheritdoc />
        public IObjectLocalizer WithCulture(CultureInfo culture)
        {
            var resource = _resourceManager.GetResource(_baseName, _location, culture);
            if (resource != null)
            {
                return new JsonFileObjectLocalizer(
                    _loggerFactory, _resourceManager, resource,
            _baseName, _location);
            }
            return null;
        }
    }
}