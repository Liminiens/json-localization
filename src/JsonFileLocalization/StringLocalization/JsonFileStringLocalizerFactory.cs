using System;
using System.Globalization;
using JsonFileLocalization.Middleware;
using JsonFileLocalization.Resource;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

/*
 * ResourceManagerStringLocalizerFactory HtmlLocalizerFactory
 */

namespace JsonFileLocalization.StringLocalization
{
    /// <summary>
    /// Factory for string localizers based on Json file resources
    /// </summary>
    public class JsonFileStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly JsonFileLocalizationSettings _fileBasedLocalizationSettings;
        private readonly IJsonFileResourceManager _resourceManager;

        public JsonFileStringLocalizerFactory(
            ILoggerFactory loggerFactory,
            JsonFileLocalizationSettings fileBasedLocalizationSettings,
            IJsonFileResourceManager resourceManagerManager)
        {
            _loggerFactory = loggerFactory;
            _fileBasedLocalizationSettings = fileBasedLocalizationSettings;
            _resourceManager = resourceManagerManager;
        }

        /// <inheritdoc />
        public IStringLocalizer Create(Type resourceSource)
        {
            var typeName = resourceSource.FullName;
            var assemblyName = resourceSource.Assembly.GetName().Name;
            return Create(typeName, assemblyName);
        }

        /// <inheritdoc />
        public IStringLocalizer Create(string baseName, string location)
        {
            //location is a prefix to a resource name
            var resource = _resourceManager.GetResource(baseName, location, CultureInfo.CurrentUICulture);
            if (resource != null)
            {
                return new JsonFileStringLocalizer(
                    _loggerFactory, _resourceManager, resource,
                    _fileBasedLocalizationSettings, baseName, location);
            }
            return null;
        }
    }
}
