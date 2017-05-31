using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Func<TEntity, bool> predicate);
        TEntity Find(params object[] key);
        void saveAll();
        void save(TEntity obj);
        void delete(Func<TEntity, bool> predicate);
        void update(TEntity entity);
        //IQueryable<TEntity> GetAllFromFirm(IQueryable<TEntity> source, string propertyName, string value);
        //int GetCountFromFirm(IQueryable<TEntity> source, string propertyName, string value);


    }
}
