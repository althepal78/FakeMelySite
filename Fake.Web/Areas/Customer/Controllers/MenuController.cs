using Microsoft.AspNetCore.Mvc;

namespace Fake.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
