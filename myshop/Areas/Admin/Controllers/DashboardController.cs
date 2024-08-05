using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Repository.IRepository;
using myshop.Utilities;

namespace myshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles=SD.AdminRole)]

    public class DashboardController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public DashboardController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            ViewBag.Orders= _unitofWork.OrderHeader.GetAll().Count();
            ViewBag.ApprovedOrders = _unitofWork.OrderHeader.GetAll(x=>x.OrderStatus==SD.StatusApproved).Count();
            ViewBag.Users = _unitofWork.applicationuser.GetAll().Count();
            ViewBag.Products = _unitofWork.product.GetAll().Count();
            return View();
        }
    }
}
