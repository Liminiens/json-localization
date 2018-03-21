using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.View
{
    /*
     * ViewLocalizer source
     * https://github.com/aspnet/Mvc/blob/760c8f38678118734399c58c2dac981ea6e47046/src/Microsoft.AspNetCore.Mvc.Localization/ViewLocalizer.cs
     */

    public class JsonViewLocalizer : IViewLocalizer, IViewContextAware
    {
        private readonly IHtmlLocalizerFactory _factory;
        private string _viewPrefix = String.Empty;
        private IHtmlLocalizer _localizer;

        public JsonViewLocalizer(IHtmlLocalizerFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc />
        public LocalizedString GetString(string name)
        {
            return _localizer.GetString(name);
        }

        /// <inheritdoc />
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return _localizer.GetString(name, arguments);
        }

        /// <inheritdoc />
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        /// <inheritdoc />
        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            return _localizer.WithCulture(culture);
        }

        /// <inheritdoc />
        public LocalizedHtmlString this[string name]
            => _localizer[name];

        /// <inheritdoc />
        public LocalizedHtmlString this[string name, params object[] arguments]
            => _localizer[name, arguments];

        /// <inheritdoc />
        public void Contextualize(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            // Given a view path "/Views/Home/Index.cshtml" we want a baseName like "Views.Home.Index"
            var path = viewContext.ExecutingFilePath;

            if (string.IsNullOrEmpty(path))
            {
                path = viewContext.View.Path;
            }

            Debug.Assert(!string.IsNullOrEmpty(path), "Couldn't determine a path for the view");

            //Turns view path to a resource name without culture
            _viewPrefix = GetPrefix(path);

            _localizer = _factory.Create(_viewPrefix, String.Empty);
        }

        private string GetPrefix(in string viewPath)
        {
            var builder = new StringBuilder(viewPath);
            var extension = Path.GetExtension(viewPath);
            builder.Replace(extension, String.Empty);
            builder.Replace(Path.DirectorySeparatorChar, '.');
            builder.Replace(Path.AltDirectorySeparatorChar, '.');
            if (builder[0] == '.')
            {
                builder.Remove(0, 1); //removes leading '.'
            }
            return builder.ToString();
        }
    }
}