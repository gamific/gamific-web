using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Media.Domain;
using Vlast.Gamific.Model.Public.Domain;
using Vlast.Util.Data;
using Vlast.Util.Parameter;

namespace Vlast.Gamific.Model
{

    public class ModelContext : DbContext
    {
        #region Entities

        public DbSet<MetricEntity> Metrics { get; set; }

        public DbSet<TeamWorkerEntity> TeamWorkers { get; set; }

        public DbSet<ResultEntity> Results { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<UserAccountEntity> Users { get; set; }

        public DbSet<VideoEntity> Videos { get; set; }

        public DbSet<VideoQuestionEntity> VideoQuestions { get; set; }

        public DbSet<VideoQuestionAnsweredEntity> VideoQuestionAnswereds { get; set; }

        public DbSet<UserRoleEntity> Roles { get; set; }

        public DbSet<UserProfileEntity> Profiles { get; set; }

        public DbSet<DataEntity> Datas { get; set; }

        public DbSet<SponsorTeamEntity> SponsorTeams { get; set; }

        public DbSet<WorkerTypeEntity> WorkerTypes { get; set; }

        public DbSet<CampaignEntity> Campaigns { get; set; }

        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<TopicHelpEntity> TopicHelps { get; set; }

        public DbSet<HelpEntity> Helps { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }

        public DbSet<WorkerEntity> Workers { get; set; }

        public DbSet<QuizEntity> QuizEntity { get; set; }

        public DbSet<AnswersEntity> AnswersEntity { get; set; }

        public DbSet<QuestionAnsweredEntity> QuestionAnsweredEntity { get; set; }

        public DbSet<QuestionAnswersEntity> QuestionAnswersEntity { get; set; }

        public DbSet<QuizQuestionEntity> QuizQuestionEntity { get; set; }

        public DbSet<QuizCampaignEntity> QuizCampaignEntity { get; set; }

        public DbSet<QuestionEntity> QuestionEntity { get; set; }

        public DbSet<WorkerTypeMetricEntity> WorkerTypeMetrics { get; set; }

        public DbSet<ParamEntity> Params { get; set; }

        public DbSet<AccountDevicesEntity> AccountDevices { get; set; }

        public DbSet<EmailLogEntity> EmailLogs { get; set; }

        public DbSet<QuestionAnsweredSelectedEntity> QuestionAnsweredSelectedEntity { get; set; }

        #endregion

        internal static int DEFAULT_PAGE_SIZE = 100;

        public ObjectContext ObjectContextInstance
        {
            get
            {
                return (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
            }
        }

        public static CultureInfo DataBaseCulture = CultureInfo.CreateSpecificCulture("en-US");

        public ModelContext()
            : base("name=DB_CONNECTION")
        {
            Database.Connection.ConnectionString = ParameterCache.DB_CONNECTION_STRING;

#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }

        /// <summary>
        /// Executa uma consulta customizada (SQL language) no banco e retorna a lista de objetos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> ExecuteQuery<T>(string query) where T : new()
        {
            return DBHelper.ExecuteQuery<T>(this, query);
        }

        /// <summary>
        /// Executa operações de update e delete diretamente na base de dados
        /// </summary>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query)
        {
            return DBHelper.ExecuteNonQuery(this, query);
        }
    }


    public enum DATABASES
    {

        PROCESSADORA,
        CONVENIO,
        NOTIFICACAO
    }
}



