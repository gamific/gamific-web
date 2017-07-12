using System;
using System.Transactions;
using System.Web.Mvc;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Model.Account.DTO;
using System.Collections.Generic;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Account;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Util.Instrumentation;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;
using System.Web;
using System.IO;
using System.Drawing;
using Vlast.Gamific.Model.Media.Repository;
using Vlast.Gamific.Model.Media.Domain;
using Vlast.Gamific.Account.Model;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/empresas")]
    public class FirmController : BaseController
    {

        [Route("search")]
        [CustomAuthorize(Roles = "ADMINISTRATOR")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<DataEntity> searchResult = null;

                searchResult = DataRepository.Instance.GetAll(jqueryTableRequest.Page, 10);

                var searchedQueryList = new List<DataEntity>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where n.FirmName.ToLowerInvariant().Trim().Contains(filter) ||
                                        n.CompanyName.ToLowerInvariant().Trim().Contains(filter)
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
                        Data = searchedQueryList.Select(r => new string[] { r.FirmName, r.Status.ToString(), r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.FirmName, r.Status.ToString(), r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRATOR,ADMINISTRADOR")]
        public ActionResult Index()
        {
            if (IsSystemAdmin)
            {
                return View("Index");
            }
            else if (CurrentFirm != null)
            {
                if (CurrentWorkerType.ProfileName.Equals(Profiles.ADMINISTRADOR))
                {
                    return Redirect("/public/dashboard");
                }
                else
                {
                    return Redirect("/public/ranking");
                }
            }

            return new HttpUnauthorizedResult();
        }

        [Route("visualizar/{firmId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
        public ActionResult Show(int firmId)
        {
            DataEntity firm = DataRepository.Instance.GetById(firmId);

            return View("Show", firm);
        }

        [Route("editar/{firmId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRATOR,ADMINISTRADOR")]
        public ActionResult Edit(int firmId)
        {
            FirmDTO dto = new FirmDTO();

            DataEntity firm = DataRepository.Instance.GetById(firmId);

            dto.DataInfo = firm;

            ViewBag.Status = GetStatusToSelect(firm.Status == GenericStatus.ACTIVE ? 1 : 0);

            return View("Edit", dto);
        }

        [Route("remover/{firmId:int}")]
        [CustomAuthorize(Roles = "ADMINISTRATOR")]
        public ActionResult Remove(int firmId)
        {
            DataEntity firm = DataRepository.Instance.GetById(firmId);

            firm.Status = GenericStatus.INACTIVE;

            ViewBag.Status = GetStatusToSelect(firm.Status == GenericStatus.ACTIVE ? 1 : 0);

            DataRepository.Instance.UpdateFirm(firm);

            return View("Index");
        }

        [Route("cadastrar")]
        [CustomAuthorize(Roles = "ADMINISTRATOR")]
        public ActionResult Create()
        {
            FirmDTO firm = new FirmDTO();

            return PartialView("Create", firm);
        }

        /// <summary>
        /// Salva as informações da empresa sendo criada
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logoUpload"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRATOR,ADMINISTRADOR")]
        public ActionResult Save(FirmDTO entity, HttpPostedFileBase logoUpload)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (ModelState.IsValid)
                    {
                        ImageEntity imageSaving = new ImageEntity();
                        if (logoUpload != null && logoUpload.ContentLength > 0)
                        {

                            imageSaving.Status = GenericStatus.ACTIVE;
                            imageSaving.UpdatedBy = CurrentUserId;

                            byte[] cover = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                logoUpload.InputStream.CopyTo(memoryStream);
                                if (memoryStream.Length > 0)
                                {
                                    using (Image image = Bitmap.FromStream(memoryStream))
                                    {
                                        logoUpload.InputStream.CopyTo(memoryStream);
                                        if (memoryStream.Length > 0)
                                        {
                                            cover = memoryStream.ToArray();
                                        }
                                    }
                                }
                            }

                            if (entity.DataInfo.LogoId > 0)
                            {
                                imageSaving.Id = entity.DataInfo.LogoId;
                            }
                            else
                            {
                                imageSaving = ImageRepository.Instance.CreateImage(imageSaving);
                            }

                            ImageRepository.Instance.SaveOrReplaceLogo(imageSaving.Id, cover);

                            entity.DataInfo.LogoId = imageSaving.Id;
                        }

                        entity.DataInfo.UpdatedBy = CurrentUserId;

                        if (entity.DataInfo.Id > 0)
                        {

                            if (entity.Status == 0)
                            {
                                entity.DataInfo.Status = GenericStatus.INACTIVE;
                            }
                            else
                            {
                                entity.DataInfo.Status = GenericStatus.ACTIVE;
                            }

                            ValidateModel(entity.DataInfo);

                            GameEngineDTO game = new GameEngineDTO
                            {
                                Adress = entity.DataInfo.Adress,
                                City = entity.DataInfo.City,
                                LogoId = entity.DataInfo.LogoId,
                                Name = entity.DataInfo.FirmName,
                                Neighborhood = entity.DataInfo.Neighborhood,
                                Phone = entity.DataInfo.Phone,
                                Id = entity.DataInfo.ExternalId,
                                LogoPath = CurrentURL + entity.DataInfo.LogoId
                            };

                            try
                            {
                                game = GameEngineService.Instance.CreateOrUpdate(game);
                            }
                            catch (Exception e)
                            {
                                Logger.LogError(e.Message);

                            }


                            List<WorkerDTO> workers = WorkerRepository.Instance.GetAllFromFirm(entity.DataInfo.Id);

                            if (entity.DataInfo.Status == GenericStatus.ACTIVE)
                            {
                                foreach (WorkerDTO item in workers)
                                {
                                    UserAccountEntity acc = AccountRepository.Instance.GetById(item.IdUser);

                                    acc.Status = GenericStatus.ACTIVE;

                                    AccountRepository.Instance.Update(acc);
                                }
                            }
                            else
                            {
                                foreach (WorkerDTO item in workers)
                                {
                                    UserAccountEntity acc = AccountRepository.Instance.GetById(item.IdUser);

                                    acc.Status = GenericStatus.INACTIVE;

                                    AccountRepository.Instance.Update(acc);
                                }
                            }

                            DataRepository.Instance.UpdateFirm(entity.DataInfo);

                        }
                        else
                        {
                            if (!entity.Password.Equals(entity.PasswordConfirmation))
                            {
                                Warning("As duas senhas digitadas não conferem.");
                            }

                            NewRequest request = new NewRequest();

                            AuthResult result = new AuthResult();

                            request.Cpf = entity.ProfileInfo.CPF;
                            request.Name = entity.ProfileInfo.Name;
                            request.Phone = entity.ProfileInfo.Phone;
                            request.Password = entity.Password;
                            request.Email = entity.ProfileInfo.Email;
                            request.Username = entity.Username;

                            result = AccountHandler.CreateFirmUser(request, Roles.WORKER);

                            if (!AuthStatus.OK.Equals(result.AuthStatus))
                            {
                                Error(AccountHelper.HandleError(result));

                                return View("Create", entity);
                            }

                            ValidateModel(entity.DataInfo);

                            GameEngineDTO game = new GameEngineDTO
                            {
                                Adress = entity.DataInfo.Adress,
                                City = entity.DataInfo.City,
                                LogoId = entity.DataInfo.LogoId,
                                Name = entity.DataInfo.FirmName,
                                Neighborhood = entity.DataInfo.Neighborhood,
                                Phone = entity.DataInfo.Phone,
                                Id = entity.DataInfo.ExternalId
                            };
                            game = GameEngineService.Instance.CreateOrUpdate(game, "victor@duplov.com.br");



                            entity.DataInfo.ExternalId = game.Id;

                            entity.DataInfo.Status = GenericStatus.ACTIVE;

                            entity.DataInfo = DataRepository.Instance.CreateFirm(entity.DataInfo);

                            WorkerEntity worker = new WorkerEntity();

                            WorkerTypeEntity workerType = new WorkerTypeEntity();

                            workerType.FirmId = entity.DataInfo.Id;
                            workerType.ProfileName = Profiles.ADMINISTRADOR;
                            workerType.Status = GenericStatus.ACTIVE;
                            workerType.TypeName = "ADMINISTRADOR";
                            workerType.UpdatedBy = CurrentUserId;

                            workerType = WorkerTypeRepository.Instance.CreateWorkerType(workerType);

                            worker.WorkerTypeId = workerType.Id;
                            worker.UserId = result.UserId;
                            worker.UpdatedBy = CurrentUserId;
                            worker.FirmId = entity.DataInfo.Id;
                            worker.Status = GenericStatus.ACTIVE;
                            worker.LogoId = entity.DataInfo.LogoId;

                            PlayerEngineDTO player = new PlayerEngineDTO
                            {
                                Nick = request.Name,
                                Xp = 1,
                                Level = 1,
                                Role = workerType.TypeName,
                                GameId = worker.ExternalFirmId,
                                LogoId = worker.LogoId,
                                Email = entity.ProfileInfo.Email,
                                Cpf = entity.ProfileInfo.CPF
                            };
                            player = PlayerEngineService.Instance.CreateOrUpdate(player, "victor@duplov.com.br");

                            worker.ExternalId = player.Id;
                            worker.ExternalFirmId = game.Id;

                            worker = WorkerRepository.Instance.CreateWorker(worker);
                        }

                        Success("Empresa salva com sucesso.");
                        scope.Complete();

                    }
                    else
                    {
                        Warning("Alguns campos são obrigatórios para salvar a empresa.");

                        if (entity.DataInfo.Id > 0)
                        {
                            ViewBag.Status = GetStatusToSelect(entity.DataInfo.Status == GenericStatus.ACTIVE ? 1 : 0);

                            return View("Edit", entity);
                        }
                        else
                        {
                            return View("Create", entity);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar salvar a empresa.", ex);

                if (entity.DataInfo.Id > 0)
                {
                    ViewBag.Status = GetStatusToSelect(entity.DataInfo.Status == GenericStatus.ACTIVE ? 1 : 0);

                    return View("Edit", entity);
                }
                else
                {
                    return View("Create", entity);
                }
            }

            return View("Index");
        }

        /// <summary>
        /// Cria a lista de seleção dos status
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetStatusToSelect(int selected)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            List<SelectListItem> rtnList = new List<SelectListItem>();


            SelectListItem item = new SelectListItem
            {
                Text = "Ativo",
                Value = "1",
                Selected = 1 == selected
            };

            SelectListItem item1 = new SelectListItem
            {
                Text = "Inativo",
                Value = "0",
                Selected = 0 == selected
            };

            rtnList.Add(item);
            rtnList.Add(item1);

            return rtnList;
        }

        /// <summary>
        /// Salva as informações da empresa 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logoUpload"></param>
        /// <returns></returns>
        [Route("atualizar")]
        [HttpPost]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRATOR,ADMINISTRADOR")]
        public ActionResult SaveFirm(DataEntity entity)
        {



            DataRepository.Instance.UpdateFirm(entity);

            GameEngineDTO game = new GameEngineDTO
            {
                Adress = entity.Adress,
                City = entity.City,
                Neighborhood = entity.Neighborhood,
                Phone = entity.Phone,
                Id = entity.ExternalId,
                Name = entity.CompanyName,
                LogoId = entity.LogoId,
                Description = entity.Cnpj


            };



            GameEngineService.Instance.CreateOrUpdate(game);
            Success("Dados da empresa alterados com sucesso!");
            return Redirect("/public/dashboard");
        }




    }
}