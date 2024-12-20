using Microsoft.EntityFrameworkCore;
using tp6.Data;
namespace tp6.Repositories

{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        protected AppDbContext _db;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext dbcontext)
        {
            _db = dbcontext;
            _dbSet = _db.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            var t = _dbSet.ToList();
            return t;
        }

        public T GetById(int id)
        {
            var t = _dbSet.Find(id);
            return t;
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var t = _dbSet.Find(id);
            if (t != null)
            {
                _dbSet.Remove(t);
                _db.SaveChanges();
            }
        }



    }
}
