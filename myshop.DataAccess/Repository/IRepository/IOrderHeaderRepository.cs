using myshop.Entities;
using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
    {
        
        void update(OrderHeader OrderHeader);
        void UpdateOrderStatus(int id, string orderstatus, string PaymentStatus=null);
    }
}
