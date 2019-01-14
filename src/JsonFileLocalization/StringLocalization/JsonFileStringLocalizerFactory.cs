using System;
using System.Globalization;
using JsonFileLocalization.Resource;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace JsonFileLocalization.StringLocalization
{
    public class JsonFileStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IJsonFileResourceManager _resourceManager;

        public JsonFileStringLocalizerFactory(
            ILoggerFactory loggerFactory,
            IJsonFileResourceManager resourceManagerManager)
        {
            _loggerFactory = loggerFactory;
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
                return new JsonFileStringLocalizer(_loggerFactory, _resourceManager, resource, baseName, location);
            }
            return null;
        }
    }
}
