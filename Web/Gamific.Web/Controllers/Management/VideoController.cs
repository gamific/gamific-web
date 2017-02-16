using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/videos")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
    public class VideoController : BaseController
    {
        // GET: About
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("editar/{videoId:int}")]
        public ActionResult Edit(int videoId)
        {
            VideoEntity video = VideoRepository.Instance.GetById(videoId);

            return PartialView("_Edit", video);
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            VideoEntity video = new VideoEntity();

            return PartialView("_Edit", video);
        }

        [Route("remover/{videoId:int}")]
        public ActionResult Remove(int videoId)
        {

            List<VideoQuestionEntity> questions = VideoQuestionRepository.Instance.GetAllByVideo(videoId, CurrentFirm.Id);

            foreach (VideoQuestionEntity item in questions)
            {
                item.Status = GenericStatus.INACTIVE;

                VideoQuestionRepository.Instance.UpdateVideoQuestion(item);
            }

            VideoEntity video = VideoRepository.Instance.GetById(videoId);

            video.Status = GenericStatus.INACTIVE;

            VideoRepository.Instance.UpdateUpdate(video);

            return View("Index");
        }

        /// <summary>
        /// Salva as informações do video sendo criado
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(VideoEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    if (ModelState.IsValid)
                    {

                        if (entity.Id > 0)
                        {
                            entity.Status = GenericStatus.ACTIVE;
                            entity.FirmId = CurrentFirm.Id;
                            entity.UpdatedBy = CurrentUserId;

                            ValidateModel(entity);

                            VideoRepository.Instance.UpdateUpdate(entity);

                            Success("Video atualizado com sucesso.");
                            scope.Complete();
                        }
                        else
                        {
                            entity.Status = GenericStatus.ACTIVE;
                            entity.FirmId = CurrentFirm.Id;
                            entity.UpdatedBy = CurrentUserId;

                            ValidateModel(entity);

                            VideoRepository.Instance.CreateVideo(entity);

                            Success("Video criado com sucesso.");
                            scope.Complete();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar o video.");

                        return PartialView("_Edit", entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar o video.");

                return PartialView("_Edit", entity);
            }

            return new EmptyResult();
        }

        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<VideoEntity> searchResult = null;

                searchResult = VideoRepository.Instance.GetAllFromFirm(CurrentFirm.Id);

                var searchedQueryList = new List<VideoEntity>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.VideoTitle.ToLowerInvariant().Trim().Contains(filter))
                                        select n;

                    searchedQueryList = searchedQuery.ToList();
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
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.VideoTitle, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.VideoTitle, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}