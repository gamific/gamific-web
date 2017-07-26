using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Properties;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;
using System.Globalization;

namespace Vlast.Gamific.Model.Firm.Repository
{

    /// <summary>
    /// Consulta de dados relacionados com o funcionario
    /// </summary>
    public class WorkerRepository
    {

        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile WorkerRepository instance;

        private WorkerRepository() { }

        public static WorkerRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new WorkerRepository();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Worker

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<WorkerEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Workers
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }


        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<WorkerEntity> GetAll(int i)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.Workers
                            select sc;

                return query.ToList();
            }
                

            
        }

        /*
        /// <summary>
        /// Busca todos funcionarios ativos de uma empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetAllFromFirm(int firmId, int pageIndex = 0, int pageSize = 10)
        {
            List<WorkerDTO> list = new List<WorkerDTO>();
            using (ModelContext context = new ModelContext())
            {
                var query = Resources.SQL_WORKERS;
                query = string.Format(query, firmId, pageIndex * pageSize, pageSize);

                list = context.Database.SqlQuery<WorkerDTO>(query).ToList();
            }
            return list;
        }
        */

        /// <summary>
        /// Busca todos funcionarios ativos de uma equipe
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="teamId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetAllFromTeam(int firmId, int teamId, int pageIndex = 0, int pageSize = 10)
        {
            List<WorkerDTO> list = new List<WorkerDTO>();
            using (ModelContext context = new ModelContext())
            {
                var query = Resources.SQL_WORKERS_FROM_TEAM;
                query = string.Format(query, teamId, firmId, pageIndex * pageSize, pageSize);

                list = context.Database.SqlQuery<WorkerDTO>(query).ToList();
            }
            return list;
        }

        /// <summary>
        /// Busca todos funcionarios ativos de uma equipe
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetDTOFromListExternalId(List<string> externalIds)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from wp in context.Profiles
                            from wt in context.WorkerTypes
                            from externalId in externalIds
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.ExternalId == externalId
                            && worker.UserId == wp.Id
                            && worker.WorkerTypeId == wt.Id
                            select new WorkerDTO
                            {
                                Name = wp.Name,
                                IdWorker = worker.Id,
                                WorkerTypeId = worker.WorkerTypeId,
                                IdUser = worker.UserId,
                                LogoId = worker.LogoId,
                                Cpf = wp.CPF,
                                Email = wp.Email,
                                Phone = wp.Phone,
                                WorkerTypeName = wt.TypeName,
                                ExternalId = externalId
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos funcionarios ativos de uma equipe
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetAllFromTeam(int teamId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from wp in context.Profiles
                            from tw in context.TeamWorkers
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && tw.TeamId == teamId
                            && worker.Id == tw.WorkerId
                            && worker.UserId == wp.Id
                            && worker.WorkerTypeId == wt.Id
                            select new WorkerDTO
                            {
                                Name = wp.Name,
                                IdWorker = worker.Id,
                                WorkerTypeId = worker.WorkerTypeId,
                                IdUser = worker.UserId,
                                LogoId = worker.LogoId,
                                Cpf = wp.CPF,
                                Email = wp.Email,
                                Phone = wp.Phone,
                                IdAssociation = tw.Id,
                                WorkerTypeName = wt.TypeName
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos funcionarios ativos de uma equipe
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public List<WorkerEntity> GetAllByWorkerType(int workerTypeId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.WorkerTypeId == workerTypeId
                            select worker;

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca um funcionario pelo Id externo
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public WorkerEntity GetByExternalId(string externalId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.ExternalId == externalId
                            select worker;

                return query.FirstOrDefault();
            }
        }

                /// <summary>
        /// Busca um funcionario pelo Id externo
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public WorkerDTO GetDTOByExternalId(string externalId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.UserId == profile.Id
                            && worker.ExternalId == externalId
                            select new WorkerDTO
                            {
                                Name = profile.Name,
                                IdWorker = worker.Id,
                                WorkerTypeId = worker.WorkerTypeId,
                                IdUser = worker.UserId,
                                LogoId = worker.LogoId,
                                Cpf = profile.CPF,
                                Email = profile.Email,
                                Phone = profile.Phone
                            };

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca todos os responsaveis de times na lista recebida
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetAllSponsorByListTeamId(List<string> teamIds)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && teamIds.Contains(worker.ExternalId)
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                WorkerTypeName = wt.TypeName
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos os responsaveis de times na lista recebida
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetWorkerDTOByListExternalId(List<string> ExternalIds)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where ExternalIds.Contains(worker.ExternalId)
                            && worker.UserId == profile.Id
                            && worker.WorkerTypeId == wt.Id
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                WorkerTypeName = wt.TypeName
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos os responsaveis de times na lista recebida
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public WorkerDTO GetWorkerDTOByExternalId(string externalId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.ExternalId == externalId
                            && worker.UserId == profile.Id
                            && worker.WorkerTypeId == wt.Id
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                WorkerTypeName = wt.TypeName,
                                ProfileName = wt.ProfileName
                            };

                return query.FirstOrDefault();
            }
        }




        /// <summary>
        /// Busca todos os responsaveis de times na lista recebida
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetWorkerDTOByExternalGameId(string gameId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where 
                            //worker.Status == GenericStatus.ACTIVE
                            worker.ExternalFirmId == gameId
                            && worker.UserId == profile.Id
                            && worker.WorkerTypeId == wt.Id
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                WorkerTypeName = wt.TypeName,
                                ProfileName = wt.ProfileName,
                                Role = wt.ProfileName.ToString()
                            };

                return query.ToList();
            }
        }


        /// <summary>
        /// Busca todos funcionarios ativos de uma equipe
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetAllFromTeam(int teamId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from worker in context.Workers
                            from wp in context.Profiles
                            from tw in context.TeamWorkers
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && tw.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && tw.TeamId == teamId
                            && worker.Id == tw.WorkerId
                            && worker.UserId == wp.Id
                            && worker.WorkerTypeId == wt.Id
                            select new WorkerDTO
                            {
                                Name = wp.Name,
                                IdWorker = worker.Id,
                                WorkerTypeId = worker.WorkerTypeId,
                                IdUser = worker.UserId,
                                LogoId = worker.LogoId,
                                Cpf = wp.CPF,
                                Email = wp.Email,
                                Phone = wp.Phone,
                                IdAssociation = tw.Id,
                                WorkerTypeName = wt.TypeName
                            }).OrderBy(x => x.Name).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera o responsavel pelo id da equipe
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public WorkerEntity GetSponsorTeam(int teamId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from team in context.Teams
                            from worker in context.Workers
                            where team.Status == GenericStatus.ACTIVE
                            && team.Id == teamId
                            && worker.Id == team.SponsorId
                            select worker;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca todos funcionarios ativos sem equipe e de um determinado perfil
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="teamId"></param>
        /// <param name="profileId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetWorkersToAssociateByProfileWithoutTeamSponsor(int firmId, int teamId, int profileId, int pageIndex = 0, int pageSize = 10)
        {
            List<WorkerDTO> list = new List<WorkerDTO>();
            using (ModelContext context = new ModelContext())
            {
                var query = Resources.SQL_WORKERS_TO_ASSOCIATE;
                query = string.Format(query, firmId, teamId, profileId, pageIndex * pageSize, pageSize);

                list = context.Database.SqlQuery<WorkerDTO>(query).ToList();
            }
            return list;
        }
   
        /// <summary>
        /// Busca todos funcionarios ativos de uma empresa pelo perfil
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="profileId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetAllFromFirmByProfile(int firmId, Profiles profileId, int pageIndex, int pageSize)
        {
            List<WorkerDTO> list = new List<WorkerDTO>();
            using (ModelContext context = new ModelContext())
            {
                var query = Resources.SQL_WORKERS_BY_PROFILE;
                query = string.Format(query, firmId, (int)profileId, pageIndex * pageSize, pageSize);

                list = context.Database.SqlQuery<WorkerDTO>(query).ToList();
            }
            return list;
        }

        /// <summary>
        /// Busca todos funcionarios ativos de uma empresa pelo perfil
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetAllFromFirmByProfile(string firmId, Profiles profileId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && wt.Id == worker.WorkerTypeId
                            && wt.ProfileName == profileId
                            && profile.Id == worker.UserId
                            && worker.ExternalFirmId == firmId
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                WorkerTypeName = wt.TypeName
                            }; 

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos funcionarios ativos de uma empresa pelo perfil
        /// </summary>
        /// <returns></returns>
        public List<WorkerDTO> GetAllDTO()
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && profile.Id == worker.UserId
                            && wt.Id == worker.WorkerTypeId
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                Role = wt.ProfileName.ToString()
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera o funcionario pelo id
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public WorkerEntity GetById(int workerId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Workers
                        where sc.Id == workerId
                        select sc;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Recupera o responsavel pelo id da equipe
        /// </summary>
        /// <param name="externalIds"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetUserProfileByListOfExternalIds(List<string> externalIds)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && externalIds.Contains(worker.ExternalId)
                            && profile.Id == worker.UserId
                            && wt.Id == worker.WorkerTypeId
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                WorkerTypeName = wt.TypeName
                            };


                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera os funcionario para serem associados de um tipo de jogador que ainda nao estejam associados a equipe
        /// </summary>
        /// <param name="externalIds"></param>
        /// <returns></returns>
        public List<WorkerDTO> GetUserProfileByWorkerTypeIdExceptWhenOnListOfExternalIds(List<string> externalIds, int workerTypeId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && !externalIds.Contains(worker.ExternalId)
                            && worker.WorkerTypeId == workerTypeId
                            && profile.Id == worker.UserId
                            && wt.Id == worker.WorkerTypeId
                            select new WorkerDTO
                            {
                                Cpf = profile.CPF,
                                Name = profile.Name,
                                Email = profile.Email,
                                IdAssociation = 0,
                                ExternalId = worker.ExternalId,
                                IdUser = worker.UserId,
                                IdWorker = worker.Id,
                                LogoId = worker.LogoId,
                                Phone = profile.Phone,
                                TotalPoints = 0,
                                WorkerTypeId = worker.WorkerTypeId,
                                WorkerTypeName = wt.TypeName
                            }).OrderBy(x => x.Name).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera a quantidade de funcionarios para serem associados de um tipo de jogador que ainda nao estejam associados a equipe
        /// </summary>
        /// <param name="externalIds"></param>
        /// <returns></returns>
        public int GetCountByWorkerTypeId(List<string> externalIds, int workerTypeId)
        {
            using (ModelContext context = new ModelContext())
            {
                int count = (from worker in context.Workers
                            from profile in context.Profiles
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && !externalIds.Contains(worker.ExternalId)
                            && worker.WorkerTypeId == workerTypeId
                            && profile.Id == worker.UserId
                            && wt.Id == worker.WorkerTypeId
                            select worker).Count();

                return count;
            }
        }

        /*
        /// <summary>
        /// Recupera o funcionario dto pelo id
        /// </summary>
        /// <param name="workerId"></param>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public WorkerDTO GetDTOById(int workerId, int firmId)
        {
            WorkerDTO worker = new WorkerDTO();
            using (ModelContext context = new ModelContext())
            {
                var query = Resources.SQL_WORKER;
                query = string.Format(query, firmId, workerId);

                worker = context.Database.SqlQuery<WorkerDTO>(query).FirstOrDefault();
            }
            return worker;
        }
        */

        /// <summary>
        /// Recupera o funcionario dto pelo id
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public WorkerDTO GetDTOById(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from userProfile in context.Profiles
                            from workerType in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.Id == workerId
                            && worker.UserId == userProfile.Id
                            && worker.WorkerTypeId == workerType.Id
                            select new WorkerDTO
                            {
                                Cpf = userProfile.CPF,
                                Name = userProfile.Name,
                                Email = userProfile.Email,
                                WorkerTypeName = workerType.TypeName,
                                WorkerTypeId = workerType.Id,
                                Phone = userProfile.Phone,
                                LogoId = worker.LogoId,
                                IdWorker = worker.Id,
                                IdUser = worker.UserId,
                                ExternalId = worker.ExternalId
                            };

                return query.FirstOrDefault();
            }
            
        }

        /// <summary>
        /// Recupera o funcionario dto pelo id
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public WorkerDTO GetDTOById(string workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from userProfile in context.Profiles
                            from workerType in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.ExternalId == workerId
                            && worker.UserId == userProfile.Id
                            && worker.WorkerTypeId == workerType.Id
                            select new WorkerDTO
                            {
                                Cpf = userProfile.CPF,
                                Name = userProfile.Name,
                                Email = userProfile.Email,
                                WorkerTypeName = workerType.TypeName,
                                WorkerTypeId = workerType.Id,
                                Phone = userProfile.Phone,
                                LogoId = worker.LogoId,
                                IdWorker = worker.Id,
                                IdUser = worker.UserId,
                                ExternalId = worker.ExternalId
                            };

                return query.FirstOrDefault();
            }

        }

        /// <summary>
        /// Recupera o funcionario dto pelo id
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public WorkerDTO GetDTOByWorkerId(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from w in context.Workers
                            from p in context.Profiles
                            from wt in context.WorkerTypes
                            where w.Status == GenericStatus.ACTIVE
                            && w.Id == workerId
                            && wt.Id == w.WorkerTypeId
                            && p.Id == w.Id
                            select new WorkerDTO
                            {
                                Name = p.Name,
                                IdWorker = w.Id,
                                WorkerTypeId = w.WorkerTypeId,
                                WorkerTypeName = wt.TypeName,
                                LogoId = w.LogoId,
                                Cpf = p.CPF,
                                Email = p.Email,
                                Phone = p.Phone,
                            };

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Recupera o funcionario pelo userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WorkerEntity GetByUserId(int userId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Workers
                        where sc.UserId == userId
                        orderby sc.Id ascending
                        select sc;
            //sc.Status == GenericStatus.ACTIVE &&

            return query.FirstOrDefault();
        }

        ///<summary>
        ///Busca todos os funcionarios do perfil jogador e lider de uma firma
        /// </summary>
        public List<WorkerDTO> GetAllPlayersAndLeadersFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from w in context.Workers
                            from wt in context.WorkerTypes
                            from u in context.Profiles
                            where w.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && w.FirmId == firmId
                            && wt.FirmId == firmId
                            && w.WorkerTypeId == wt.Id
                            && (wt.ProfileName == Profiles.JOGADOR || wt.ProfileName == Profiles.LIDER )
                            && w.UserId == u.Id
                            select new WorkerDTO
                            {
                                IdWorker = w.Id,
                                IdUser = w.UserId,
                                WorkerTypeId = w.WorkerTypeId,
                                Name = u.Name,
                                Cpf = u.CPF,
                                Email = u.Email,
                                Phone = u.Phone,
                                WorkerTypeName = wt.TypeName
                            };

                return query.ToList();
            }
        }

        ///<summary>
        ///Busca todos os funcionarios do perfil jogador de uma firma
        /// </summary>
        public List<WorkerDTO> GetAllPlayersFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from w in context.Workers
                            from wt in context.WorkerTypes
                            from u in context.Profiles
                            where w.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && w.FirmId == firmId
                            && wt.FirmId == firmId
                            && w.WorkerTypeId == wt.Id
                            && wt.ProfileName == Profiles.JOGADOR
                            && w.UserId == u.Id
                            select new WorkerDTO
                            {
                                IdWorker = w.Id,
                                IdUser = w.UserId,
                                WorkerTypeId = w.WorkerTypeId,
                                Name = u.Name,
                                Cpf = u.CPF,
                                Email = u.Email,
                                Phone = u.Phone,
                                WorkerTypeName = wt.TypeName
                            };

                return query.ToList();
            }
        }

        ///<summary>
        ///Busca todos os funcionarios do perfil jogador de uma firma
        /// </summary>
        public List<WorkerDTO> GetAllByProfileFromFirm(int firmId, Profiles profile, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from w in context.Workers
                            from wt in context.WorkerTypes
                            from u in context.Profiles
                            where w.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && w.FirmId == firmId
                            && wt.FirmId == firmId
                            && w.WorkerTypeId == wt.Id
                            && wt.ProfileName == profile
                            && w.UserId == u.Id
                            select new WorkerDTO
                            {
                                IdWorker = w.Id,
                                IdUser = w.UserId,
                                WorkerTypeId = w.WorkerTypeId,
                                Name = u.Name,
                                Cpf = u.CPF,
                                Email = u.Email,
                                Phone = u.Phone,
                                WorkerTypeName = wt.TypeName,
                                LogoId = w.LogoId
                            }).OrderBy(x => x.Name).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        ///<summary>
        ///Busca a quantidade de jogadores de uma firma
        /// </summary>
        public int GetCountByProfileFromFirm(int firmId, Profiles profile)
        {
            using (ModelContext context = new ModelContext())
            {
                int count = (from worker in context.Workers
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.FirmId == firmId
                            && worker.WorkerTypeId == wt.Id
                            && wt.ProfileName == profile
                            select worker).Count();

                return count;
            }
        }

        ///<summary>
        ///Busca todos os funcionarios do perfil jogador de uma firma
        /// </summary>
        public List<WorkerDTO> GetAllFromFirm(int firmId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from w in context.Workers
                             from wt in context.WorkerTypes
                             from u in context.Profiles
                             where w.Status == GenericStatus.ACTIVE
                             && wt.Status == GenericStatus.ACTIVE
                             && w.FirmId == firmId
                             && wt.FirmId == firmId
                             && w.WorkerTypeId == wt.Id
                             && w.UserId == u.Id
                             select new WorkerDTO
                             {
                                 IdWorker = w.Id,
                                 IdUser = w.UserId,
                                 WorkerTypeId = w.WorkerTypeId,
                                 Name = u.Name,
                                 Cpf = u.CPF,
                                 Email = u.Email,
                                 Phone = u.Phone,
                                 WorkerTypeName = wt.TypeName,
                                 LogoId = w.LogoId
                             }).OrderBy(x => x.Name).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        ///<summary>
        ///Busca a quantidade de jogadores de uma firma
        /// </summary>
        public int GetCountFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                int count = (from worker in context.Workers
                             from wt in context.WorkerTypes
                             where worker.Status == GenericStatus.ACTIVE
                             && worker.FirmId == firmId
                             && worker.WorkerTypeId == wt.Id
                             select worker).Count();

                return count;
            }
        }

        ///<summary>
        ///Busca os jogadores data de acesso
        /// </summary>
        public List<ReportDTO> GetWorkerDTOByDate(DateTime initDate, DateTime finishDate, string gameId = "")
        {
            using (ModelContext context = new ModelContext())
            {

                CultureInfo cult = new CultureInfo("pt-BR");

                var lastUpdateDevices = from device in context.AccountDevices
                                        group device by device.External_User_Id into d
                                        select d.OrderByDescending(x => x.Last_Update).FirstOrDefault();

                var games = gameId == "" ?
                    from firm in context.Datas select firm :
                    from firm in context.Datas where firm.ExternalId == gameId select firm;

                var workers = from worker in context.Workers
                            join device in lastUpdateDevices on worker.ExternalId equals device.External_User_Id into lud
                            from d in lud.DefaultIfEmpty() select new { device = d, worker = worker };
                /*
                var emails = from emailLogs in context.EmailLogs
                             from game in games
                             where emailLogs.GameId == game.ExternalId
                             group emailLogs by emailLogs.PlayerId into p
                             select new { To = p.FirstOrDefault().To , count = p.Count(), max = p.Max(x => x.SendTime) };
*/
                var query = (from workerDevice in workers
                             from profile in context.Profiles
                             from firm in games
                             from userAccount in context.Users
                             //from email in emails
                             where workerDevice.worker.Status == GenericStatus.ACTIVE
                             && ((profile.LastUpdate >= initDate
                             && profile.LastUpdate <= finishDate) ||
                             (workerDevice.device == null ? false : (workerDevice.device.Last_Update >= initDate &&
                             workerDevice.device.Last_Update <= finishDate)))
                             && profile.Id == workerDevice.worker.UserId
                             && firm.ExternalId == workerDevice.worker.ExternalFirmId
                             && userAccount.Id == workerDevice.worker.UserId
                             //&& email.To == profile.Email
                             select new ReportDTO
                             {
                                 Name = profile.Name,
                                 Email = profile.Email,
                                 GameName = firm.FirmName,
                                 LastUpdateMobile = workerDevice.device == null ? new DateTime() : workerDevice.device.Last_Update,
                                 LastUpdateWeb = userAccount.LastUpdate,
                                 LastUpdateMobileString = (workerDevice.device == null ? new DateTime() : workerDevice.device.Last_Update).ToString(),//("dd/MM/yyyy HH:mm:ss", cult),
                                 LastUpdateWebString = userAccount.LastUpdate.ToString(),//("dd/MM/yyyy HH:mm:ss", cult),
                                 //LastReciveEmail = email.max,
                                 //LastReciveEmailString = email.max.ToString(),//("dd/MM/yyyy HH:mm:ss", cult),
                                 //CountEmails = email.count

                             }).OrderBy(x => x.GameName);
                             

                return query.ToList();
            }
        }

        /// <summary>
        /// Salva um funcionario na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public WorkerEntity CreateWorker(WorkerEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Workers.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza um funcionario
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public WorkerEntity UpdateWorker(WorkerEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Workers.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        /// <summary>
        /// Remover um funcionario
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void RemoveWorker(int workerId)
        {

            using (ModelContext context = new ModelContext())
            {
                WorkerEntity workerEntity = GetById(workerId);
                context.Workers.Attach(workerEntity);
                context.Workers.Remove(workerEntity);
                context.SaveChanges();
            }
        }

        #endregion

    }
}
