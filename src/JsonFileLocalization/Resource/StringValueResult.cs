using System;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// String resource from a file
    /// </summary>
    public readonly struct StringValueResult
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

        public StringValueResult(string path, string name, string value)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}