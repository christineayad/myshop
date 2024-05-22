using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myshop.DataAccess.Data;
using myshop.DataAccess.Repository.IRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository
{
    public class GenericRepository <T>: IGenericRepository<T> where T : class
    {
        private  DbSet<T> _dbset;
        private readonly ApplictionDbContext _context;
        public GenericRepository(ApplictionDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>(); //initialize table in constructor.
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter = null, string? includeword = null)
        {
            IQueryable<T> Query = _dbset;
            if(Filter!=null)
            {
                Query = Query.Where(Filter);
            }
            //_context.products.include(category,logos,subcategory).tolist();
            if (!string.IsNullOrEmpty(includeword))
            {
                foreach (var item in includeword.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(item);
                }
            }
            return Query.ToList();
        }

        public T GetFirstorDefault(Expression<Func<T, bool>>? Filter = null, string? includeword = null)
        {
            IQueryable<T> Quary = _dbset;
            if(Filter!=null) {
                Quary = Quary.Where(Filter);
            }  
            if(!string.IsNullOrEmpty(includeword))
            {
                foreach (var item in includeword.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    Quary = Quary.Include(item);
                }
            }
            return Quary.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _context.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);
        }
    }
}
