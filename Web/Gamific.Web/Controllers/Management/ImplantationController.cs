using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/implantacao")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class ImplantationController : BaseController
    {
        // GET: Implantation
        [Route("")]
        public ActionResult Index()
        {
            ImplantationDTO implantation = new ImplantationDTO();
           
            ViewBag.Profiles = GetProfilesToSelect(new Profiles());
            ViewBag.Types = GetWorkerTypesToSelect(0);

            ViewBag.Sponsors = GetSponsorsToSelect();
            ViewBag.Episodes = GetEpisodesToSelect();

            ViewBag.SubTeams = JsonConvert.SerializeObject(new List<string>());

            ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Selected = implantation.Metric.Icon == i.ToString().Replace("_", "-") ? true : false,
                Text = i.GetType().GetMember(i.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name,
                Value = i.ToString().Replace("_", "-")
            }).ToList();

            ViewBag.WorkerTypes = WorkerTypeRepository.Instance.GetAllByGameId(CurrentFirm.ExternalId);

            return View("Index",implantation);
        }

        /// <summary>
        /// Cria a lista de seleção dos perfis
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetProfilesToSelect(Profiles selected)
        {
            IEnumerable<Profiles> profiles = Enum.GetValues(typeof(Profiles)).Cast<Profiles>();

            var query = from c in profiles
                        select new SelectListItem
                        {
                            Text = c.ToString(),
                            Value = c.ToString(),
                            Selected = c == selected
                        };

            return query.ToList();
        }
        /// <summary>
        /// Cria a lista de seleção dos perfis
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetWorkerTypesToSelect(int selected)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            List<WorkerTypeEntity> profiles = new List<WorkerTypeEntity>();

            profiles = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.Id);

            var query = from c in profiles
                        select new SelectListItem
                        {
                            Text = c.TypeName,
                            Value = c.Id.ToString(),
                            Selected = c.Id == selected
                        };

            return query.ToList();
        }
        /// <summary>
        /// Cria a lista de seleção dos responsaveis
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private List<SelectListItem> GetSponsorsToSelect(string selected = null)
        {
            List<WorkerDTO> sponsors = new List<WorkerDTO>();

            sponsors = WorkerRepository.Instance.GetAllFromFirmByProfile(CurrentFirm.ExternalId, Profiles.LIDER);

            var query = from sponsor in sponsors
                        select new SelectListItem
                        {
                            Text = sponsor.Name,
                            Value = sponsor.ExternalId,
                            Selected = sponsor.ExternalId == selected
                        };

            if (query == null)
            {
                return new List<SelectListItem>();
            }

            return query.ToList();
        }

        /// <summary>
        /// Cria a lista de seleção dos responsaveis
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private List<SelectListItem> GetEpisodesToSelect(string selected = null)
        {
            GetAllDTO episodes;

            episodes = EpisodeEngineService.Instance.GetByGameIdAndActiveIsTrue(CurrentFirm.ExternalId);

            var query = from episode in episodes.List.episode
                        select new SelectListItem
                        {
                            Text = episode.Name,
                            Value = episode.Id,
                            Selected = episode.Id == selected
                        };

            if (query == null)
            {
                return new List<SelectListItem>();
            }

            return query.ToList();
        }
    }
    
}