using System;
using System.Collections.Generic;
using System.Globalization;
using JsonFileLocalization.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace JsonFileLocalization
{
    /* ResourceManager string localizer source:
     * https://github.com/aspnet/Localization/blob/51549e8471c247f91d5ac57bd6f8f4c68508854b/src/Microsoft.Extensions.Localization/ResourceManagerStringLocalizer.cs
     *
     * With culture:
     * https://github.com/aspnet/Localization/blob/f260d4e5244ca536c5fcc05ccea1163548c6eddc/src/Microsoft.Extensions.Localization/ResourceManagerWithCultureStringLocalizer.cs
     */
    /// <summary>
    /// String localizer based on json files
    /// </summary>
    public class JsonFileStringLocalizer : IStringLocalizer
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<JsonFileStringLocalizer> _logger;
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

        /// <summary>
        /// Json localization resources
        /// </summary>
        private JsonFileResource Resource { get; }

        public JsonFileStringLocalizer(
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
            _logger = loggerFactory.CreateLogger<JsonFileStringLocalizer>();

            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }

        /// <summary>
        /// Culture of localizer
        /// </summary>
        public CultureInfo Culture => Resource.Culture;

        /// <summary>
        /// Method for retrieving string data from resource by JPath
        /// </summary>
        /// <param name="jsonPropertyPath">JPath for string</param>
        /// <returns>Value and if resource was searched by full name (location.baseName)</returns>
        private ValueFromResource<string> GetString(in string jsonPropertyPath)
        {
            var result = Resource.GetValue<string>(jsonPropertyPath);
            if (result.ParseSuccess)
            {
                _logger.LogInformation(
                    "Got resource \"{result}\" from file \"{FilePath}\" with culture \"{Culture}\"",
                    result.Value, Resource.FilePath, Culture.Name);
            }
            return result;
        }

        private LocalizedString GetLocalizedString(in string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var value = GetString(name);
            return new LocalizedString(name, value.Value, !value.ParseSuccess, Resource.ResourceName);
        }

        private LocalizedString GetLocalizedString(in string name, in object[] arguments)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var value = GetString(name);
            var formatted = String.Format(value.Value, arguments);
            return new LocalizedString(name, formatted, !value.ParseSuccess, Resource.ResourceName);
        }

        private IEnumerable<LocalizedString> GetResourceStringsFromCultureHierarchy(
            CultureInfo startingCulture, bool includeParentCultures)
        {
            foreach (var str in Resource.GetRootStrings())
            {
                yield return new LocalizedString(str.Path, str.Value, true, Resource.ResourceName);
            }

            if (includeParentCultures)
            {
                var currentCulture = startingCulture;
                var parentCulture = currentCulture.Parent;
                //https://github.com/aspnet/Localization/blob/51549e8471c247f91d5ac57bd6f8f4c68508854b/src/Microsoft.Extensions.Localization/ResourceManagerStringLocalizer.cs#L236
                while (!parentCulture.Equals(currentCulture)
                       && !currentCulture.Parent.Equals(CultureInfo.InvariantCulture))
                {
                    var resource = _resourceManager.GetResource(_baseName, _location, parentCulture);
                    foreach (var str in resource.GetRootStrings())
                    {
                        yield return new LocalizedString(str.Path, str.Value, true, Resource.ResourceName);
                    }
                    currentCulture = parentCulture;
                    parentCulture = currentCulture.Parent;
                }
            }
        }
        /// <inheritdoc />
        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return GetResourceStringsFromCultureHierarchy(Culture, includeParentCultures);
        }

        /// <inheritdoc />
        public virtual IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new JsonFileStringLocalizer(_loggerFactory, _resourceManager,
                _resourceManager.GetResource(_baseName, _location, culture),
                _localizationSettings, _baseName, _location);
        }

        /// <inheritdoc />
        public virtual LocalizedString this[string name]
            => GetLocalizedString(name);


        /// <inheritdoc />
        public virtual LocalizedString this[string name, params object[] arguments]
            => GetLocalizedString(name, arguments);
    }
}