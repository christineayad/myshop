using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Repository.IRepository;

namespace myshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreProductController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public StoreProductController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetData()
        {
            var storeProducts = _unitofWork.StoreProduct.GetAll(includeword: "Product,Store").ToList();
            return Json(new { data = storeProducts });

        }
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> ListStore = _unitofWork.Store.GetAll()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.StoreList = ListStore;
            IEnumerable<SelectListItem> ListProduct = _unitofWork.product.GetAll()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ProductList = ListProduct;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoreProduct stproduct)
        {
            if (ModelState.IsValid)
            {


                _unitofWork.StoreProduct.Add(stproduct);
                _unitofWork.save();
                TempData["Success"] = "Item Has Created Succefully";
                return RedirectToAction("Index");

            }

            return View(stproduct);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            StoreProduct? stproductobj = _unitofWork.StoreProduct.GetFirstorDefault(u => u.Id == id);
            if (stproductobj == null)
            {
                return NotFound();
            }
            IEnumerable<SelectListItem> ListStore = _unitofWork.Store.GetAll()
                 .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.StoreList = ListStore;
            IEnumerable<SelectListItem> ListProduct = _unitofWork.product.GetAll()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ProductList = ListProduct;
            return View(stproductobj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StoreProduct stproductdb)
        {
            if (ModelState.IsValid)
            {
              
                _unitofWork.StoreProduct.update(stproductdb);
                _unitofWork.save();
                TempData["Success"] = "StoreProduct Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0 | id == null)
                return NotFound();
            var storeProductInDB = _unitofWork.StoreProduct.GetFirstorDefault(x => x.Id == id);
            if (storeProductInDB == null)
                return Json(new { success = false, message = "Error in Delete" });
            _unitofWork.StoreProduct.Remove(storeProductInDB);
           
            _unitofWork.save();
            return Json(new { success = true, message = "item has been Deleted" });
        }
        [HttpGet]
        public IActionResult Transform()
        {
            IEnumerable<SelectListItem> ListStore = _unitofWork.Store.GetAll(x=>x.IsMain==true)
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.StoreList = ListStore;

            IEnumerable<SelectListItem> ListStore2 = _unitofWork.Store.GetAll(x => x.IsMain == false)
              .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.StoreList2 = ListStore2;

            IEnumerable<SelectListItem> ListProduct = _unitofWork.product.GetAll()
            .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ProductList = ListProduct;





            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transform(StoreProduct stproduct)
        {
            //if (stproduct.Store.IsMain == true && stproduct.Quantity_Stocks <= 0)
            //{
                var quantityMain = _unitofWork.StoreProduct.GetAll(s => s.ProductId == stproduct.ProductId && s.Store.IsMain == true).FirstOrDefault();
                var quantity=_unitofWork.StoreProduct.GetAll(s => s.ProductId == stproduct.ProductId && s.Store.IsMain==false ).FirstOrDefault();
               quantityMain.Quantity_Stocks += quantity.Quantity_Stocks;
                quantity.Quantity_Stocks = 0;
            _unitofWork.save();
          return RedirectToAction("Index");
           // returnView(stproduct);
            //}
            //else
            //{ return View(); }

        }
    }
}
