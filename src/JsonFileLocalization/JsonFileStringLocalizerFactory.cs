﻿using System;
using System.Globalization;
using JsonFileLocalization.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace JsonFileLocalization
{
    /*
     * ResourceManagerStringLocalizerFactory source:
     * https://github.com/aspnet/Localization/blob/bab2a50ec1a780585ffa89756abd1d995b7f3f17/src/Microsoft.Extensions.Localization/ResourceManagerStringLocalizerFactory.cs
     *
     * HtmlLocalizerFactory source
     * https://github.com/aspnet/Mvc/blob/760c8f38678118734399c58c2dac981ea6e47046/src/Microsoft.AspNetCore.Mvc.Localization/HtmlLocalizerFactory.cs
     */

    /// <summary>
    /// Factory for string localizers based on Json file resources
    /// </summary>
    public class JsonFileStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IJsonFileLocalizationSettings _fileBasedLocalizationSettings;
        private readonly IJsonFileResourceManager _resourceManager;

        public JsonFileStringLocalizerFactory(
            ILoggerFactory loggerFactory,
            IJsonFileLocalizationSettings fileBasedLocalizationSettings,
            IJsonFileResourceManager resourceManagerManager)
        {
            _loggerFactory = loggerFactory;
            _fileBasedLocalizationSettings = fileBasedLocalizationSettings;
            _resourceManager = resourceManagerManager;
        }

        /// <inheritdoc />
        public IStringLocalizer Create(Type resourceSource)
        {
            var typeName = resourceSource.Name;
            var assemblyName = resourceSource.Assembly.GetName().Name;
            return Create(typeName, assemblyName);
        }

        /// <inheritdoc />
        public IStringLocalizer Create(string baseName, string location)
        {
            //location is a prefix to a resource name
            return new JsonFileStringLocalizer(_loggerFactory,_resourceManager,
                _resourceManager.GetResource(baseName, location, CultureInfo.CurrentUICulture),
                _fileBasedLocalizationSettings, baseName, location);
        }
    }
}
