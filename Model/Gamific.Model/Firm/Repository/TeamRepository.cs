using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Properties;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class TeamRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile TeamRepository instance;

        private TeamRepository() { }

        public static TeamRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new TeamRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Team

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<TeamEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Teams
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /*
        /// <summary>
        /// Busca todas equipes ativas de um funcionario
        /// </summary>
        /// <param name="workerId"></param>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<TeamEntity> GetAllFromWorker(int workerId, int firmId)
        {
            List<TeamEntity> list = new List<TeamEntity>();
            using (ModelContext context = new ModelContext())
            {
                var query = Resources.SQL_TEAMS_FROM_WORKER;
                query = string.Format(query, workerId, firmId);

                list = context.Database.SqlQuery<TeamEntity>(query).ToList();
            }
            return list;
        }
        */

        /// <summary>
        /// Busca todas equipes ativas de um funcionario
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public List<TeamEntity> GetAllFromLeader(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from team in context.Teams
                            where team.Status == GenericStatus.ACTIVE
                            && team.SponsorId == workerId
                            select team;

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todas equipes ativas de um jogador
        /// </summary>
        /// <param name="workerId"></param>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<TeamEntity> GetAllFromWorker(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from team in context.Teams
                            from tw in context.TeamWorkers
                            where team.Status == GenericStatus.ACTIVE
                            && tw.Status == GenericStatus.ACTIVE
                            && tw.WorkerId == workerId
                            && tw.TeamId == team.Id
                            select team;

                return query.ToList();
            }
        }


        /// <summary>
        /// Busca todas equipes ativas de um jogador
        /// </summary>
        ///<param name="teamId"></param>
        /// <returns></returns>
        public string GetSponsorIdByTeamId(string teamId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =  from st in context.SponsorTeams
                                where st.TeamId == teamId
                                select st.SponsorId;

                return query.FirstOrDefault();
            }
        }



        /// <summary>
        /// Busca a equipe em que o funcionario participa
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public TeamEntity GetFromWorker(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from tw in context.TeamWorkers
                            from team in context.Teams
                            where tw.Status == GenericStatus.ACTIVE
                            && team.Status == GenericStatus.ACTIVE
                            && tw.WorkerId == workerId
                            && tw.TeamId == team.Id
                            select team;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca todas equipes ativas de um tipo de funcionario
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public List<TeamEntity> GetAllFromWorkerType(int workerTypeId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from team in context.Teams
                            where team.Status == GenericStatus.ACTIVE
                            && team.WorkerTypeId == workerTypeId
                            select team;

                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera a equipe pelo id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public TeamEntity GetById(string teamId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Teams
                            where t.Status == GenericStatus.ACTIVE && t.ExternalId == teamId
                            select t;

                return query.FirstOrDefault();
            }
        }

        public TeamEntity GetByIdExterno(string idExterno)
        {
            using (ModelContext context = new ModelContext())
            {

                var query =
                            from t in context.Teams
                            where t.Status == GenericStatus.ACTIVE && t.ExternalId == idExterno
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca os DTOs pela lista de Ids
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="teamIdList"></param>
        /// <returns></returns>
        public List<TeamDTO> GetDTOsByIds(int firmId, string teamIdList)
        {
            List<TeamDTO> result = new List<TeamDTO>();
            using (ModelContext context = new ModelContext())
            {
                var query = Resources.SQL_TEAMS_DTOS_BY_TEAMS_ID;
                query = string.Format(query, firmId, teamIdList);

                result = context.Database.SqlQuery<TeamDTO>(query).ToList();
            }
            return result;
        }

        /// <summary>
        /// Recupera a equipe pelo responsavel
        /// </summary>
        /// <param name="sponsorId"></param>
        /// <returns></returns>
        public List<TeamEntity> GetBySponsor(int sponsorId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Teams
                            where t.Status == GenericStatus.ACTIVE && t.SponsorId == sponsorId
                            select t;

                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera o numero de jogadores que uma equipe possui
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public int GetNumberOfMembers(int teamId)
        {
            using (ModelContext context = new ModelContext())
            {
                var numberOfMembers = (from tw in context.TeamWorkers
                                       where tw.Status == GenericStatus.ACTIVE
                                       && tw.TeamId == teamId
                                       select tw).Count();

                return numberOfMembers;
            }
        }

        /// <summary>
        /// Recupera o numero de jogadores que a equipe ainda pode associar
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public int GetNumberOfMembersToAssociate(int teamId)
        {
            using (ModelContext context = new ModelContext())
            {
                var exceptionList = from worker in context.Workers
                                    from tw in context.TeamWorkers
                                    from team in context.Teams
                                    where worker.Status == GenericStatus.ACTIVE
                                    && tw.Status == GenericStatus.ACTIVE
                                    && worker.Id == tw.WorkerId
                                    && team.WorkerTypeId == worker.WorkerTypeId
                                    && team.Id == teamId
                                    select worker.Id;

                int count = (from team in context.Teams
                             from worker in context.Workers
                             where team.Status == GenericStatus.ACTIVE
                             && worker.Status == GenericStatus.ACTIVE
                             && !exceptionList.Contains(worker.Id)
                             && team.WorkerTypeId == worker.WorkerTypeId
                             && team.Id == teamId
                             select worker).Count();

                return count;
            }
        }

        /// <summary>
        /// Recupera o numero de jogadores que uma equipe possui
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public int GetNumberOfTeamOfWorkerType(int workerTypeId)
        {
            using (ModelContext context = new ModelContext())
            {
                var numberOfteams = (from tw in context.Teams
                                     where tw.Status == GenericStatus.ACTIVE
                                     && tw.WorkerTypeId == workerTypeId
                                     select tw).Count();

                return numberOfteams;
            }
        }


        /// <summary>
        /// Recupera a equipe pelo responsavel
        /// </summary>
        /// <param name="sponsorId"></param>
        /// <returns></returns>
        public TeamEntity GetBySponsorId(int sponsorId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Teams
                            where t.Status == GenericStatus.ACTIVE
                            && t.SponsorId == sponsorId
                            select t;

                return query.FirstOrDefault();
            }
        }



        /// <summary>
        /// Recupera o DTO da equipe pelo responsavel
        /// </summary>
        /// <param name="sponsorId"></param>
        /// <returns></returns>
        public List<TeamDTO> GetDTOBySponsor(int sponsorId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Teams
                            where t.Status == GenericStatus.ACTIVE && t.SponsorId == sponsorId
                            select new TeamDTO { IdTeam = t.Id, TeamName = t.TeamName };

                return query.ToList();
            }
        }

        /// <summary>
        /// Salva uma associaçao de responsavel e equipe no banco
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public SponsorTeamEntity AssociateSponsorToTeamById(SponsorTeamEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                context.SponsorTeams.Add(newEntity);
                context.SaveChanges();

                return newEntity;
            }   
        }

        /// <summary>
        /// Salva uma equipe na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public TeamEntity CreateTeam(TeamEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Teams.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza uma equipe
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public TeamEntity UpdateTeam(TeamEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Teams.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        /// <summary>
        /// Busca todas equipes ativas de uma empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<TeamDTO> GetAllFromFirm(int firmId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from team in context.Teams
                             from worker in context.Workers
                             from wt in context.WorkerTypes
                             from p in context.Profiles
                             where team.Status == GenericStatus.ACTIVE
                             && worker.Status == GenericStatus.ACTIVE
                             && wt.Status == GenericStatus.ACTIVE
                             && team.SponsorId == worker.Id
                             && team.WorkerTypeId == wt.Id
                             && worker.UserId == p.Id
                             && team.FirmId == firmId
                             select new TeamDTO
                             {
                                 TeamName = team.TeamName,
                                 IdTeam = team.Id,
                                 LogoId = team.LogoId,
                                 ProfileName = wt.TypeName,
                                 SponsorId = team.SponsorId,
                                 SponsorName = p.Name,
                             }
                            ).OrderBy(x => x.TeamName).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todas equipes ativas de uma empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public int GetCountFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                int count = (from team in context.Teams
                             where team.Status == GenericStatus.ACTIVE
                             && team.FirmId == firmId
                             select team).Count();

                return count;
            }
        }

        /// <summary>
        /// Busca todos os responsaveis de times na lista recebida
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public List<SponsorTeamEntity> GetAllSponsorTeamByListTeamId(List<string> teamIds)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from st in context.SponsorTeams
                            where teamIds.Contains(st.TeamId)
                            select st;

                return query.ToList();
            }
        }


        /// <summary>
        /// Busca todas equipes ativas de uma empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<TeamEntity> GetAllFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from team in context.Teams
                            where team.Status == GenericStatus.ACTIVE
                            && team.FirmId == firmId
                            select team;

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todas equipes ativas de uma empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<TeamEntity> GetAllEntityFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from team in context.Teams
                            where team.Status == GenericStatus.ACTIVE
                            && team.FirmId == firmId
                            select team;

                return query.ToList();
            }
        }

        #endregion

    }
}
