using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Data;
using myshop.Utilities;
using System.Security.Claims;

namespace myshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class UsersController : Controller
    {
        private readonly ApplictionDbContext _context;
        public UsersController(ApplictionDbContext context)
        {
            _context= context;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims= claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claims.Value;

            return View(_context.ApplicationUsers.Where(x => x.Id != userId));
        }
        public IActionResult LockUnlock(string ? Id)
        {
            var user=_context.ApplicationUsers.FirstOrDefault(x=>x.Id == Id);
            if (user == null)
                return NotFound();
            if(user.LockoutEnd==null |user.LockoutEnd<DateTime.Now) //open
            {
                user.LockoutEnd = DateTime.Now.AddYears(1); //close user until year
            }
            else
            {
                user.LockoutEnd = DateTime.Now;//open user b5ly date=now .
            }
            _context.SaveChanges(); //save n DB
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }
    }
}
