using myshop.Entities;
using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository:IGenericRepository<OrderDetail>
    {
        
        void update(OrderDetail orderDetail);
       
    }
}
