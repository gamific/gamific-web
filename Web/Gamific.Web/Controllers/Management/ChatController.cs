using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("public/chat")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class ChatController : BaseController
    {
        /*
        /// <summary>
        /// Index
        /// </summary>
        /// <returns> </returns>
        [Route("")]
        public ActionResult Index()
        {
            WorkerTypeEntity currentWorkerType = WorkerTypeRepository.Instance.GetById(CurrentWorker.WorkerTypeId);

            List<TeamEntity> teams = new List<TeamEntity>();

            if (currentWorkerType.ProfileName.Equals("ADMINISTRADOR"))
            {
                teams = TeamRepository.Instance.GetAllFromWorker(CurrentWorker.Id, CurrentFirm.Id);

                List<TeamEntity> teamsToAdd = TeamRepository.Instance.GetAllEntityFromFirm(CurrentFirm.Id);

                if (teamsToAdd != null)
                {
                    teams.AddRange(teamsToAdd);
                }
            }
            else if (currentWorkerType.ProfileName.Equals("LIDER"))
            {
                teams = TeamRepository.Instance.GetAllFromWorker(CurrentWorker.Id, CurrentFirm.Id);

                List<TeamEntity> teamsToAdd = TeamRepository.Instance.GetBySponsor(CurrentFirm.Id);

                if (teamsToAdd != null)
                {
                    teams.AddRange(teamsToAdd);
                }
            }
            else
            {
                teams = TeamRepository.Instance.GetAllFromWorker(CurrentWorker.Id, CurrentFirm.Id);
            }

            TeamEntity team = new TeamEntity();

            ViewBag.Team = team;

            return View(teams);
        }
        */

        /// <summary>
        /// Index
        /// </summary>
        /// <returns> </returns>
        [Route("")]
        public ActionResult Index()
        {
            List<TeamEngineDTO> teams = new List<TeamEngineDTO>();

            if(CurrentWorkerType.ProfileName == Profiles.LIDER || CurrentWorkerType.ProfileName == Profiles.JOGADOR)
            {
                teams.AddRange(TeamEngineService.Instance.GetAllTeamsByPlayerId(CurrentFirm.ExternalId, CurrentWorker.ExternalId));
            }
            else if(CurrentWorkerType.ProfileName == Profiles.ADMINISTRADOR)
            {
                teams.AddRange(TeamEngineService.Instance.GetAllTeamsByGameId(CurrentFirm.ExternalId));
            }

            ViewBag.WorkerId = CurrentWorker.Id;

            return View(teams);
        }

        /// <summary>
        /// Busca as mensagens de uma equipe
        /// </summary>
        /// <returns> </returns>
        [Route("buscarMensagens/{teamId}")]
        [HttpGet]
        public ActionResult ChatMessages(string teamId)
        {
            List<MessageDTO> messagesToAdd = MessageRepository.Instance.GetByTeam(teamId);

            if (messagesToAdd == null && messagesToAdd.Count <= 0)
            {
                messagesToAdd = new List<MessageDTO>();
            }

            TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);

            ViewBag.Team = team;
            ViewBag.WorkerId = CurrentWorker.UserId;

            return PartialView("_ChatMessages", messagesToAdd);
        }

        /// <summary>
        /// Enviar uma nova
        /// </summary>
        /// <returns> </returns>
        [Route("enviarMensagem")]
        [HttpPost]
        public ActionResult SendMessage(string message, string teamId)
        {
            MessageEntity messageObj = new MessageEntity();

            messageObj.TeamId = teamId;
            messageObj.Sender = CurrentUserId;
            messageObj.Message = message;
            messageObj.FirmId = CurrentFirm.Id;

            messageObj = MessageRepository.Instance.CreateMessage(messageObj);

            MessageDTO messageInformation = new MessageDTO()
            {
                SenderName = CurrentUserProfile.Name,
                SendDateTime = messageObj.SendDateTime.ToString("dd/MM/yyyy HH:mm:ss"),
                SenderLogoId = CurrentWorker.LogoId,
                Message = message,
                Id = messageObj.Id,
                FirmId = CurrentFirm.Id,
                TeamId = messageObj.TeamId,
                Sender = messageObj.Sender
            };

            return Json(JsonConvert.SerializeObject(messageInformation), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index Search
        /// </summary>
        /// <returns> </returns>
        [Route("")]
        [HttpPost]
        public ActionResult Index(string filter)
        {
            WorkerTypeEntity currentWorkerType = WorkerTypeRepository.Instance.GetById(CurrentWorker.WorkerTypeId);

            List<TeamEntity> teams = new List<TeamEntity>();

            var teamsRtn = new List<TeamEntity>();

            if (currentWorkerType.ProfileName.Equals("ADMINISTRADOR"))
            {
                //teams = TeamRepository.Instance.GetAllFromWorker(CurrentWorker.Id, CurrentFirm.Id);

                List<TeamEntity> teamsToAdd = TeamRepository.Instance.GetAllEntityFromFirm(CurrentFirm.Id);

                if (teamsToAdd != null)
                {
                    teams.AddRange(teamsToAdd);
                }
            }
            else if (currentWorkerType.ProfileName.Equals("LIDER"))
            {
                //teams = TeamRepository.Instance.GetAllFromWorker(CurrentWorker.Id, CurrentFirm.Id);

                List<TeamEntity> teamsToAdd = TeamRepository.Instance.GetBySponsor(CurrentFirm.Id);

                if (teamsToAdd != null)
                {
                    teams.AddRange(teamsToAdd);
                }
            }
            else
            {
                //teams = TeamRepository.Instance.GetAllFromWorker(CurrentWorker.Id, CurrentFirm.Id);
            }

            filter = filter.ToLowerInvariant().Trim();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var temp = from n in teams
                           where (n.TeamName.ToLowerInvariant().Trim().Contains(filter))
                           select n;

                teamsRtn = temp.ToList();
            }
            else
            {
                teamsRtn = teams;
            }

            ViewBag.Filter = filter;

            TeamEntity team = new TeamEntity();

            ViewBag.Team = team;

            return View(teamsRtn);
        }

    }
}