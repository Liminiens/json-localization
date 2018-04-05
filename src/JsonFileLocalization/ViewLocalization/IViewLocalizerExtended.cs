using JsonFileLocalization.ObjectLocalization;
using Microsoft.AspNetCore.Mvc.Localization;

namespace JsonFileLocalization.ViewLocalization
{
    /// <summary>
    /// Extended <see cref="IViewLocalizer"/> service
    /// </summary>
    public interface IViewLocalizerExtended : IViewLocalizer, IObjectLocalizer
    {
    }
}