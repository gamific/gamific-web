﻿using System;
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
using Vlast.Gamific.Model.Account.Repository;
using Newtonsoft.Json;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/relatorio")]
    public class ReportAdminController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {

           // ViewBag.Games = DataRepository.Instance.GetAllOfGameIdAndGameName();

            return View();
        }


        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarEmpresa")]
        [HttpGet]
        public ActionResult SearchEpisodes()
        {
            List<DataEntity> all = DataRepository.Instance.GetAllOfGameIdAndGameName();

            return Json(JsonConvert.SerializeObject(all), JsonRequestBehavior.AllowGet);
        }

        [Route("create/{initDateMonth}/{initDateDay}/{initDateYear}/{finishDateMonth}/{finishDateDay}/{finishDateYear}/{gameId}")]
        public string CreateTableUsers(string initDateMonth, string initDateDay, string initDateYear, string finishDateMonth, string finishDateDay, string finishDateYear, string gameId)
        {
            var initDate = DateTime.Parse(initDateYear + "-" + initDateMonth + "-" + initDateDay + " 00:00:00");
            var finishDate = DateTime.Parse(finishDateYear + "-" + finishDateMonth + "-" + finishDateDay + " 00:00:00");
            List<UserAccountEntity> accontEntityResults = AccountRepository.Instance.GetByInitDate(initDate, finishDate);
            List<AccountDevicesEntity> accontDeviceEntitys = AccountDevicesRepository.Instance.FindAll();
            List<UserProfileEntity> userProfileEntitys = UserProfileRepository.Instance.GetUsersByDate(initDate, finishDate);


            if (gameId != null || gameId != "")
            {
                

                string util = "<table>";
                util = util + "<tr> <th>Nome</th> <th>Email</th> <th>Empresa</th> <th>Web</th> <th>Mobile</th> </tr>";
                foreach (UserProfileEntity userProfileEntity in userProfileEntitys)
                {
                    util = util + "<tr>";
                    PlayerEngineDTO player = null;
                    AccountDevicesEntity device = null;
                    bool token = true;
                    try
                    {
                        player = PlayerEngineService.Instance.GetByEmail(userProfileEntity.Email, true);
                    }
                    catch (Exception ex)
                    {
                        token = false;
                    }

                    try
                    {
                        device = AccountDevicesRepository.Instance.FindByPlayerIdDescending(player.Id).First();
                    }
                    catch (Exception ex)
                    {
                        device = null;
                    }


                    util = util + "<th>" + userProfileEntity.Name + "</th>";
                    util = util + "<th>" + userProfileEntity.Email + "</th>";

                    if (token)
                    {
                        GameEngineDTO game = GameEngineService.Instance.GetById(player.GameId, player.Email);
                        util = util + "<th>" + game.Name + "</th>";
                    }
                    else
                    {
                        util = util + "<th>" + "------" + "</th>";
                    }


                    try
                    {
                        util = util + "<th>" + AccountRepository.Instance.FindByUserName(player.Email).LastUpdate + "</th>";
                    }
                    catch (Exception ex)
                    {
                        util = util + "<th>" + "--------" + "</th>";
                    }



                    try
                    {
                        util = util + "<th>" + device.Last_Update + "</th>";
                    }
                    catch (Exception ex)
                    {
                        util = util + "<th>" + "-----" + "</th>";
                    }



                    util = util + "</tr>";
                }

                return util;
            }
            else
            {
                string util = "<table>";
                util = util + "<tr> <th>Nome</th> <th>Email</th> <th>Empresa</th> <th>Web</th> <th>Mobile</th> </tr>";
                foreach (UserProfileEntity userProfileEntity in userProfileEntitys)
                {
                    util = util + "<tr>";
                    PlayerEngineDTO player = null;
                    AccountDevicesEntity device = null;
                    bool token = true;
                    try
                    {
                        player = PlayerEngineService.Instance.GetByEmail(userProfileEntity.Email, true);
                    }
                    catch (Exception ex)
                    {
                        token = false;
                    }

                    try
                    {
                        device = AccountDevicesRepository.Instance.FindByPlayerIdDescending(player.Id).First();
                    }
                    catch (Exception ex)
                    {
                        device = null;
                    }


                    util = util + "<th>" + userProfileEntity.Name + "</th>";
                    util = util + "<th>" + userProfileEntity.Email + "</th>";

                    if (token)
                    {
                        GameEngineDTO game = GameEngineService.Instance.GetById(player.GameId, player.Email);
                        util = util + "<th>" + game.Name + "</th>";
                    }
                    else
                    {
                        util = util + "<th>" + "------" + "</th>";
                    }


                    try
                    {
                        util = util + "<th>" + AccountRepository.Instance.FindByUserName(player.Email).LastUpdate + "</th>";
                    }
                    catch (Exception ex)
                    {
                        util = util + "<th>" + "--------" + "</th>";
                    }



                    try
                    {
                        util = util + "<th>" + device.Last_Update + "</th>";
                    }
                    catch (Exception ex)
                    {
                        util = util + "<th>" + "-----" + "</th>";
                    }



                    util = util + "</tr>";
                }

                return util;
            }
            



            
        }
    }
}