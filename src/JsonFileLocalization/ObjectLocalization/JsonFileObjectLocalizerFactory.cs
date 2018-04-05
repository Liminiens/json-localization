using System;
using System.Globalization;
using JsonFileLocalization.Resource;
using Microsoft.Extensions.Logging;

namespace JsonFileLocalization.ObjectLocalization
{
    /// <summary>
    /// Factory of <see cref="JsonFileObjectLocalizer"/> objects
    /// </summary>
    public class JsonFileObjectLocalizerFactory : IObjectLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IJsonFileLocalizationSettings _fileBasedLocalizationSettings;
        private readonly IJsonFileResourceManager _resourceManager;

        public JsonFileObjectLocalizerFactory(
            ILoggerFactory loggerFactory,
            IJsonFileLocalizationSettings fileBasedLocalizationSettings,
            IJsonFileResourceManager resourceManagerManager)
        {
            _loggerFactory = loggerFactory;
            _fileBasedLocalizationSettings = fileBasedLocalizationSettings;
            _resourceManager = resourceManagerManager;
        }

        /// <inheritdoc />
        public IObjectLocalizer Create(Type resourceSource)
        {
            var typeName = resourceSource.Name;
            var assemblyName = resourceSource.Assembly.GetName().Name;
            return Create(typeName, assemblyName);
        }

        /// <inheritdoc />
        public IObjectLocalizer Create(string baseName, string location)
        {
            return new JsonFileObjectLocalizer(_loggerFactory, _resourceManager,
                _resourceManager.GetResource(baseName, location, CultureInfo.CurrentUICulture),
                _fileBasedLocalizationSettings, baseName, location);
        }
    }
}