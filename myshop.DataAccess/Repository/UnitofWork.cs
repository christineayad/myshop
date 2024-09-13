using myshop.DataAccess.Data;
using myshop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository
{
    public class UnitofWork : IUnitofWork
    {
        public ICategoryRepository category { get; private set; }

        public IProductRepository product { get; private set; }

        public IShoppingCartRepository shoppingcart { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public IApplicationUserRepository applicationuser { get; private set; }
        public IstoreRepository Store { get; private set; }

        private readonly ApplictionDbContext _context;
        public UnitofWork(ApplictionDbContext context)
        {
            _context = context;
            category = new CategoryRepository(context);
            product = new ProductRepository(context);
            shoppingcart = new ShoppingCartRepository(context);
            OrderHeader= new OrderHeaderRepository(context);
            OrderDetail=new OrderDetailRepository(context);
            applicationuser = new ApplicationUserRepository(context);
            Store = new StoreRepository(context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void save()
        {
            _context.SaveChanges();
        }
    }
}
