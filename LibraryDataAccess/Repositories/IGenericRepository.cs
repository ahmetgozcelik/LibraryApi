using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDataAccess.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region
        void Create(TEntity entity);
        Task CreateAsync(TEntity entity);

        IQueryable<TEntity> GetAll();

        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);

        void DeleteRange(List<TEntity> entities);
        Task DeleteRangeAsync(List<TEntity> entities);

        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id);

        IQueryable<TEntity> Queryable();
        #endregion
    }
}
