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
    public class OrderHeaderRepository : GenericRepository<OrderHeader>,IOrderHeaderRepository
    {
        private readonly ApplictionDbContext _context;

        public OrderHeaderRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }


        public void update(OrderHeader OrderHeader)
        {
            _context.OrderHeaders.Update(OrderHeader);
        }
        //update for status if pending ,process,canceled
        //update for payment if paied , not paied
        public void UpdateOrderStatus(int id, string orderstatus, string PaymentStatus=null)
        {
            var orderDB = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(orderDB!=null)
            {
                orderDB.OrderStatus = orderstatus;
                orderDB.PaymentDate = DateTime.Now; //hena bsgel tare5 daf3 bnfs tare5 order
                if (PaymentStatus != null) 
                {
                    orderDB.PaymentStatus = PaymentStatus;
                }
            }
           
        }
    }
}
