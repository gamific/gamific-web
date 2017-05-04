using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Account.Repository;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Services.Push;
using Vlast.Gamific.Web.Services.Push.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/notificacoes")]
    [CustomAuthorize(Roles = "ADMINISTRADOR")]
    public class PushNotificationController : BaseController
    {
        // GET: PushNotification
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarEpisodios")]
        [HttpGet]
        public ActionResult SearchEpisodes(int state = 1)
        {
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, state);

            return Json(JsonConvert.SerializeObject(all.List.episode), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os times
        /// </summary>
        /// <returns></returns>
        [Route("buscarEquipes")]
        [HttpGet]
        public ActionResult SearchTeams(string episodeId)
        {
            GetAllDTO all = TeamEngineService.Instance.FindByEpisodeId(episodeId);

            return Json(JsonConvert.SerializeObject(all.List.team), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os jogadores de uma equipe
        /// </summary>
        /// <returns></returns>
        [Route("buscarJogadores")]
        [HttpGet]
        public ActionResult SearchPlayers(string teamId)
        {
            GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId, 0, 10000);

            List<string> externalIds = (from run in all.List.run select run.PlayerId).ToList();

            List<WorkerDTO> workers = WorkerRepository.Instance.GetDTOFromListExternalId(externalIds);

            List<SelectListItem> workersList = (from worker in workers
                                                select new SelectListItem
                                                {
                                                    Value = worker.ExternalId,
                                                    Text = worker.Name
                                                }).ToList();

            return Json(JsonConvert.SerializeObject(workersList), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega um datatable com todas as equipes do episodio ordenados por Score
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <returns></returns>
        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, string episodeId, string teamId)
        {
            GetAllDTO all;

            if (jqueryTableRequest != null)
            {
                if (teamId != "empty")
                {
                    all = RunEngineService.Instance.GetRunsByTeamId(teamId, jqueryTableRequest.Page);
                    List<PlayerEngineDTO> players = (from run in all.List.run select PlayerEngineService.Instance.GetById(run.PlayerId)).ToList();

                    all.List.result = (from run in all.List.run
                                       from player in players
                                       where run.PlayerId == player.Id
                                       && player.Active == true
                                       select new ResultEngineDTO
                                       {
                                           Id = player.Id,
                                           Nick = player.Nick,
                                           Score = run.Score,
                                           LogoId = player.LogoId
                                       }).ToList();
                }
                else
                {
                    all = TeamEngineService.Instance.FindByEpisodeIdAndGameId(episodeId, CurrentFirm.ExternalId, jqueryTableRequest.Page);
                    all.List.result = (from team in all.List.team
                                       select new ResultEngineDTO
                                       {
                                           Id = team.Id,
                                           Nick = team.Nick,
                                           Score = team.Score,
                                           LogoId = team.LogoId
                                       }).ToList();
                }

                int index = 0;
                if (jqueryTableRequest.Order != null)
                {
                    index = Int32.Parse(jqueryTableRequest.Order);
                }
                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = 1,//all.PageInfo.totalElements,
                        RecordsFiltered = 1,//all.PageInfo.totalElements,
                        Data = all.List.result.OrderBy(t => t.Nick).Select(t => new string[] { t.Id, t.Nick}).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = 1,//all.PageInfo.totalElements,
                        RecordsFiltered = 1,//all.PageInfo.totalElements,
                        Data = all.List.result.OrderByDescending(t => t.Nick).Select(t => new string[] { t.Id, t.Nick }).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("send")]
        [HttpPost]
        public ActionResult SendNotificationPush(List<string> checkedIds, string teamId, string message, string title)
        {
            if(checkedIds == null)
            {
                return Json(new { text = "Selecione os destinatarios.", error = true }, JsonRequestBehavior.DenyGet);
            }
            else if(message == "" || title == "")
            {
                return Json(new { text = "Escreva uma mensagem e um titulo.", error = true }, JsonRequestBehavior.DenyGet);
            }

            List<NotificationPushDTO> notifications = new List<NotificationPushDTO>();

            if(teamId != "empty")
            {
                foreach(string checkedId in checkedIds)
                {
                    List<AccountDevicesEntity> devices = AccountDevicesRepository.Instance.FindByPlayerId(checkedId);

                    foreach (AccountDevicesEntity device in devices)
                    {
                        NotificationPushDTO notification = new NotificationPushDTO
                        {
                            Token = device.Notification_Token,
                            PlayerId = device.External_User_Id,
                            Message = message,
                            Title = title
                        };

                        notifications.Add(notification);
                    }
                }
            }
            else
            {
                foreach (string checkedId in checkedIds)
                {
                    GetAllDTO runs = RunEngineService.Instance.GetRunsByTeamId(checkedId, 0, 100000);

                    foreach(RunEngineDTO run in runs.List.run)
                    {
                        List<AccountDevicesEntity> devices = AccountDevicesRepository.Instance.FindByPlayerId(run.PlayerId);

                        foreach (AccountDevicesEntity device in devices)
                        {
                            NotificationPushDTO notification = new NotificationPushDTO
                            {
                                Token = device.Notification_Token,
                                PlayerId = device.External_User_Id,
                                Message = message,
                                Title = title
                            };

                            notifications.Add(notification);
                        }
                    }
                }
            }

            int countErrors = 0;
            int countSuccess = 0;

            foreach(NotificationPushDTO notification in notifications)
            {
                NotificationLogDTO notificationLog = NotificationPushService.Instance.SendPush(notification);
                if(notificationLog.Success == "0")
                {
                    countErrors++;
                }
                else
                {
                    countSuccess++;
                }
            }

            return Json(new { text = countSuccess + " notificações foram enviadas com sucesso!    " + countErrors + " falharam ao serem enviadas!", error = false });
        }

        [Route("getAllPlayerIdsFromTeam/{teamId}")]
        [HttpGet]
        public ActionResult GetAllPlayerIdsFromTeam(string teamId)
        {
            try
            {
                GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId, 0, 100000);
                List<string> playerIds = all.List.run.Select(x => x.PlayerId).ToList();

                return Json(JsonConvert.SerializeObject(playerIds), JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json("Error: " + e.Message, JsonRequestBehavior.DenyGet);
            }
        }

        [Route("getAllTeamIdsFromEpisode/{episodeId}")]
        [HttpGet]
        public ActionResult GetAllTeamIdsFromEpisode(string episodeId)
        {
            try
            {
                GetAllDTO all = TeamEngineService.Instance.FindByEpisodeId(episodeId);
                List<string> teamIds = all.List.team.Select(x => x.Id).ToList();

                return Json(JsonConvert.SerializeObject(teamIds), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Error: " + e.Message, JsonRequestBehavior.DenyGet);
            }
        }

    }
}