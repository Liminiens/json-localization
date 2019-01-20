using System;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.ViewLocalization
{
    public class JsonFileHtmlLocalizerFactory : IHtmlLocalizerFactory
    {
        private readonly IStringLocalizerFactory _factory;

        public JsonFileHtmlLocalizerFactory(IStringLocalizerFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        /// <inheritdoc />
        public IHtmlLocalizer Create(Type resourceSource)
        {
            var localizer = _factory.Create(resourceSource);
            if (localizer != null)
            {
                return new JsonFileHtmlLocalizer(localizer);
            }
            return null;
        }

        /// <inheritdoc />
        public IHtmlLocalizer Create(string baseName, string location)
        {
            var localizer = _factory.Create(baseName, location);
            if (localizer != null)
            {
                return new JsonFileHtmlLocalizer(localizer);
            }
            return null;
        }
    }
}