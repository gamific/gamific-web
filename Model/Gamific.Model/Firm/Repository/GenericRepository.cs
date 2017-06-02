using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public abstract class GenericRepository<TEntity> : IDisposable,
       IGenericRepository<TEntity> where TEntity : class, GenericEntity
    {
        ModelContext ctx = new ModelContext();
        public IQueryable<TEntity> GetAll()
        {
            return ctx.Set<TEntity>();
        }
        public int GetAllCount()
        {
            return ctx.Set<TEntity>().Count();
        }

        public IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return GetAll().Where(predicate).AsQueryable();
        }

        public TEntity GetById(int id)
        {
            return GetAll().Where(x => x.Id == id).First();
        }

        public TEntity Find(params object[] key)
        {
            return ctx.Set<TEntity>().Find(key);
        }

        public void update(TEntity entity)
        {
           try
            { 

            using (ModelContext context = new ModelContext())
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            
            }
            catch (Exception dbEx)
            {
                
                throw dbEx;
            }
        }

        public void SalvarTodos()
        {
            ctx.SaveChanges();
        }

        public void save(TEntity obj)
        {
            ctx.Set<TEntity>().Add(obj);
            try
            {
                ctx.SaveChanges();

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void delete(Func<TEntity, bool> predicate)
        {
            try
            {
                ctx.Set<TEntity>()
                    .Where(predicate).ToList()
                    .ForEach(del => ctx.Set<TEntity>().Remove(del));
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            ctx.Dispose();
        }

        public void saveAll()
        {
            throw new NotImplementedException();
        }

        //public IQueryable<TEntity> GetAllFromFirm(IQueryable<TEntity> source, string propertyName, string value)
        //{

        //    Expression<Func<TEntity, bool>> whereExpression = x => x.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, x, null).ToString().IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;

        //    return source.Where(whereExpression);
        //}


        //public int GetCountFromFirm(IQueryable<TEntity> source, string propertyName, string value) 
        //{

        //    Expression<Func<TEntity, bool>> whereExpression = x => x.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, x, null).ToString().IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;

        //    return source.Where(whereExpression).Count();

        //}


    }
}
