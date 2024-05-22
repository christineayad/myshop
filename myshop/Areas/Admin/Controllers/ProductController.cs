using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Entities;
using myshop.DataAccess.Repository.IRepository;


namespace myshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _webHost; //de bt3rf wwwroot
        public ProductController(IUnitofWork unitofWork, IWebHostEnvironment webHost)
        {
            _unitofWork = unitofWork;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            //var result = _unitofWork.product.GetAll().ToList();
            return View();
        }
            public IActionResult GetData()
            {
            var products=_unitofWork.product.GetAll(includeword:"Category").ToList();
            return Json( new { data = products });

            }
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> listcategory = _unitofWork.category.GetAll()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.categoryList = listcategory;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootpath = _webHost.WebRootPath; //da WWWRoot
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();//RandomNumber
                    var upload = Path.Combine(rootpath, @"Images\Product\");
                    var ext = Path.GetExtension(file.FileName); //.jpg
                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    product.Img = @"Images\Product\" + filename + ext;
                }

                _unitofWork.product.Add(product);
                _unitofWork.save();
                TempData["Success"] = "Item Has Created Succefully";
                return RedirectToAction("Index");

            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Product? productobj = _unitofWork.product.GetFirstorDefault(u => u.Id == id);
            if (productobj == null)
            {
                return NotFound();
            }
            IEnumerable<SelectListItem> CategoryList = _unitofWork.category.GetAll().Select(
           u => new SelectListItem
           {
               Text = u.Name,
               Value = u.Id.ToString()
           }
           );
            ViewBag.CategoryList = CategoryList;
            return View(productobj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product productdb, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string pathroot = _webHost.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string pathproduct = Path.Combine(pathroot, @"images\product\");

                    if (!string.IsNullOrEmpty(productdb.Img))
                    {
                        //delete old image
                        var oldImagepath = Path.Combine(pathroot, productdb.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagepath))
                        {
                            System.IO.File.Delete(oldImagepath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(pathproduct, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productdb.Img = @"images\product\" + fileName;
                }
                _unitofWork.product.update(productdb);
                _unitofWork.save();
                TempData["Success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
            }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0 | id == null)
                return NotFound();
            var ProductInDB = _unitofWork.product.GetFirstorDefault(x => x.Id == id);
            if (ProductInDB == null)
                return Json(new { success = false, message = "Error in Delete" });
            _unitofWork.product.Remove(ProductInDB);
            var oldImagepath = Path.Combine(_webHost.WebRootPath, ProductInDB.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagepath))
            {
                System.IO.File.Delete(oldImagepath);
            }
            _unitofWork.save();
            return Json(new { success = true, message = "item has been Deleted" });
        }
    }
}
