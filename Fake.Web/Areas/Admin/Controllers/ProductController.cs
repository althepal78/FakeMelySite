using Fake.DataAccess.Repository.IRepository;
using Fake.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Fake.Utilities;
using Org.BouncyCastle.Crypto.Macs;

namespace Fake.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWOrk, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWOrk = unitOfWOrk;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        // get action   
        public IActionResult Upsert(Guid? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWOrk.Category.GetAll().Select(i => new SelectListItem
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
                productVM.Product = _unitOfWOrk.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImgURL != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImgURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImgURL = @"\images\products\" + fileName + extension;
                }
                if (obj.Product.Id == Guid.Empty)
                {
                    _unitOfWOrk.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWOrk.Product.Update(obj.Product);
                }
                _unitOfWOrk.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index", "Product");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWOrk.Product.GetAll(includeProperties: "Category");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(Guid? id)
        {
            var obj = _unitOfWOrk.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Errror While Deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImgURL.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWOrk.Product.Remove(obj);
            _unitOfWOrk.Save();
            return Json(new { success = true, message = "Deleted Successful" });
        }
        #endregion
    }


}
