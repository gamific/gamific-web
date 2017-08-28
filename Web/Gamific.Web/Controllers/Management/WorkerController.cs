using Aspose.Cells;
using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Vlast.Gamific.Account.Model;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Account.DTO;
using Vlast.Gamific.Model.Account.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Media.Domain;
using Vlast.Gamific.Model.Media.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Account;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/funcionarios")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class WorkerController : BaseController
    {
        static int rowsCount;
        // GET: Worker
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.NumberOfWorkers = WorkerRepository.Instance.GetCountFromFirm(CurrentFirm.Id);

            return View();
        }

        [Route("editar/{workerId:int}")]
        public ActionResult Edit(int workerId)
        {
            WorkerDTO worker = WorkerRepository.Instance.GetDTOById(workerId);
            PlayerEngineDTO workerEngine = PlayerEngineService.Instance.GetById(worker.ExternalId);
            WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(worker.WorkerTypeId);

            worker.Role = workerType.ProfileName.ToString();
            worker.ProfileName = workerType.ProfileName;

            worker.TotalXp = (int)workerEngine.Xp;

            ViewBag.Types = GetWorkerTypesToSelect(worker.WorkerTypeId);

            return PartialView("_Edit", worker);
        }


        [Route("cadastrar")]
        public ActionResult Create()
        {
            WorkerDTO worker = new WorkerDTO();

            ViewBag.Types = GetWorkerTypesToSelect(0);

            return PartialView("_Edit", worker);
        }

        [Route("remover/{workerId:int}")]
        public ActionResult Remove(int workerId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    WorkerEntity worker = WorkerRepository.Instance.GetById(workerId);
                    worker.Status = GenericStatus.INACTIVE;
                    WorkerRepository.Instance.UpdateWorker(worker);
                    //WorkerRepository.Instance.RemoveWorker(worker.Id);

                    //AccountRepository.Instance.RemoveAccount(worker.UserId);

                    //PlayerEngineService.Instance.DeleteById(worker.ExternalId);
                    PlayerEngineDTO player = PlayerEngineService.Instance.GetById(worker.ExternalId);
                    player.Active = false;
                    PlayerEngineService.Instance.CreateOrUpdate(player);

                    scope.Complete();
                }
            }
            catch(Exception e)
            {
                Error("Ocorreu um erro ao remover.");
            }

            ViewBag.NumberOfWorkers = WorkerRepository.Instance.GetCountFromFirm(CurrentFirm.Id);

            return View("Index");
        }

        /// <summary>
        /// Salva as informações do funcionario sendo criado
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logoUpload"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(WorkerDTO entity, HttpPostedFileBase logoUpload)
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

                            //if (entity.LogoId > 0)
                            {
                                //imageSaving.Id = entity.LogoId;
                            }
                            //else
                            {
                                imageSaving = ImageRepository.Instance.CreateImage(imageSaving);
                            }

                            ImageRepository.Instance.SaveOrReplaceLogo(imageSaving.Id, cover);

                            entity.LogoId = imageSaving.Id;
                        }

                        if (entity.IdWorker > 0)
                        {
                            PlayerEngineDTO player = PlayerEngineService.Instance.GetById(entity.ExternalId);
                            WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(entity.WorkerTypeId);

                            UserProfileEntity userProfile = new UserProfileEntity();

                            userProfile.Id = entity.IdUser;
                            userProfile.Name = entity.Name;
                            userProfile.Email = entity.Email;
                            userProfile.CPF = entity.Cpf.Replace(".", "").Replace("-", "");
                            userProfile.Phone = entity.Phone;

                            ValidateModel(userProfile);

                            WorkerEntity worker = new WorkerEntity();

                            worker.Status = GenericStatus.ACTIVE;
                            worker.FirmId = CurrentFirm.Id;
                            worker.ExternalFirmId = CurrentFirm.ExternalId;
                            worker.WorkerTypeId = entity.WorkerTypeId;
                            worker.UserId = entity.IdUser;
                            worker.Id = entity.IdWorker;
                            worker.LogoId = entity.LogoId;
                            worker.UpdatedBy = CurrentUserId;
                            worker.ExternalId = player.Id;

                            ValidateModel(worker);

                            WorkerRepository.Instance.UpdateWorker(worker);

                            UserProfileRepository.Instance.UpdateUserProfile(userProfile);

                            UserAccountEntity acc = AccountRepository.Instance.GetById(entity.IdUser);

                            acc.UserName = userProfile.Email;

                            AccountRepository.Instance.Update(acc);

                            player.Nick = userProfile.Name;
                            player.Role = workerType.ProfileName.ToString();
                            player.LogoId = worker.LogoId;
                            player.Xp = entity.TotalXp;
                            player.Email = entity.Email;
                            player.Cpf = entity.Cpf;
                            player.LogoPath = CurrentURL + player.LogoId;
                            player.Active = true;
                            player.GameId = CurrentFirm.ExternalId;
                            PlayerEngineService.Instance.CreateOrUpdate(player);

                            Success("Funcionário atualizado com sucesso.");
                            scope.Complete();
                        }
                        else
                        {
                            WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(entity.WorkerTypeId);

                            NewRequest request = new NewRequest();
                            AuthResult result = new AuthResult();

                            request.Cpf = entity.Cpf.Replace("-", "").Replace(".", "");
                            request.Name = entity.Name;
                            request.Phone = entity.Phone;
                            request.Email = entity.Email;
                            request.Username = entity.Email;
                            request.Password = request.Cpf;

                            result = AccountHandler.CreateFirmUser(request, Roles.WORKER);

                            if (!AuthStatus.OK.Equals(result.AuthStatus))
                            {
                                Error(AccountHelper.HandleError(result));

                                ViewBag.Types = GetWorkerTypesToSelect(entity.WorkerTypeId);

                                ModelState.AddModelError("", "Ocorreu um erro ao salvar o funcionário.");

                                return PartialView("_Edit", entity);
                            }

                            WorkerEntity worker = new WorkerEntity();

                            worker.Status = GenericStatus.ACTIVE;
                            worker.FirmId = CurrentFirm.Id;
                            worker.ExternalFirmId = CurrentFirm.ExternalId;
                            worker.UserId = result.UserId;
                            worker.WorkerTypeId = entity.WorkerTypeId;
                            worker.LogoId = entity.LogoId;
                            worker.UpdatedBy = CurrentUserId;

                            ValidateModel(worker);

                            PlayerEngineDTO player = PlayerEngineService.Instance.CreateOrUpdate(
                                new PlayerEngineDTO
                                {
                                    GameId = worker.ExternalFirmId,
                                    Nick = request.Name,
                                    Role = workerType.ProfileName.ToString(),
                                    Level = 1,
                                    LogoId = worker.LogoId,
                                    Cpf = entity.Cpf.Replace(".", "").Replace("-", ""),
                                    Email = entity.Email,
                                    Xp = 1,
                                    LogoPath = CurrentURL + worker.LogoId,
                                    Active = true
                                });

                            worker.ExternalId = player.Id;

                            WorkerRepository.Instance.CreateWorker(worker);

                            Success("Funcionário criado com sucesso.");
                            scope.Complete();
                        }
                    }
                    else
                    {
                        ViewBag.Types = GetWorkerTypesToSelect(entity.WorkerTypeId);

                        ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar o funcionário.");

                        return PartialView("_Edit", entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Error("Houve um erro ao salvar funcionário.");

                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar o funcionário.");

                ViewBag.Types = GetWorkerTypesToSelect(entity.WorkerTypeId);

                return PartialView("_Edit", entity);
            }

            return new EmptyResult();
        }

        [Route("search/{numberOfWorkers:int}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, int numberOfWorkers)
        {
            numberOfWorkers = WorkerRepository.Instance.GetCountFromFirm(CurrentFirm.Id);

            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];

                List<WorkerDTO> searchResult = null;

                searchResult = WorkerRepository.Instance.GetAllFromFirm(CurrentFirm.Id, jqueryTableRequest.Page, 10);

                var searchedQueryList = new List<WorkerDTO>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.Cpf != null && n.Cpf.ToLowerInvariant().Trim().Contains(filter)) ||
                                        n.Name.ToLowerInvariant().Trim().Contains(filter) ||
                                        n.Email.ToLowerInvariant().Trim().Contains(filter)
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
                        RecordsTotal = numberOfWorkers,
                        RecordsFiltered = numberOfWorkers,
                        Data = searchedQueryList.Select(r => new string[] { r.Name + ";" + r.LogoId.ToString(), r.Email, r.WorkerTypeName, r.IdWorker.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = numberOfWorkers,
                        RecordsFiltered = numberOfWorkers,
                        Data = searchedQueryList.Select(r => new string[] { r.Name + ";" + r.LogoId.ToString(), r.Email, r.WorkerTypeName, r.IdWorker.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
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
        /// Abre o popup de cadastrar funcionarios via arquivo
        /// </summary>
        /// <returns></returns>
        [Route("cadastrarFuncionariosArquivo")]
        public ActionResult CreateWorkersArchive()
        {
            return PartialView("_WorkersArchive");
        }

        /// <summary>
        /// Abre o popup de cadastrar funcionarios via arquivo
        /// </summary>
        /// <returns></returns>
        [Route("changePassword/{playerId:int}")]
        public ActionResult ChangePassword(int playerId)
        {
            WorkerDTO worker = WorkerRepository.Instance.GetDTOById(playerId);
            //PlayerEngineDTO player = PlayerEngineService.Instance.GetById(playerId);

            AccountHandler.ChangePassword(worker.Email, "Gamific123");

            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna a planiha de funcionarios para ser preenchida
        /// </summary>
        /// <returns></returns>
        [Route("baixarPlanilha")]
        [HttpGet]
        public FileContentResult DownloadPlan()
        {

            var workbook = new Workbook();

            var worksheetResults = workbook.Worksheets[0];

            rowsCount = 5000;

            worksheetResults.Cells.HideColumns(5, 16384);
            worksheetResults.Cells.HideRows(rowsCount, 1048576);
            worksheetResults.Cells.StandardWidth = 35.0;

            worksheetResults.Name = "Workers";

            var cellsResults = worksheetResults.Cells;

            cellsResults["A1"].PutValue("Nome");
            cellsResults["B1"].PutValue("Email");
            cellsResults["C1"].PutValue("Telefone");
            cellsResults["D1"].PutValue("CPF");
            cellsResults["E1"].PutValue("Função");

            List<string> workerTypeNames = new List<string>();

            List<WorkerTypeEntity> workerTypes = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.Id);

            foreach (WorkerTypeEntity workerType in workerTypes)
            {
                workerTypeNames.Add(workerType.TypeName);
            }

            var flatListProfiles = string.Join(",", workerTypeNames.ToArray());

            var validations = worksheetResults.Validations;

            var validationName = validations[validations.Add()];
            validationName.Type = ValidationType.TextLength;
            validationName.Operator = OperatorType.None;
            validationName.InCellDropDown = false;
            validationName.ShowError = true;
            validationName.AlertStyle = ValidationAlertType.Stop;
            CellArea areaNames;
            areaNames.StartRow = 1;
            areaNames.EndRow = rowsCount;
            areaNames.StartColumn = 0;
            areaNames.EndColumn = 0;
            validationName.AreaList.Add(areaNames);

            var validationEmail = validations[validations.Add()];
            validationEmail.Type = ValidationType.TextLength;
            validationEmail.Operator = OperatorType.None;
            validationEmail.InCellDropDown = false;
            validationEmail.ShowError = true;
            validationEmail.AlertStyle = ValidationAlertType.Stop;
            CellArea areaEmail;
            areaEmail.StartRow = 1;
            areaEmail.EndRow = rowsCount;
            areaEmail.StartColumn = 1;
            areaEmail.EndColumn = 1;
            validationEmail.AreaList.Add(areaEmail);

            var validationPhone = validations[validations.Add()];
            validationPhone.Type = ValidationType.TextLength;
            validationPhone.Operator = OperatorType.None;
            validationPhone.InCellDropDown = false;
            validationPhone.ShowError = true;
            validationPhone.AlertStyle = ValidationAlertType.Stop;
            CellArea areaPhone;
            areaPhone.StartRow = 1;
            areaPhone.EndRow = rowsCount;
            areaPhone.StartColumn = 2;
            areaPhone.EndColumn = 2;
            validationPhone.AreaList.Add(areaPhone);

            var validationCPF = validations[validations.Add()];
            validationCPF.Type = ValidationType.TextLength;
            validationCPF.Operator = OperatorType.None;
            validationCPF.InCellDropDown = false;
            validationCPF.ShowError = true;
            validationCPF.AlertStyle = ValidationAlertType.Stop;
            CellArea areaCPF;
            areaCPF.StartRow = 1;
            areaCPF.EndRow = rowsCount;
            areaCPF.StartColumn = 3;
            areaCPF.EndColumn = 3;
            validationCPF.AreaList.Add(areaCPF);

            var validationProfile = validations[validations.Add()];
            validationProfile.Type = ValidationType.List;
            validationProfile.Operator = OperatorType.Between;
            validationProfile.InCellDropDown = true;
            validationProfile.ShowError = true;
            validationProfile.Formula1 = flatListProfiles;
            validationProfile.AlertStyle = ValidationAlertType.Stop;
            CellArea areaProfile;
            areaProfile.StartRow = 1;
            areaProfile.EndRow = rowsCount;
            areaProfile.StartColumn = 4;
            areaProfile.EndColumn = 4;
            validationProfile.AreaList.Add(areaProfile);

            MemoryStream ms = new MemoryStream();

            ms = workbook.SaveToStream();

            return File(ms.ToArray(), "application/vnd.ms-excel");
        }

        /// <summary>
        /// Salva novos funcionarios via arquivo
        /// </summary>
        /// <param name="resultsArchive"></param>
        /// <returns></returns>
        [Route("salvarResultadoArquivo")]
        [HttpPost]
        public ActionResult SaveWorkersArchive(HttpPostedFileBase workersArchive)
        {
            try
            {
                string gameId = CurrentFirm.ExternalId;

                workersArchive.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), workersArchive.FileName));

                string path = Path.Combine(Server.MapPath("~/App_Data"), workersArchive.FileName);

                var archive = new ExcelQueryFactory(path);

                var rows = from x in archive.WorksheetRange("A1", "E" + rowsCount, "Workers")
                           select x;

                foreach (var row in rows)
                {
                    if (!string.IsNullOrWhiteSpace(row[0].ToString()) && !string.IsNullOrWhiteSpace(row[1].ToString()) && !string.IsNullOrWhiteSpace(row[2].ToString()) && !string.IsNullOrWhiteSpace(row[4].ToString()))
                    {
                        NewRequest request = new NewRequest();

                        AuthResult result = new AuthResult();

                        request.Cpf = row[3].ToString();
                        request.Name = row[0].ToString();
                        request.Phone = row[2].ToString();
                        request.Password = "Gamific123";
                        request.Email = row[1].ToString();
                        request.Username = row[1].ToString();

                        result = AccountHandler.CreateFirmUser(request, Roles.WORKER);

                        WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetByGameIdAndTypeName(gameId, row[4].ToString());

                        if (AuthStatus.OK.Equals(result.AuthStatus))
                        {
                            WorkerEntity worker = new WorkerEntity();

                            worker.Status = GenericStatus.ACTIVE;
                            worker.FirmId = CurrentFirm.Id;
                            worker.UserId = result.UserId;
                            worker.ExternalFirmId = CurrentFirm.ExternalId;
                            worker.WorkerTypeId = workerType.Id;
                            worker.UpdatedBy = CurrentUserId;

                            ValidateModel(worker);

                            PlayerEngineDTO dto = new PlayerEngineDTO
                            {
                                Level = 1,
                                Xp = 1,
                                Nick = request.Name,
                                Role = workerType.ProfileName.ToString(),
                                GameId = worker.ExternalFirmId,
                                LogoId = worker.LogoId,
                                Cpf = request.Cpf,
                                Email = request.Email,
                                LogoPath = CurrentURL + worker.LogoId,
                                Active = true
                            };

                            dto = PlayerEngineService.Instance.CreateOrUpdate(dto);

                            worker.ExternalId = dto.Id;

                            worker = WorkerRepository.Instance.CreateWorker(worker);
                        }
                    }
                }

                Success("Funcionários criados com sucesso.");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar os funcionários via arquivo.");

                return PartialView("_WorkersArchive");
            }

            return new EmptyResult();
        }

    }
}