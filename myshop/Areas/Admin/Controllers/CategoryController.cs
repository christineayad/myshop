using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Data;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities;

namespace myshop.Areas.Admin.Controllers

{
    [Area("Admin")]

    public class CategoryController : Controller

    {
        private readonly IUnitofWork _unitofWork;
        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {

            var result = _unitofWork.category.GetAll();
            return View(result);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.categories.Add(category);
                _unitofWork.category.Add(category);
                //_context.SaveChanges();
                _unitofWork.save();
                TempData["Success"] = "Item Has Created Succefully";
                return RedirectToAction("Index");
            }
            return View(category);

        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            //_context.categories.Find(id);
            var CategoryId = _unitofWork.category.GetFirstorDefault(x => x.Id == id);
            return View(CategoryId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // _context.categories.Update(category);
                _unitofWork.category.update(category);
                // _context.SaveChanges();
                _unitofWork.save();
                TempData["Success"] = "Item Has Updated Succefully";
                return RedirectToAction("Index");
            }
            return View(category);

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var CategoryId = _unitofWork.category.GetFirstorDefault(x => x.Id == id);
            return View(CategoryId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var CategoryId = _unitofWork.category.GetFirstorDefault(x => x.Id == id);
            if (CategoryId == null)
            {
                return NotFound();

            }
            //_context.categories.Remove(CategoryId);
            _unitofWork.category.Remove(CategoryId);
            // _context.SaveChanges();
            _unitofWork.save();
            TempData["Delete"] = "Item Has Deleted Succefully";
            return RedirectToAction("Index");
            //return View(category);

        }

    }
}
