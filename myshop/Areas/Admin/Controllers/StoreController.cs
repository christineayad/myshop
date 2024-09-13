using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Repository.IRepository;

namespace myshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public StoreController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
           // var result = _unitofWork.Store.GetAll();

            return View();
        }
        public IActionResult GetData()
        {
            var stores = _unitofWork.Store.GetAll().ToList();
            return Json(new { data = stores });

        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Store store)
        {
            if (ModelState.IsValid)
            {
               

                _unitofWork.Store.Add(store);
                _unitofWork.save();
                TempData["Success"] = "Item Has Created Succefully";
                return RedirectToAction("Index");

            }

            return View(store);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Store? storeobj = _unitofWork.Store.GetFirstorDefault(u => u.Id == id);
            if (storeobj == null)
            {
                return NotFound();
            }
           
            return View(storeobj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Store storedb)
        {
            if (ModelState.IsValid)
            {
                
                _unitofWork.Store.update(storedb);
                _unitofWork.save();
                TempData["Success"] = "Store Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0 | id == null)
                return NotFound();
            var StoreInDB = _unitofWork.Store.GetFirstorDefault(x => x.Id == id);
            if (StoreInDB == null)
                return Json(new { success = false, message = "Error in Delete" });
            _unitofWork.Store.Remove(StoreInDB);
            
            _unitofWork.save();
            return Json(new { success = true, message = "item has been Deleted" });
        }
    }
}
