using System;
using System.Transactions;
using System.Web.Mvc;
using Vlast.Util.Instrumentation;
using Vlast.Gamific.Model.Account.Repository;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Account.DTO;
using Vlast.Gamific.Account.Model;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Util.Global;
using System.Web;
using Vlast.Gamific.Model.Media.Domain;
using System.IO;
using System.Drawing;
using Vlast.Gamific.Model.Media.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Services.Engine;

namespace Vlast.Gamific.Web.Controllers.Account
{
    [CustomAuthorize]
    [RoutePrefix("admin/usuario/perfil")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRATOR,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE,JOGADOR")]
    public class AccountProfileController : BaseController
    {

        [Route("editar")]
        public ActionResult Edit()
        {
            UserProfileEntity user = UserProfileRepository.Instance.GetById(CurrentUserId);

            ViewBag.LogoId = CurrentWorker.LogoId;

            return View("Edit", user);
        }

        [Route("editarSenha")]
        public ActionResult PasswordEdit()
        {
            PasswordDTO userPassword = new PasswordDTO();

            userPassword.UserId = CurrentUserId;

            return View("PasswordEdit", userPassword);
        }

        /// <summary>
        /// Salva a nova senha do perfil
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("salvarSenha")]
        [HttpPost]
        public ActionResult Save(PasswordDTO entity)
        {
            var complete = true;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (ModelState.IsValid)
                    {
                        
                        if (!entity.NewPassword.Equals(entity.NewPasswordConfirmation))
                        {
                            Error("As duas senhas digitadas não conferem.");
                            complete = false;
                        }

                        UserAccountEntity user = AccountRepository.Instance.GetById(CurrentUserId);

                        if (!PasswordUtils.ValidatePassword(entity.CurrentPassword, user.SecurityStamp, user.PasswordHash))
                        {
                            Error("A senha atual digitada não confere com a vigente.");
                            complete = false;
                        }

                        NewRequest request = new NewRequest();

                        request.Password = entity.NewPassword;
                        request.Username = user.UserName;
                        request.Name = user.UserName;

                        AccountHandler.UpdateUser(request);

                        if(complete)
                        {
                            Success("Senha atualizada com sucesso.");
                        }

                        scope.Complete();
                    }
                    else
                    {
                        Warning("Alguns campos são obrigatórios para salvar a senha.");

                        return View("PasswordEdit", entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar salvar a senha.", ex);
            }

            if (complete)
            {
                if (IsSystemAdmin)
                {
                    return Redirect("/admin/empresas/");
                }
                else
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
            }
            else
            {
                return Redirect("/admin/usuario/perfil/editarSenha");
            }

        }

        /// <summary>
        /// Salva as informações do perfil
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logoUpload"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(UserProfileEntity entity, HttpPostedFileBase logoUpload)
        {

            WorkerEntity currentWorker = CurrentWorker;

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

                            //if (currentWorker.LogoId <= 0)
                            {
                                imageSaving = ImageRepository.Instance.CreateImage(imageSaving);
                            }
                            //else
                            {
                                //imageSaving.Id = currentWorker.LogoId;
                            }

                            ImageRepository.Instance.SaveOrReplaceLogo(imageSaving.Id, cover);

                            currentWorker.LogoId = imageSaving.Id;

                            WorkerRepository.Instance.UpdateWorker(currentWorker);
                        }

                        ViewBag.LogoId = currentWorker.LogoId;

                        ValidateModel(entity);

                        entity.CPF = entity.CPF.Replace(".", "").Replace("/", "");

                        PlayerEngineDTO player = PlayerEngineService.Instance.GetById(currentWorker.ExternalId);
                        
                        player.LogoId = currentWorker.LogoId;
                        player.LogoPath = CurrentURL + player.LogoId;
                        player.Nick = entity.Name;
                        player.Cpf = entity.CPF;

                        player = PlayerEngineService.Instance.CreateOrUpdate(player);

                        UserProfileRepository.Instance.UpdateUserProfile(entity);

                        Success("Perfil atualizado com sucesso.");
                        scope.Complete();
                    }
                    else
                    {
                        Warning("Alguns campos são obrigatórios para salvar o perfil.");
                        return View("Edit", entity);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.LogoId = currentWorker.LogoId;

                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar salvar o perfil.", ex);
            }

            ViewBag.LogoId = currentWorker.LogoId;

            if (IsSystemAdmin)
            {
                return Redirect("/admin/empresas/");
            }
            else
            {
                if (CurrentWorkerType.ProfileName.Equals(Profiles.ADMINISTRADOR))
                {
                    return Redirect("/public/dashboard");
                }
                else
                {
                    return Redirect("/public/resultadosIndividuais");
                }
            }
        }

    }
}