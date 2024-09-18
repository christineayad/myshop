using myshop.DataAccess.Data;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository
{
    public class StoreProductRepository : GenericRepository<StoreProduct>, IstoreProductRepository
    {
        private readonly ApplictionDbContext _context;
        public StoreProductRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }

        public StoreProduct GetStorewithPrice(int productId, int quantity)
        {
            
           var stocks= _context.StoreProducts.Where(s => s.ProductId == productId && s.Quantity_Stocks >= quantity).OrderBy(s=>s.PriceProduct).ThenByDescending(s=>s.Quantity_Stocks).ThenBy(_=>Guid.NewGuid()).ToList();
            
            var stock = stocks.FirstOrDefault();
            return stock;


        }

        public void update(StoreProduct stproduct)
        {
            var stproductdb=_context.StoreProducts.FirstOrDefault(x=>x.Id== stproduct.Id);
            if(stproductdb!=null)
            {
                stproductdb.Quantity_Stocks = stproduct.Quantity_Stocks;
                stproductdb.PriceProduct = stproduct.PriceProduct;
                stproductdb.ProductId = stproduct.ProductId;
                stproductdb.StoreId=stproduct.StoreId;
                

            }
        }
    }
}
