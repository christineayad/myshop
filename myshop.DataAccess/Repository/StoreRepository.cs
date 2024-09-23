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
    public class StoreRepository : GenericRepository<Store>, IstoreRepository
    {
        private readonly ApplictionDbContext _context;
        public StoreRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Store store)
        {
            var storedb=_context.Stores.FirstOrDefault(x=>x.Id== store.Id);
            if(storedb!=null)
            {
                storedb.Name = store.Name;
                storedb.Address = store.Address;
                storedb.PhoneNumber = store.PhoneNumber;
                storedb.storekeeper = store.storekeeper;
                storedb.Minum_order = store.Minum_order;
                storedb.IsMain = store.IsMain;
            }
        }
    }
}
