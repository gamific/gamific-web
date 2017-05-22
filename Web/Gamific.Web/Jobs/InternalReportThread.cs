using System;
using Gamific.Scheduler;
using System.Threading;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Util.Parameter;
using Vlast.Broker.EMAIL;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using System.Collections;
using System.Globalization;

namespace Vlast.Gamific.Web.Jobs
{
    /// <summary>
    /// Envia, periodicamente, por email para os analizadores Gamific.
    /// </summary>
    public class InternalReportThread : BaseThread
    {

        private string email = "rafael@gamific.com.br";
        public override void Init(TimeSpan timeToRun)
        {
            RankingMailThread rankingThread = new RankingMailThread();

            rankingThread.timeToRun = timeToRun;

            Instance = new Thread(rankingThread.Start);
            Instance.Start();
        }

        public async override void Run()
        {

            //Send(new EmailSupportDTO { Msg = "Bom dia", Category = "", Subject = "Testes Gamific" }, "igorgarantes@gmail.com");

            //GetAllDTO players = PlayerEngineService.Instance.GetByGameId(game.Id);

            //GetAllDTO episodes = EpisodeEngineService.Instance.GetByGameIdAndActive(game.Id, 1);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            string dayOfWeek = DateTime.Now.ToString("ddd");
            List<EpisodeEngineDTO> episodes;
            //episodes.Add(new EpisodeEngineDTO("58e3708a3a8778588098a82b", "5880a1743a87783b4f0ba709"));

            GetAllDTO allEpisodes = EpisodeEngineService.Instance.GetAll(0, 100000);

            episodes = allEpisodes.List.episode.Where(x => x.Active == true && x.sendEmail == true && (x.DaysOfWeek != null ? x.DaysOfWeek.Split(',').Contains(dayOfWeek.ToLower()) : false)).ToList();

            foreach (EpisodeEngineDTO episode in episodes)
            {
                List<string> emails = new List<string>();
                GetAllDTO teams = TeamEngineService.Instance.FindByEpisodeId(episode.Id, email);
                GameEngineDTO game = GameEngineService.Instance.GetById(episode.GameId, email);

                foreach (TeamEngineDTO team in teams.List.team)
                {
                    GetAllDTO runs = RunEngineService.Instance.GetRunsByTeamId(team.Id, email);

                    foreach (RunEngineDTO run in runs.List.run)
                    {
                        WorkerDTO worker = WorkerRepository.Instance.GetWorkerDTOByExternalId(run.PlayerId);
                        if (worker != null && (worker.ProfileName == Profiles.LIDER || worker.ProfileName == Profiles.JOGADOR))
                        {
                            string emailBody = CreateEmail(game, episode.Id, team.Id, worker.ExternalId, worker);
                            Send(new EmailSupportDTO { Msg = emailBody, Category = "", Subject = "Ranking Gamific" }, "igorgarantes@gmail.com");
                            //Send(new EmailSupportDTO { Msg = emailBody, Category = "", Subject = "Ranking Gamific" }, worker.Email);
                        }
                    }
                }
            }



        }

        private string CreateDatesXls()
        {
            UserAccountEntity accontEntity = AccontRepository.Instance.GetAll();
            AccontDevicesEntity accontDeviceEntity = AccontRepository.instance.GetAllDevice();

            resultAcconts = (from e in accontEntity
                      select new UserAccountEntity
                      {
                          Name = e.UserName,
                          LastUpdate = e.LastUpdate,
                          LastLogin = e.LastLogin
                      }).ToList();

            


           /* results = (from d in accontDeviceEntity
                      select new AccontDevicesEntity
                      {
                          Name = d.UserName,
                          Last_Update = d.Last_Update
                      }).ToList();*/

            DataSet ds = new DataSet("New_DataSet");
            DataTable dt = new DataTable("Table_Users_Web");
            dt.Columns.Add("Nome");
            dt.Columns.Add("Ultimo_Update");
            dt.Columns.Add("Ultimo_Login");

            foreach (string result in resultAcconts)
            {
                dt.Rows.Add(result.name, result.LastUpdate, result.LastLogin);
            }

            

            return null;
        }


    }

}