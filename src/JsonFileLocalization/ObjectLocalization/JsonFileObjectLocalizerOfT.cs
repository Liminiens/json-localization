using System.Globalization;

namespace JsonFileLocalization.ObjectLocalization
{
    /// <summary>
    /// A <see cref="JsonFileObjectLocalizer"/> for a type TResource
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class JsonFileObjectLocalizerOfT<TResource> : IObjectLocalizer
    {
        private readonly IObjectLocalizer _localizer;

        /// <summary>
        /// Creates a <see cref="JsonFileObjectLocalizerOfT{TResource}"/>
        /// </summary>
        /// <param name="factory">A factory of <see cref="JsonFileObjectLocalizer"/> objects</param>
        public JsonFileObjectLocalizerOfT(JsonFileObjectLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(TResource));
        }

        /// <inheritdoc />
        public LocalizedObject<TValue> GetLocalizedObject<TValue>(string name)
        {
            return _localizer.GetLocalizedObject<TValue>(name);
        }

        /// <inheritdoc />
        public IObjectLocalizer WithCulture(CultureInfo culture)
        {
            return _localizer.WithCulture(culture);
        }
    }
}