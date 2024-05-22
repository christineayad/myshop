using myshop.DataAccess.Data;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplictionDbContext _context;

        public ShoppingCartRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(ShoppingCart ShoppingCart)
        {
            _context.ShoppingCarts.Update(ShoppingCart);
        }
    }
}
