using System;
using System.Globalization;
using JsonFileLocalization.Resource;
using Microsoft.Extensions.Logging;

namespace JsonFileLocalization.ObjectLocalization
{
    /// <summary>
    /// Service for localizing objects from resources in <see cref="JsonFileResource"/>
    /// </summary>
    public class JsonFileObjectLocalizer : IObjectLocalizer
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<JsonFileObjectLocalizer> _logger;
        /// <summary>
        /// Localization settings for application
        /// </summary>
        private readonly IJsonFileLocalizationSettings _localizationSettings;
        /// <summary>
        /// Logger factory
        /// </summary>
        private readonly ILoggerFactory _loggerFactory;
        /// <summary>
        /// Json file resource manager
        /// </summary>
        private readonly IJsonFileResourceManager _resourceManager;
        /// <summary>
        /// Resource name without culture
        /// </summary>
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
            IJsonFileLocalizationSettings localizationSettings,
            string baseName,
            string location)
        {
            _localizationSettings = localizationSettings ?? throw new ArgumentNullException(nameof(localizationSettings));

            _baseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            _location = location ?? throw new ArgumentNullException(nameof(location));

            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<JsonFileObjectLocalizer>();

            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }

        /// <summary>
        /// Json localization resources
        /// </summary>
        private JsonFileResource Resource { get; }

        /// <inheritdoc />
        public LocalizedObject<TValue> GetLocalizedObject<TValue>(string name)
        {
            var result = Resource.GetValue<TValue>(name);
            var value = result.ParseSuccess ? result.Value : default;
            if (result.ParseSuccess)
            {
                _logger.LogInformation(
                    "Retrieved object \"{name}\" of type \"{type}\" with value \"{value}\" from a resource \"{resource}\"",
                    name, typeof(TValue).FullName, result.Value, Resource.FilePath);
            }
            return new LocalizedObject<TValue>(name, value, !result.ParseSuccess, Resource.ResourceName);
        }

        /// <inheritdoc />
        public IObjectLocalizer WithCulture(CultureInfo culture)
        {
            return new JsonFileObjectLocalizer(
                _loggerFactory,
                _resourceManager,
                _resourceManager.GetResource(_baseName, _location, culture),
                _localizationSettings,
                _baseName,
                _location);
        }
    }
}