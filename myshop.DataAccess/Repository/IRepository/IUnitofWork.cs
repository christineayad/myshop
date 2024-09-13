using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository.IRepository
{
    public interface IUnitofWork:IDisposable
    {
        public void save();
        public ICategoryRepository category { get;  } //property from category get
        public IProductRepository product { get; }
        public IShoppingCartRepository shoppingcart { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get; }
        public IApplicationUserRepository applicationuser { get; }
        public IstoreRepository Store { get; }
    }
}
