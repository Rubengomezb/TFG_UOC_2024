using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Context;

namespace TFG_UOC_2024.DB.Repository.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllAsNoTracking();

        TEntity GetById(Guid id);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindAsNoTracking(Expression<Func<TEntity, bool>> predicate);

        bool Exists(Expression<Func<TEntity, bool>> predicate);

        void Create(TEntity entity, bool saveChanges = true);

        void Update(TEntity entity, bool saveChanges = true);

        void UpdateRange(IEnumerable<TEntity> entity, bool saveChanges = true);

        void Delete(TEntity entity, bool saveChanges = true);

        Task SaveChanges();

        ApplicationContext GetDbContext();
    }
}
