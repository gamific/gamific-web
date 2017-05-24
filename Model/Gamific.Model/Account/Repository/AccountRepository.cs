using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Account.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Account.Model
{
    public class AccountRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile AccountRepository instance;

        private AccountRepository() { }

        public static AccountRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new AccountRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region UserAccount

        /// <summary>
        /// Busca conta por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserAccountEntity GetById(int id)
        {
            using (ModelContext context = new ModelContext())
            {
                var foundUserAccount = from c in context.Users where c.Id == id select c;
                return foundUserAccount.FirstOrDefault();
            }
        }
        /// <summary>
        /// Busca contas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<UserAccountEntity> GetAll()
        {
            using (ModelContext context = new ModelContext())
            {
                var foundUserAccount = from c in context.Users select c;
                return foundUserAccount.ToList();
            }
        }

        /// <summary>
        /// Cria uma conta de usuário
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public UserAccountEntity Create(UserAccountEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.Status = GenericStatus.ACTIVE;
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Users.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();

                return newEntity;
            }
        }

        /// <summary>
        /// Atualiza uma conta de usuário
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public UserAccountEntity Update(UserAccountEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Users.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return updatedEntity;
            }
        }

        /// <summary>
        /// Inativa uma conta de usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(int id)
        {

            UserAccountEntity account = this.GetById(id);

            if (account == null)
                return false;

            using (ModelContext context = new ModelContext())
            {
                context.Users.Attach(account);
                context.Entry(account).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
        }


        /// <summary>
        /// Busca por usuário e senha
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserAccountEntity FindByUserName(string username)
        {
            using (ModelContext context = new ModelContext())
            {
                var foundUserAccount = from c in context.Users
                                       where c.UserName.Equals(username) && c.Status == GenericStatus.ACTIVE
                                       select c;

                UserAccountEntity userAccount = foundUserAccount.FirstOrDefault();

                return userAccount;
            }
        }


        /// <summary>
        /// Recupera o perfil de usuário pelo id da conta
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserProfileEntity GetProfileById(int userId)
        {
            using (ModelContext context = new ModelContext())
            {
                var foundUserAccount = from c in context.Profiles where c.Id == userId select c;
                return foundUserAccount.FirstOrDefault();
            }
        }

        /// <summary>
        /// Recupera um profile pelo cpf
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public UserProfileEntity GetProfileByCPF(string cpf)
        {
            using (ModelContext context = new ModelContext())
            {
                var foundUserAccount = from c in context.Profiles where c.CPF.Equals(cpf) select c;
                return foundUserAccount.FirstOrDefault();
            }
        }

        /// <summary>
        /// Cria um perfil do usuário
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public UserProfileEntity CreateProfile(UserProfileEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Profiles.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();

                return newEntity;
            }
        }

        /// <summary>
        /// Recupera as roles de um usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Roles> GetUserRoles(int userId)
        {
            using (ModelContext context = new ModelContext())
            {
                var foundUserAccount = from r in context.Roles where r.UserId == userId select r.Role;
                return foundUserAccount.ToList();
            }
        }


        /// <summary>
        /// Cria uma role para o usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public UserRoleEntity CreateUserRole(int userId, Roles role)
        {
            using (ModelContext context = new ModelContext())
            {
                UserRoleEntity newEntity = new UserRoleEntity() { Role = role, UserId = userId };

                context.Roles.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();

                return newEntity;
            }
        }

        /// <summary>
        /// Cria uma role para o usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public UserRoleEntity GetUserRoleById(int userId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from c in context.Roles where c.UserId == userId select c;
                UserRoleEntity entity = query.FirstOrDefault();

                return entity;
            }
        }

        /// <summary>
        /// Remover um funcionario
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void RemoveAccount(int userId)
        {
            using (ModelContext context = new ModelContext())
            {
                UserAccountEntity accountEntity = GetById(userId);
                UserProfileEntity profileEntity = GetProfileById(userId);
                UserRoleEntity roleEntity = GetUserRoleById(userId);
                context.Roles.Attach(roleEntity);
                context.Roles.Remove(roleEntity);
                context.SaveChanges();
                context.Users.Attach(accountEntity);
                context.Users.Remove(accountEntity);
                context.SaveChanges();
                context.Profiles.Attach(profileEntity);
                context.Profiles.Remove(profileEntity);
                context.SaveChanges();
            }
        }

        #endregion

    }
}
