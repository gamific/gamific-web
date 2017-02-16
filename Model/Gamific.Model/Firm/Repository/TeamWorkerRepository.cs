using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class TeamWorkerRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile TeamWorkerRepository instance;

        private TeamWorkerRepository() { }

        public static TeamWorkerRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new TeamWorkerRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region TeamWorker

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<TeamWorkerEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.TeamWorkers
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Buscar todos ativos pela empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<TeamWorkerEntity> GetAllFromFirm(int firmId, int pageIndex = 0, int pageSize = 10)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.TeamWorkers
                        where sc.FirmId == firmId && sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Buscar todos ativos pela equipe
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<TeamWorkerEntity> GetAllFromTeam(int teamId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.TeamWorkers
                        where sc.TeamId == teamId && sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Buscar todos ativos pelo id
        /// </summary>
        /// <param name="teamWorkerId"></param>
        /// <returns></returns>
        public TeamWorkerEntity GetById(int teamWorkerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.TeamWorkers
                            where t.Id == teamWorkerId && t.Status == GenericStatus.ACTIVE
                            select t;

                return query.FirstOrDefault();
            }
        }

        public TeamWorkerEntity CreateTeamWorker(TeamWorkerEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.TeamWorkers.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        public TeamWorkerEntity UpdateTeamWorker(TeamWorkerEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.TeamWorkers.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
