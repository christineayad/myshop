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
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        private readonly ApplictionDbContext _context;
        public ProductRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Product product)
        {
            var productdb=_context.Products.FirstOrDefault(x=>x.Id==product.Id);
            if (productdb!=null)
            {
                productdb.Name = product.Name;
                productdb.Description=product.Description;
                productdb.price = product.price;
                productdb.CategoryId = product.CategoryId;
                productdb.Img=product.Img;
            }
        }
    }
}
