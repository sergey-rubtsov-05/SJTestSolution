using System.Linq;
using DataModel.Enities;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IQueryable<T> Query<T>() where T : Entity;

        void Add<T>(T entity) where T : Entity;
        void SaveChanges();
    }
}
