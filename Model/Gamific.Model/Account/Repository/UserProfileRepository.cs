using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Account.Domain;

namespace Vlast.Gamific.Model.Account.Repository
{

    /// <summary>
    /// Consulta de dados relacionados com o user profile
    /// </summary>
    public class UserProfileRepository
    {

        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile UserProfileRepository instance;

        private UserProfileRepository() { }

        public static UserProfileRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new UserProfileRepository();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region UserProfile

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public IQueryable<UserProfileEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Profiles
                        orderby sc.Id ascending
                        select sc;

            return query;
        }

        /// <summary>
        /// Recupera as informações de um user profile
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <returns></returns>
        public UserProfileEntity GetById(int userProfileId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.Profiles
                            where sc.Id == userProfileId
                            select sc;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Recupera todas as informações de um user profile
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <returns></returns>
        public List<UserProfileEntity> GetAllUsers()
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.Profiles
                            select sc;

                return query.ToList();
            }
        }

        public UserProfileEntity GetByEmail(string email)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.Profiles
                            where sc.Email == email
                            select sc;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Recupera as informações de um user profile
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public UserProfileEntity GetByWorkerId(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from p in context.Profiles
                            from w in context.Workers
                            where w.Id == workerId
                            && w.UserId == p.Id
                            select p;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva um user profile na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public UserProfileEntity CreateUserProfile(UserProfileEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                context.Profiles.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza um user profile
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public UserProfileEntity UpdateUserProfile(UserProfileEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                context.Profiles.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
