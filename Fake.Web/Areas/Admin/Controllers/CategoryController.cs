using Fake.DataAccess;
using Fake.DataAccess.Repository.IRepository;
using Fake.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fake.Utilities;

namespace Fake.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;

        public CategoryController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWOrk = unitOfWOrk;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWOrk.Category.GetAll();
            return View(objCategoryList);
        }

        // get action
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The diplay order can't be the same as the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWOrk.Category.Add(obj);
                _unitOfWOrk.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index", "Category");
            }
            return View(obj);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }
            var categoryInDb = _unitOfWOrk.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryInDb == null)
            {
                return NotFound();
            }
            return View(categoryInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The diplay order can't be the same as the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWOrk.Category.Update(obj);
                _unitOfWOrk.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index", "Category");
            }
            return View(obj);
        }

        public IActionResult ViewInfo(Guid? id)
        {
            var categoryInDb = _unitOfWOrk.Category.GetFirstOrDefault(u => u.Id == id);
            return View(categoryInDb);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }
            var categoryInDb = _unitOfWOrk.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryInDb == null)
            {
                return NotFound();
            }
            return View(categoryInDb);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(Guid? id)
        {
            var categoryInDb = _unitOfWOrk.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryInDb == null)
            {
                return NotFound();
            }
            _unitOfWOrk.Category.Remove(categoryInDb);
            _unitOfWOrk.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
