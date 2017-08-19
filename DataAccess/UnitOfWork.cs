using System.Linq;
using DataModel.Enities;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SjContext _context;

        public UnitOfWork(SjContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query<T>() where T : Entity
        {
            return _context.Set<T>();
        }

        public void Add<T>(T entity) where T : Entity
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges(); //TODO what with transactions
        }
    }
}