using JsonFileLocalization.Example.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.Example.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Something = _localizer["Something"];
            return View("Index");
        }

        [HttpGet]
        public RedirectToActionResult RedirectToDefaultLanguage()
        {
            return RedirectToAction("Index", new { lang = CurrentLanguage });
        }
    }
}
