using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Repository;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities;
using myshop.Entities.Models;
using myshop.Entities.ViewModels;
using myshop.Utilities;
using Stripe.Checkout;
using System.Security.Claims;

namespace myshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public ShoppingCartVM ShoppingCartVM { get; set; } //property from vm
        public CartController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            //User el hy3mel shopping.....
            var IdentityClaim = (ClaimsIdentity)User.Identity;
            var UserId = IdentityClaim.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitofWork.shoppingcart.GetAll(x => x.ApplicationUserId == UserId, includeword: "product")
            };
            foreach (var item in ShoppingCartVM.CartsList) //hena lef gwa kol cartlist w agm3 Alltotal
            {
                ShoppingCartVM.TotalCarts += (item.Count * item.product.price);
            }
            return View(ShoppingCartVM);
        }
        public IActionResult Plus(int cartId)
        {
            var cartfromDb = _unitofWork.shoppingcart.GetFirstorDefault(u => u.Id == cartId);
            cartfromDb.Count += 1;
            _unitofWork.shoppingcart.update(cartfromDb);
            _unitofWork.save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Minus(int cartId)
        {
            var cartfromDb = _unitofWork.shoppingcart.GetFirstorDefault(u => u.Id == cartId);
            if (cartfromDb.Count <= 1)
            {
                //remove from shopping cart
                _unitofWork.shoppingcart.Remove(cartfromDb);
               
                //Set Session when Remove 
                HttpContext.Session.SetInt32(SD.SessionKey, _unitofWork.shoppingcart
                       .GetAll(u => u.ApplicationUserId == cartfromDb.ApplicationUserId).Count() - 1);
                _unitofWork.save();
                return RedirectToAction(nameof(Index),"Home");
            }
            else
            {
                cartfromDb.Count -= 1;
                _unitofWork.shoppingcart.update(cartfromDb);
                _unitofWork.save();
                //Set Session when Remove 
               // var count = _unitofWork.shoppingcart.GetAll(x => x.ApplicationUserId == cartfromDb.ApplicationUserId).ToList().Count() - 1;
               // HttpContext.Session.SetInt32(SD.SessionKey, count);

            }
            
            return RedirectToAction(nameof(Index));
            

        }

        public IActionResult Remove(int cartId)
        {
            var cartfromDb = _unitofWork.shoppingcart.GetFirstorDefault(u => u.Id == cartId);

            //remove from shopping cart
            _unitofWork.shoppingcart.Remove(cartfromDb);
            _unitofWork.save();
            //Set Session when Remove 
            var count = _unitofWork.shoppingcart.GetAll(x => x.ApplicationUserId == cartfromDb.ApplicationUserId).ToList().Count();
            HttpContext.Session.SetInt32(SD.SessionKey, count);
           
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Summary()
        {
            //User el hy3mel shopping.....
            var IdentityClaim = (ClaimsIdentity)User.Identity;
            var UserId = IdentityClaim.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitofWork.shoppingcart.GetAll(x => x.ApplicationUserId == UserId, includeword: "product"),
                OrderHeader = new()
            };
            //1- initialize Data from applicationuser to summary data 
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofWork.applicationuser.GetFirstorDefault(x=>x.Id==UserId);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.Street;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            foreach (var item in ShoppingCartVM.CartsList) //hena lef gwa kol cartlist w agm3 Alltotal
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.product.price);
            }
            return View(ShoppingCartVM);

        }


		[HttpPost]
        [ActionName("Summary")]
		[ValidateAntiForgeryToken]
		public IActionResult SummaryPost(ShoppingCartVM ShoppingCartVM)
        {
            //User el hy3mel shopping.....
            var IdentityClaim = (ClaimsIdentity)User.Identity;
            var UserId = IdentityClaim.FindFirst(ClaimTypes.NameIdentifier).Value;
           //Gab kol el product 5asa be user
            ShoppingCartVM.CartsList = _unitofWork.shoppingcart.GetAll(x => x.ApplicationUserId == UserId, includeword: "product");
			ShoppingCartVM.OrderHeader.OderDate = System.DateTime.Now;
		
			ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
			ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			ShoppingCartVM.OrderHeader.ApplicationUserId = UserId;
            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.product.price);
            }
				_unitofWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			_unitofWork.save();


			foreach (var cart in ShoppingCartVM.CartsList)
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId = cart.ProductId,
					OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
					Price = cart.product.price,
					Count = cart.Count

				};
				_unitofWork.OrderDetail.Add(orderDetail);
				_unitofWork.save();
			}
            var domain = "https://localhost:44379/";
			//hna b3ml configration le options 
			var options = new SessionCreateOptions
			{
				SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
				CancelUrl = domain + "Customer/Cart/Index",
				//line Item da ykon feha product details
				LineItems = new List<SessionLineItemOptions>(),
		
		 
				Mode = "payment",
				
			};
			//b3mel iterate le kol product 3shan a7ota in lineitem List.
			foreach (var item in ShoppingCartVM.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
					// price data is data used to create a new price object
					PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.product.price * 100), //$20.50===>2050
						Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                //Add for all data orderdetails....
                options.LineItems.Add(sessionlineoption);
		
            }     

			var service = new SessionService();
			Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;
           
            _unitofWork.save();

			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		
	

			//return RedirectToAction("Index","Home");

        }


		public IActionResult OrderConfirmation(int id)
		{
			OrderHeader orderHeader = _unitofWork.OrderHeader.GetFirstorDefault(u => u.Id == id, includeword: "ApplicationUser");
            //3wz check on payment  bt3ty at3mlt bnga7 wla la2
            //if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            //{
          
            var service = new SessionService();
            //when Get session hena a7na Get Session el at3melt
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                
                _unitofWork.OrderHeader.UpdateOrderStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                orderHeader.PaymentIntentId = session.PaymentIntentId;//hena order 7slo paied
                var shoppingCartItems = _unitofWork.shoppingcart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId, includeword: "product");

                foreach (var cartItem in shoppingCartItems)
                {
                    //hena 7ded store =1 azay a3melha dynamic
                    // var storeProduct = _unitofWork.StoreProduct.GetFirstorDefault(sp => sp.ProductId == cartItem.ProductId && sp.StoreId == 1, includeword: "Store");
                    var storeProduct = _unitofWork.StoreProduct.GetStorewithPrice(cartItem.ProductId, cartItem.Count);

                    if (storeProduct != null)
                    {
                      
                        storeProduct.Quantity_Stocks -= cartItem.Count;

                      
                        if (storeProduct.Quantity_Stocks < 0)
                        {
                            storeProduct.Quantity_Stocks = 0;
                        }
                    }
                }
                _unitofWork.save();
            }

            //}
            //hyro7 b3d kda tlama paid hyshlhalo men cart bta3o
            //list from shoppingcart el 5asa ll user da bs .....
            IEnumerable<ShoppingCart> shoppingcartRemove = _unitofWork.shoppingcart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitofWork.shoppingcart.RemoveRange(shoppingcartRemove);
            _unitofWork.save();
            return View(id);
		}
	}
}
