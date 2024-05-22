using myshop.DataAccess.Data;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities;
using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>,IOrderDetailRepository
    {
        private readonly ApplictionDbContext _context;

        public OrderDetailRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }


        public void update(OrderDetail OrderDetail)
        {
            _context.OrderDetails.Update(OrderDetail);
        }
     
    }
}
