using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IGenericReponsitory<Entities>
    {
        Task<Entities> GetByIdAsync(string id);

        IQueryable<Entities> SearchFor(Expression<Func<Entities, bool>> predicate);

        IQueryable<Entities> GetAll();

        Task EditAsync(Entities entities);
        Task InsertAsync(Entities entities);
        Task DeleteAsync(Entities entities);
        Task SaveChanges();

        void Dispose();

        IEnumerable<Entities> GetList();
    }
}
