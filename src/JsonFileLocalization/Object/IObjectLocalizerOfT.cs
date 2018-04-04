using System;
using System.Collections.Generic;
using System.Text;

namespace JsonFileLocalization.Object
{

    /// <summary>
    /// Represents a service that provides localized objects for <typeparam name="TResource"/>
    /// </summary>
    public interface IObjectLocalizer<TResource> : IObjectLocalizer
    {
    }
}
