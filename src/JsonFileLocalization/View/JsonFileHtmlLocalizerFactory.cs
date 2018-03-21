using System;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.View
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
            return new JsonFileHtmlLocalizer(_factory.Create(resourceSource));
        }

        /// <inheritdoc />
        public IHtmlLocalizer Create(string baseName, string location)
        {
            return new JsonFileHtmlLocalizer(_factory.Create(baseName, location));
        }
    }
}