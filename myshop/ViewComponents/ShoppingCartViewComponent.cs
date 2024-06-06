using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Repository;
using myshop.DataAccess.Repository.IRepository;
using myshop.Utilities;
using System.Security.Claims;

namespace myshop.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitofWork _unitofWork;
        public ShoppingCartViewComponent(IUnitofWork unitofWork)
        {
            _unitofWork= unitofWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {

                if (HttpContext.Session.GetInt32(SD.SessionKey) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitofWork.shoppingcart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count());
                }

                return View(HttpContext.Session.GetInt32(SD.SessionKey));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
