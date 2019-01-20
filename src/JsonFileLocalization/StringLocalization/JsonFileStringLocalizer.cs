using System;
using System.Collections.Generic;
using System.Globalization;
using JsonFileLocalization.Resource;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

/* ResourceManager string localizer source:
 * https://github.com/aspnet/Localization/blob/51549e8471c247f91d5ac57bd6f8f4c68508854b/src/Microsoft.Extensions.Localization/ResourceManagerStringLocalizer.cs
 *
 * With culture:
 * https://github.com/aspnet/Localization/blob/f260d4e5244ca536c5fcc05ccea1163548c6eddc/src/Microsoft.Extensions.Localization/ResourceManagerWithCultureStringLocalizer.cs
 */

namespace JsonFileLocalization.StringLocalization
{
    public class JsonFileStringLocalizer : IStringLocalizer
    {
        private readonly ILogger<JsonFileStringLocalizer> _logger;
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

        public JsonFileStringLocalizer(
            ILoggerFactory loggerFactory,
            IJsonFileResourceManager resourceManager,
            JsonFileResource resource,
            string baseName,
            string location)
        {
            _baseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            _location = location ?? throw new ArgumentNullException(nameof(location));

            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<JsonFileStringLocalizer>();

            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
        
        private JsonFileResource Resource { get; }

        public CultureInfo Culture => Resource.Culture;

        /// <summary>
        /// Method for retrieving string data from resource by JPath
        /// </summary>
        private ValueFromResource<string> GetString(string jsonPropertyPath)
        {
            var result = Resource.GetValue<string>(jsonPropertyPath);
            if (result.ParseSuccess)
            {
                _logger.LogDebug(
                    "Retrieved resource \"{path}\" with value \"{result}\" from file \"{filePath}\" with culture \"{culture}\"",
                    jsonPropertyPath, result.Value, Resource.FilePath, Culture.Name);
            }
            return result;
        }

        private LocalizedString GetLocalizedString(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var value = GetString(name);
            return new LocalizedString(name, value.Value, !value.ParseSuccess, Resource.ResourceName);
        }

        private LocalizedString GetLocalizedString(string name, object[] arguments)
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
                /*
                 * Example from ResourceManagerStringLocalizer:
                 * https://github.com/aspnet/Localization/blob/51549e8471c247f91d5ac57bd6f8f4c68508854b/src/Microsoft.Extensions.Localization/ResourceManagerStringLocalizer.cs#L236
                 */
                while (!parentCulture.Equals(currentCulture) && !currentCulture.Parent.Equals(CultureInfo.InvariantCulture))
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
            var resource = _resourceManager.GetResource(_baseName, _location, culture);
            if (resource != null)
            {
                return new JsonFileStringLocalizer(
                    _loggerFactory, _resourceManager, resource,
                    _baseName, _location);
            }
            return null;
        }

        /// <inheritdoc />
        public virtual LocalizedString this[string name] => GetLocalizedString(name);

        /// <inheritdoc />
        public virtual LocalizedString this[string name, params object[] arguments] => GetLocalizedString(name, arguments);
    }
}