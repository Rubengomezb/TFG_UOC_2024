using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.DB.Repository
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
    {
        protected ApplicationContext DbContext { get; private set; }

        public EntityRepository(ApplicationContext context) 
        {  
            this.DbContext = context; 
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>(); // We are using AsNoTracking extension to make things faster and prevent any updates to this specific IQueryable collection
        }
        public IQueryable<TEntity> GetAllAsNoTracking()
        {
            return DbContext.Set<TEntity>().AsNoTracking(); // We are using AsNoTracking extension to make things faster and prevent any updates to this specific IQueryable collection
        }
        public TEntity GetById(Guid id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate);
        }

        public IEnumerable<TEntity> FindAsNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().AsNoTracking().Where(predicate);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            lock (DbContext)
            {
                return DbContext.Set<TEntity>().Any(predicate);
            }
        }

        public void Create(TEntity entity, bool saveChanges = true)
        {
            lock (DbContext)
            {
                DbContext.Add(entity);
                if (saveChanges)
                {
                    DbContext.SaveChanges();
                }
            }
        }

        public virtual void Update(TEntity entity, bool saveChanges = true)
        {
            lock (DbContext)
            {
                DbContext.Set<TEntity>().Update(entity);
                if (saveChanges)
                {
                    lock (DbContext)
                    {
                        DbContext.SaveChanges();

                        //Update entities without tracking, if you need update the same entity in multiple times
                        DbContext.Entry(entity).State = EntityState.Detached;
                    }
                }
            }
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entity, bool saveChanges = true)
        {
            lock (DbContext)
            {
                DbContext.Set<TEntity>().UpdateRange(entity);
                if (saveChanges)
                {
                    lock (DbContext)
                    {
                        DbContext.SaveChanges();
                    }
                }
            }
        }

        public async void Delete(TEntity entity, bool saveChanges = true)
        {
            DbContext.Set<TEntity>().Remove(entity);
            if (saveChanges)
            {
                DbContext.SaveChanges();
            }
        }

        public async Task SaveChanges()
        {
            await DbContext.SaveChangesAsync();
        }

        public ApplicationContext GetDbContext()
        {
            return DbContext;
        }
    }
}
