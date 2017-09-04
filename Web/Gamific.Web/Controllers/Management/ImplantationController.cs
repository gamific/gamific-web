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
using Vlast.Util.Instrumentation;

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
            EpisodeEngineDTO episode = new EpisodeEngineDTO();
            episode.Active = true;

            ViewBag.NumberOfWorkerTypes = WorkerTypeRepository.Instance.GetCountFromFirm(CurrentFirm.ExternalId);

            ViewBag.WorkerType = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.ExternalId);

            ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Text = i.ToString(),
                Value = i.ToString()
            }).ToList();
           
            return View("Index",episode);
        }

        [Route("salvarCampanha")]
        [HttpPost]
        public ActionResult SaveEpisode(EpisodeEngineDTO episode)
        {
            try
            {
                
                if (ModelState.IsValid)
                {

                    if (episode.Active == false)
                    {
                        Success("Campanha não pode ser alterada.");
                    }
                    else
                    {
                        if (episode.Id == null || episode.Id.Equals(""))
                        {
                            episode.Active = true;
                        }


                        episode.initDate = episode.initDateAux.Ticks;

                        episode.finishDate = episode.finishDateAux.Ticks;

                        episode.GameId = CurrentFirm.ExternalId;

                        ValidateModel(episode);

                        EpisodeEngineDTO newEpisode = EpisodeEngineService.Instance.CreateOrUpdate(episode);

                        if (newEpisode == null)
                        {
                            throw new Exception(".");
                        }

                        Success("Campanha atualizada com sucesso.");
                        
                        return View("Index", episode);
                    }




                }
                else
                {
                   
                    episode.Active = true;
                   
                      ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
                      {
                          Text = i.ToString(),
                          Value = i.ToString()
                      }).ToList();
                     ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar a Campanha.");
                 
                    return PartialView("Index", episode);

                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Error("Erro ao atualizar campanha" + ex.Message);
                return View("Index");
            }

            return new EmptyResult();
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