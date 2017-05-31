using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Public;
using Vlast.Gamific.Web.Controllers.Util;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;


namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/quizAssociate")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class QuizAssociateController : BaseController
    {
        int idPrincipal = 0;

        [Route("associate/{id}")]
        public ActionResult Index(int id)
        {
            idPrincipal = id;
            return Redirect("/acesso/login");
        }


        [Route("associate")]
        public ActionResult associate(List<ParserDTO> dto)
        {
            try
            {
                if (dto.Count > 0)
                {


                    using (TransactionScope delete = new TransactionScope(TransactionScopeOption.Required))
                    {
                        QuizQuestionService.Instance.deleteByAssociated(dto[0].IdPrincipal);
                        delete.Complete();
                    }
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        for (int item = 0; item < dto.Count; item++)
                        {
                            QuizQuestionEntity association = new QuizQuestionEntity();
                            association.IdQuestion = dto[item].Id;
                            association.IdQuiz = dto[item].IdPrincipal;
                            association.Ordination = item;
                            QuizQuestionService.Instance.Create(association);
                        }

                        scope.Complete();
                    }
                    return Json(new { status = "sucess", message = "Registro removido com sucesso!" });
                }
                else
                {
                    return Json(new { status = "sucess", message = "Não há registros a serem associados!" });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { status = "error", message = "Ocorreu um problema!" });
            }


        }

       

    }
}
