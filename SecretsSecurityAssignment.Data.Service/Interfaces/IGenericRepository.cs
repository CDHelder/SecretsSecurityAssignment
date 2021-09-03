using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Core.Data.Service
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        TEntity Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        TEntity GetById(int id);
        List<TEntity> GetById(int[] ids);
        void Create(TEntity obj);
        void Create(List<TEntity> objs);
        void Update(TEntity obj);
        void Update(List<TEntity> objs);
        void Delete(int id);
        void Delete(TEntity entity);
        void Delete(List<TEntity> entities);
    }
}
