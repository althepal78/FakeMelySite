using Fake.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Fake.Utilities;
using Fake.Models;
using Fake.DataAccess;

namespace Fake.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ProductVM productVM = new ProductVM();
            List<Product> product = _db.Products.ToList();
            foreach (var prod in product)
            {
                productVM.Product.Add(prod);
            }
            return View(productVM);
        }

        // get action   
        public IActionResult Create(Guid? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == Guid.Empty)
            {
                // create the product
                return View(productVM);
            }
            else
            {
                // Update the product
                productVM.Product.Add(_db.Products.FirstOrDefault(u => u.Id == id));
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    foreach (var image in obj.Product)
                    {
                        if (image.ImgURL != null)
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, image.ImgURL.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }
                        image.ImgURL = @"\images\products\" + fileName + extension;
                        if (image.Id == Guid.Empty)
                        {
                            _db.Products.Add(image);
                        }
                        else
                        {
                            _db.Products.Update(image);
                        }
                    }

                }
                _db.SaveChanges();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index", "Product");
            }
            return View(obj);
        }
        /*
                #region API CALLS
                [HttpGet]
                 public IActionResult GetAll()
                 {
                     var productList = _db.Product.GetAll(includeProperties: "Category");
                     return Json(new { data = productList });
                 }

                 [HttpDelete]
                 public IActionResult Delete(Guid? id)
                 {
                     var obj = _db.Product.GetFirstOrDefault(u => u.Id == id);
                     if (obj == null)
                     {
                         return Json(new { success = false, message = "Errror While Deleting" });
                     }
                     var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImgURL.TrimStart('\\'));
                     if (System.IO.File.Exists(oldImagePath))
                     {
                         System.IO.File.Delete(oldImagePath);
                     }
                     _db.Product.Remove(obj);
                     _unitOfWOrk.Save();
                     return Json(new { success = true, message = "Deleted Successful" });
                 }
                #endregion
            }
            */

    }
}