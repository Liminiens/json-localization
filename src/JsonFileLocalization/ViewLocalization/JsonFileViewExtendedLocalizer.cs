using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using JsonFileLocalization.ObjectLocalization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.ViewLocalization
{
    public class JsonFileViewExtendedLocalizer : IViewExtendedLocalizer, IViewContextAware
    {
        private readonly IHtmlLocalizerFactory _htmlLocalizerFactory;
        private readonly IObjectLocalizerFactory _objectLocalizerFactory;
        private IHtmlLocalizer _htmlLocalizer;
        private IObjectLocalizer _objectLocalizer;
        private string _viewPrefix = String.Empty;

        public JsonFileViewExtendedLocalizer(IHtmlLocalizerFactory htmlLocalizerFactory, IObjectLocalizerFactory objectLocalizerFactory)
        {
            _htmlLocalizerFactory = htmlLocalizerFactory ?? throw new ArgumentNullException(nameof(htmlLocalizerFactory));
            _objectLocalizerFactory = objectLocalizerFactory ?? throw new ArgumentNullException(nameof(objectLocalizerFactory));
        }

        /// <inheritdoc />
        public LocalizedString GetString(string name)
        {
            return _htmlLocalizer.GetString(name);
        }

        /// <inheritdoc />
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return _htmlLocalizer.GetString(name, arguments);
        }

        /// <inheritdoc />
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _htmlLocalizer.GetAllStrings(includeParentCultures);
        }

        /// <inheritdoc />
        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            return _htmlLocalizer.WithCulture(culture);
        }

        /// <inheritdoc />
        public LocalizedHtmlString this[string name]
            => _htmlLocalizer[name];

        /// <inheritdoc />
        public LocalizedHtmlString this[string name, params object[] arguments]
            => _htmlLocalizer[name, arguments];

        /// <inheritdoc />
        public LocalizedObject<TValue> GetLocalizedObject<TValue>(string name)
        {
            return _objectLocalizer.GetLocalizedObject<TValue>(name);
        }

        /// <inheritdoc />
        IObjectLocalizer IObjectLocalizer.WithCulture(CultureInfo culture)
        {
            return _objectLocalizer.WithCulture(culture);
        }

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

            _htmlLocalizer = _htmlLocalizerFactory.Create(_viewPrefix, String.Empty);
            _objectLocalizer = _objectLocalizerFactory.Create(_viewPrefix, String.Empty);
        }

        private string GetPrefix(string viewPath)
        {
            var builder = new StringBuilder(viewPath);
            var extension = Path.GetExtension(viewPath);
            builder.Replace(extension, String.Empty);
            builder.Replace(Path.DirectorySeparatorChar, '.');
            builder.Replace(Path.AltDirectorySeparatorChar, '.');
            if (builder[0] == '.')
            {
                builder.Remove(0, 1);
            }
            return builder.ToString();
        }
    }
}