using System;

namespace JsonFileLocalization.Resources
{
    /// <summary>
    /// String resource from a file
    /// </summary>
    public class StringValueResult
    {
        /// <summary>
        /// Path in a resource
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// Name of a string
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Value of a string
        /// </summary>
        public readonly string Value;

        public StringValueResult(in string path, in string name, in string value)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}