using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository.IRepository
{
    public interface IstoreProductRepository:IGenericRepository<StoreProduct>
    {
        void update(StoreProduct stproduct);
        StoreProduct GetStore(int productId, int quantity);
    }
}
