using System.Globalization;

namespace JsonFileLocalization.Object
{
    /// <summary>
    /// Service for localizing objects from resources in <see cref="Resources.JsonFileResource"/>
    /// </summary>
    public class JsonFileObjectLocalizer : IObjectLocalizer
    {
        /// <inheritdoc />
        public LocalizedObject<TValue> GetLocalizedObject<TValue>(string name)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IObjectLocalizer WithCulture(CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}