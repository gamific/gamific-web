using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Properties;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class MessageRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile MessageRepository instance;

        private MessageRepository() { }

        public static MessageRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new MessageRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Message

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<MessageEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Messages
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Recupera a mensagem pelo id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public MessageEntity GetById(int messageId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Messages
                            where t.Id == messageId
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca os resultados da equipe
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public List<MessageDTO> GetByTeam(string teamId)
        {
            List<MessageDTO> result = new List<MessageDTO>();
            using (ModelContext context = new ModelContext())
            {
                var query = from message in context.Messages
                            from profile in context.Profiles
                            from worker in context.Workers
                            where message.TeamId == teamId
                            && message.Sender== profile.Id
                            && worker.UserId == profile.Id
                            select new MessageDTO
                            {
                                FirmId = message.FirmId,
                                Id = message.Id,
                                TeamId = teamId,
                                Message = message.Message,
                                SendDateTime = message.SendDateTime.ToString(),
                                Sender = message.Sender,
                                SenderLogoId = worker.LogoId,
                                SenderName = profile.Name
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Salva uma mensagem na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public MessageEntity CreateMessage(MessageEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.SendDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")); ;
                context.Messages.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza uma mensagem
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public MessageEntity UpdateTeam(MessageEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.SendDateTime = DateTime.UtcNow;
                context.Messages.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion

    }
}
