using myshop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository.IRepository
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        public void update(Product product);
    }
}
