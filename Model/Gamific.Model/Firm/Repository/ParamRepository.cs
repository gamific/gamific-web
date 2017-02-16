using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class ParamRepository
    {

        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile ParamRepository instance;

        private ParamRepository() { }

        public static ParamRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new ParamRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region CampaignParam

        /// <summary>
        /// Busca pelo id
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        public ParamEntity GetById(int paramId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from t in context.Params
                            where t.Id == paramId 
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// cria
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
       /* public ParamEntity CreateParam(ParamEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Params.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }*/

      

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<ParamEntity> GetAll(string gameId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from param in context.Params
                             where param.GameId == gameId
                             select param);//.Skip(pageSize * pageIndex).Take(pageSize);

                return query.ToList();
            }
        }

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public int GetCountFromGame(string gameId)
        {
            using (ModelContext context = new ModelContext())
            {
                int count = (from param in context.Params
                             where param.GameId == gameId
                             select param).Count();

                return count;
            }
        }

        /// <summary>
        /// Atualiza um topico de ajuda
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public ParamEntity UpdateParam(ParamEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Params.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        /// <summary>
        /// Salva um topico de ajuda na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public ParamEntity CreateParam(ParamEntity newEntity)
        {
            try
            {
                using (ModelContext context = new ModelContext())
                {
                    newEntity.LastUpdate = DateTime.UtcNow;
                    context.Params.Attach(newEntity);
                    context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newEntity;
        }

        public void DeleteById(int id)
        {
            using (ModelContext context = new ModelContext())
            {
                ParamEntity param = new ParamEntity { Id = id };
                context.Params.Attach(param);
                context.Params.Remove(param);
                try
                {
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

    }
}
