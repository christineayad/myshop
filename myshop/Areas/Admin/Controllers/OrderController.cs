using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Repository;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities.Models;
using myshop.Entities.ViewModels;
using myshop.Utilities;
using Stripe;

namespace myshop.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles=SD.AdminRole)]
    public class OrderController : Controller
	{
		
		private readonly IUnitofWork _unitofWork;
		[BindProperty]
		public OrderVM OrderVM { get; set; }
		public OrderController(IUnitofWork unitofWork)
		{
				_unitofWork= unitofWork;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetData()
		{
			IEnumerable<OrderHeader> orderHeaders = _unitofWork.OrderHeader.GetAll(includeword: "ApplicationUser").ToList();
			return Json(new { data = orderHeaders });
		}
        public IActionResult Details(int orderid)
        {
         
            OrderVM orderVM = new OrderVM()
			{
				orderHeader = _unitofWork.OrderHeader.GetFirstorDefault(x => x.Id == orderid, includeword: "ApplicationUser"),
				orderDetail = _unitofWork.OrderDetail.GetAll(x => x.OrderHeaderId == orderid, includeword: "Product")
			};

            return View(orderVM);
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetail()
		{
			var orderfromDB = _unitofWork.OrderHeader.GetFirstorDefault(x => x.Id ==OrderVM.orderHeader.Id);
           
            orderfromDB.Name = OrderVM.orderHeader.Name;
            orderfromDB.PhoneNumber = OrderVM.orderHeader.PhoneNumber;
            orderfromDB.StreetAddress = OrderVM.orderHeader.StreetAddress;
            orderfromDB.City = OrderVM.orderHeader.City;
            orderfromDB.State = OrderVM.orderHeader.State;
            orderfromDB.PostalCode = OrderVM.orderHeader.PostalCode;
            if (!string.IsNullOrEmpty(OrderVM.orderHeader.Carrier))
            {
                orderfromDB.Carrier = OrderVM.orderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.orderHeader.TrackingNumber))
            {
                orderfromDB.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            }
            _unitofWork.OrderHeader.update(orderfromDB);
            _unitofWork.save();
            TempData["Success"] = "order Details Updated Successful";
            return RedirectToAction(nameof(Details), new { orderid = orderfromDB.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StartProcessing()
        {
            _unitofWork.OrderHeader.UpdateOrderStatus(OrderVM.orderHeader.Id, SD.StatusInProcess);
            _unitofWork.save();
            TempData["Success"] = "order Details Updated Successful";
            return RedirectToAction(nameof(Details), new { orderid = OrderVM.orderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipOrder()
        {
            var orderfromDB = _unitofWork.OrderHeader.GetFirstorDefault(x => x.Id == OrderVM.orderHeader.Id);
            orderfromDB.Carrier = OrderVM.orderHeader.Carrier;
            orderfromDB.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            orderfromDB.OrderStatus = SD.StatusShipped;
            orderfromDB.ShippingDate = DateTime.Now;
            _unitofWork.OrderHeader.update(orderfromDB);
            _unitofWork.save();
            TempData["Success"] = "order Details Shipped Successful";
            return RedirectToAction(nameof(Details), new { orderid = OrderVM.orderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {

            var orderHeader = _unitofWork.OrderHeader.GetFirstorDefault(u => u.Id == OrderVM.orderHeader.Id);
            //payment but Not take order .... h3melo refund

            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitofWork.OrderHeader.UpdateOrderStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitofWork.OrderHeader.UpdateOrderStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitofWork.save();
            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(Details), new { orderid = OrderVM.orderHeader.Id });

        }
    }
    }
