using System;

namespace JsonFileLocalization.ObjectLocalization
{
    /// <summary>
    /// Localized object value from a resource
    /// </summary>
    /// <typeparam name="TValue">Object type</typeparam>
    public class LocalizedObject<TValue>
    {
        /// <summary>
        /// Creates a new <see cref="LocalizedObject{TValue}" />.
        /// </summary>
        /// <param name="name">The name of the object in the resource it was loaded from.</param>
        /// <param name="value">The actual object.</param>
        public LocalizedObject(string name, TValue value)
            : this(name, value, false)
        {
        }

        /// <summary>
        /// Creates a new <see cref="LocalizedObject{TValue}" />.
        /// </summary>
        /// <param name="name">The name of the object in the resource it was loaded from.</param>
        /// <param name="value">The actual object.</param>
        /// <param name="resourceNotFound">Whether the string was not found in a resource. Set this to <c>true</c> to indicate an alternate string value was used.</param>
        public LocalizedObject(string name, TValue value, bool resourceNotFound)
            : this(name, value, resourceNotFound, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="LocalizedObject{TValue}" />.
        /// </summary>
        /// <param name="name">The name of the object in the resource it was loaded from.</param>
        /// <param name="value">The actual object.</param>
        /// <param name="resourceNotFound">Whether the string was not found in a resource. Set this to <c>true</c> to indicate an alternate string value was used.</param>
        /// <param name="searchedLocation">The location which was searched for a localization value.</param>
        public LocalizedObject(string name, TValue value, bool resourceNotFound, string searchedLocation)
        {
            if (!typeof(TValue).IsValueType && value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
            ResourceNotFound = resourceNotFound;
            SearchedLocation = searchedLocation;
        }

        public static implicit operator TValue(LocalizedObject<TValue> localizedObject)
        {
            if (!typeof(TValue).IsValueType && localizedObject == null)
            {
                return default;
            }
            return localizedObject.Value;
        }

        /// <summary>
        /// The JPath of the object in the resource it was loaded from.
        /// </summary>
        public string Name { get; }

        /// <summary>The actual object.</summary>
        public TValue Value { get; }

        /// <summary>
        /// Whether the object was not found in a resource. If <c>true</c>, an alternate string value was used.
        /// </summary>
        public bool ResourceNotFound { get; }

        /// <summary>
        /// The location which was searched for a localization value.
        /// </summary>
        public string SearchedLocation { get; }

        /// <summary>Returns value of Value.ToString().</summary>
        /// <returns>The actual value of Value.ToString().</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}