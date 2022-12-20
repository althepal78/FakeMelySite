using Fake.DataAccess;
using Fake.Models;
using Fake.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fake.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MenuController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var product = _db.Products.ToList();
            return View(product);
        }


        // get action   
        public IActionResult Upsert(Guid? id)
        {
            ProductVM ifInDb = new ProductVM();
            Product inDb = new Product();
            if (id != null)
            {
                inDb = _db.Products.FirstOrDefault(x => x.Id == id);
            }
            if (inDb == null)
            {
                return View();
            }
            ifInDb.Product.Add(inDb);
            return View(ifInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj)
        {

            foreach (var objIn in obj.Product)
            {

            }
           obj.Product = _db.Products.Where(f => f.Id == obj.Product.Id.Id);
            
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Model", "your model is not working ");
                return View(new {id = obj.Product.Id});
            }
            
            Product pro = _db.Product.GetFirstOrDefault(p => p.Id == obj.Product.Id);
            return RedirectToAction(nameof(Index),"Menu");
        }
    }
}
