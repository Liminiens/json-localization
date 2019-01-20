using System;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// String resource from a file
    /// </summary>
    public class StringFromResource
    {
        /// <summary>
        /// Path in a resource
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// Value of a string
        /// </summary>
        public readonly string Value;

        public StringFromResource(string path, string value)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}