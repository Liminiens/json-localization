using System;

namespace JsonFileLocalization.ObjectLocalization
{
    /// <summary>
    /// Service that creates <see cref="IObjectLocalizer"/> objects
    /// </summary>
    public interface IObjectLocalizerFactory
    {
        /// <summary>
        /// Create an instance of <see cref="IObjectLocalizer"/>
        /// </summary>
        /// <param name="baseName">The base name of the resource to load objects from.</param>
        /// <param name="location">The location to load resources from.</param>
        /// <returns>Instance of <see cref="IObjectLocalizer"/></returns>
        IObjectLocalizer Create(string baseName, string location);

        /// <summary>
        /// Create an instance of <see cref="IObjectLocalizer"/> using the <see cref="System.Reflection.Assembly" /> and
        /// <see cref="Type.FullName" /> of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns>Instance of <see cref="IObjectLocalizer"/></returns>
        IObjectLocalizer Create(Type resourceSource);
    }
}