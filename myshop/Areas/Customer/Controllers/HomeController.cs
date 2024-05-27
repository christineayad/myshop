using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Repository;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities;
using myshop.Utilities;
using System.Security.Claims;
using X.PagedList;

namespace myshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public HomeController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index(int? page)
        {
            var PageNumber = page ?? 1;
            int PageSize = 6; //number product that appeard in page
            var products = _unitofWork.product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(products);
        }
        public IActionResult Details(int Id)
        {
            
            ShoppingCart obj = new ShoppingCart() // de 3mlnha 3shan productdetails
            {
                ProductId = Id,
               product= _unitofWork.product.GetFirstorDefault(x => x.Id == Id, includeword: "Category"),
                Count = 1
            };
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart cart)
        {
            cart.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.ApplicationUserId = userId;
            // da 3shan lw ana dost nfs product w count different yro7 update msh add new coulumn
            ShoppingCart cartFromDb = _unitofWork.shoppingcart.GetFirstorDefault(u => u.ApplicationUserId == userId &&
            u.ProductId == cart.ProductId);

            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += cart.Count;
                _unitofWork.shoppingcart.update(cartFromDb);
                _unitofWork.save();
            }
            else
            {
                //add cart record
                _unitofWork.shoppingcart.Add(cart);
                
                HttpContext.Session.SetInt32(SD.SessionKey,
                _unitofWork.shoppingcart.GetAll(u => u.ApplicationUserId == userId).Count());
                _unitofWork.save();
            }
            TempData["success"] = "Cart Added successfully";
            return RedirectToAction("Index");
        }
    }
}
