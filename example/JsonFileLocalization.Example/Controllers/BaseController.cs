using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace JsonFileLocalization.Example.Controllers
{
    public abstract class BaseController : Controller
    {
        private string _currentLanguage;

        protected string CurrentLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }
        
        public RedirectToActionResult RedirectToDefaultLanguage()
        {
            return RedirectToAction("Index", new { lang = CurrentLanguage });
        }
    }
}