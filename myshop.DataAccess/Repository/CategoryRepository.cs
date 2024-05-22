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
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        private readonly ApplictionDbContext _context;

        public CategoryRepository(ApplictionDbContext context) : base(context)
        {
            _context = context;
        }

        

        public void update(Category category)
        {
            var categorydb=_context.categories.FirstOrDefault(x=>x.Id== category.Id);
            if (categorydb != null)
            {
                categorydb.Name = category.Name;
                categorydb.Description = category.Description;
                categorydb.CreatedTime=DateTime.Now;
            }
        }

        
    }
}
