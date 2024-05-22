using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Repository.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        //_context.Categories.tolist();
        //_context.categories.where(x=>x.Id==id).ToList();//expression
        //_context.categories.include("Product").tolist();//includeword
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? Filter=null, string? includeword=null);
        T GetFirstorDefault(Expression<Func<T,bool>>? Filter=null,string? includeword=null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
