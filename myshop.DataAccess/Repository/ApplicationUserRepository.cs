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
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplictionDbContext _context;

        public ApplicationUserRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }

        

       

        
    }
}
