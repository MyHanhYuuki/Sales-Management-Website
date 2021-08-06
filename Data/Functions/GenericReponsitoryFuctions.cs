using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Interfaces;
using System.Collections.Generic;

namespace Data.Functions
{
    public class GenericReponsitoryFuctions<Entities> : IGenericReponsitory<Entities> where Entities : class
    {
        protected DbSet<Entities> DbSet;

        protected readonly DbContext _dbContext;

        //Khởi tạo data context kế thừa từ Interfaces
        public GenericReponsitoryFuctions(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<Entities>();
        }

        //Dùng để thực hiện truy vấn trực tiếp trong sql
        public IQueryable<Entities> GetAll()
        {
            return DbSet;
        }

        //Dùng để thao tác với các collection
        public IEnumerable<Entities> GetList()
        {
            return DbSet;
        }

        //Định nghĩa phương thức tìm kiếm bằng id
        public async Task<Entities> GetByIdAsync(String id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<Entities> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<Entities> GetByIdAsync(String id, int id2)
        {
            return await DbSet.FindAsync(id, id2);
        }

        //Truy vấn tìm kiếm bằng linq với đối số truyền vào là một biểu thức linq
        public IQueryable<Entities> SearchFor(Expression<Func<Entities, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public async Task DeleteAsync(Entities entities)
        {
            DbSet.Remove(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(Entities entities)
        {
            _dbContext.Entry(entities).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertAsync(Entities entities)
        {
            DbSet.Add(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public GenericReponsitoryFuctions()
        {

        }


        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
            _dbContext.Dispose();
        }
        public List<Entities> Fetch()
        {
                List<Entities> result = _dbContext.Set<Entities>().ToList();

                return result;
        }
        public List<Entities> Fetch(Expression<Func<Entities, bool>> query)
        {
                List<Entities> result = new List<Entities>();

                if (query != null)
                {
                    result = _dbContext.Set<Entities>().Where(query).ToList();
                }
                else
                {
                    result = _dbContext.Set<Entities>().ToList();
                }

                return result;
        }
        public Entities GetById(object id)
        {
                return _dbContext.Set<Entities>().Find(id);
        }
    }
}
