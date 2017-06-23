using System;
using Gamific.Scheduler;
using System.Threading;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Util.Parameter;
using Vlast.Broker.EMAIL;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using System.Collections;
using System.Globalization;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Account.Model;
using Aspose.Words.Lists;
using System.Data;
using System.IO;
using Vlast.Broker.SNS.Model;
using Aspose.Cells;
using Vlast.Gamific.Model.Account.Repository;

namespace Vlast.Gamific.Web.Jobs
{
    /// <summary>
    /// Envia, periodicamente, por email para os analizadores Gamific.
    /// </summary>
    public class InternalReportThread : BaseThread
    {

        private string email = "rafael@gamific.com.br";
        public override void Init(TimeSpan timeToRun)
        {
            InternalReportThread ReportThread = new InternalReportThread();

            ReportThread.timeToRun = timeToRun;

            Instance = new Thread(ReportThread.Start);
            Instance.Start();
        }

        public async override void Run()
        {

            string dayOfWeek = DateTime.Now.ToString("ddd");
            if (dayOfWeek == "mon") { // || dayOfWeek == "tue" || dayOfWeek == "wed") {
                MemoryStream ms = CreateXls();
               
            }


        }

        private void Send(EmailSupportDTO email, string emailTo, MemoryStream ms, string fileName )
        {
            string emailFrom = ParameterCache.Get("SUPPORT_EMAIL");
            bool result = EmailDispatcher.SendEmail(emailFrom, email.Subject, new List<string>() { emailTo }, email.Msg, ms, fileName);
        }

        private MemoryStream CreateXls()
        {

            List<UserAccountEntity> accontEntityResults = AccountRepository.Instance.GetAll();
            List<AccountDevicesEntity> accontDeviceEntitys = AccountDevicesRepository.Instance.FindAll();
            List<UserProfileEntity> userProfileEntitys = UserProfileRepository.Instance.GetAllUsers();
            string filename = "Input" + DateTime.Now.ToString("yyyyMMdd_hhss") + ".xls";
            /*var workbook = new Workbook();
            
            
            workbook.FileName = filename;

            var worksheetResults = workbook.Worksheets[0];

            int rowsCount = 40000;

            worksheetResults.Cells.HideColumns(5, 16384);
            worksheetResults.Cells.HideRows(rowsCount, 1048576);
            worksheetResults.Cells.StandardWidth = 35.0;

            worksheetResults.Name = "Contra_Relatorio";

            var cellsResults = worksheetResults.Cells;

            cellsResults["A1"].PutValue("Nome");
            cellsResults["B1"].PutValue("Email");
            cellsResults["C1"].PutValue("Empresa");
            cellsResults["D1"].PutValue("Web");
            cellsResults["E1"].PutValue("Mobile");

            var validations = worksheetResults.Validations;

            var validationName = validations[validations.Add()];
            validationName.Type = ValidationType.List;
            validationName.Operator = OperatorType.Between;
            validationName.InCellDropDown = true;
            validationName.ShowError = true;
            validationName.AlertStyle = ValidationAlertType.Stop;
            CellArea areaName;
            areaName.StartRow = 1;
            areaName.EndRow = rowsCount;
            areaName.StartColumn = 0;
            areaName.EndColumn = 0;
            validationName.AreaList.Add(areaName);

            var validationEmail = validations[validations.Add()];
            validationEmail.Type = ValidationType.TextLength;
            validationEmail.Operator = OperatorType.None;
            validationEmail.InCellDropDown = false;
            validationEmail.ShowError = true;
            validationEmail.AlertStyle = ValidationAlertType.Stop;
            CellArea areaEmails;
            areaEmails.StartRow = 1;
            areaEmails.EndRow = rowsCount;
            areaEmails.StartColumn = 1;
            areaEmails.EndColumn = 1;
            validationEmail.AreaList.Add(areaEmails);

            var validationEmpresa = validations[validations.Add()];
            validationEmpresa.Type = ValidationType.WholeNumber;
            validationEmpresa.Operator = OperatorType.Between;
            validationEmpresa.Formula1 = 0.ToString();
            validationEmpresa.Formula2 = Int32.MaxValue.ToString();
            validationEmpresa.InCellDropDown = false;
            validationEmpresa.ShowError = true;
            validationEmpresa.AlertStyle = ValidationAlertType.Stop;
            CellArea areaEmpresa;
            areaEmpresa.StartRow = 1;
            areaEmpresa.EndRow = rowsCount;
            areaEmpresa.StartColumn = 2;
            areaEmpresa.EndColumn = 2;
            validationEmpresa.AreaList.Add(areaEmpresa);

            var validationWeb = validations[validations.Add()];
            validationWeb.Type = ValidationType.Date;
            validationWeb.Operator = OperatorType.Between;
            DateTime firstDate = DateTime.MinValue;
            validationWeb.Formula1 = firstDate.AddYears(1899).ToString().Split(' ')[0];
            validationWeb.Formula2 = DateTime.Now.ToString().Split(' ')[0];
            validationWeb.InCellDropDown = false;
            validationWeb.ShowError = true;
            validationWeb.AlertStyle = ValidationAlertType.Stop;
            CellArea areaWeb;
            areaWeb.StartRow = 1;
            areaWeb.EndRow = rowsCount;
            areaWeb.StartColumn = 3;
            areaWeb.EndColumn = 3;
            validationWeb.AreaList.Add(areaWeb);

            var validationMobile = validations[validations.Add()];
            validationMobile.Type = ValidationType.Date;
            validationMobile.Operator = OperatorType.Between;
            DateTime secondDate = DateTime.MinValue;
            validationMobile.Formula1 = secondDate.AddYears(1899).ToString().Split(' ')[0];
            validationMobile.Formula2 = DateTime.Now.ToString().Split(' ')[0];
            validationMobile.InCellDropDown = false;
            validationMobile.ShowError = true;
            validationMobile.AlertStyle = ValidationAlertType.Stop;
            CellArea areaMobile;
            areaMobile.StartRow = 1;
            areaMobile.EndRow = rowsCount;
            areaMobile.StartColumn = 4;
            areaMobile.EndColumn = 4;
            validationMobile.AreaList.Add(areaMobile);
            */
            MemoryStream ms = new MemoryStream();
            //int row = 2;

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
                   //player = PlayerEngineService.Instance.GetByEmail("leo@gamific.com.br", true);
                }
                catch(Exception ex)
                {
                    token = false;
                }

                try
                {
                    device = AccountDevicesRepository.Instance.FindByPlayerIdDescending(player.Id).First();
                }
                catch(Exception ex)
                {
                    device = null;
                }


            //cellsResults["A" + row].PutValue(userProfileEntity.Name);
            //cellsResults["B" + row].PutValue(userProfileEntity.Email);
            util = util + "<th>" + userProfileEntity.Name + "</th>";
            util = util + "<th>" + userProfileEntity.Email + "</th>";
            //util = util + "<th>" + player.Nick + "</th>";
            //util = util + "<th>" + player.Email + "</th>";

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
                    //cellsResults["D" + row].PutValue(accontEntityResult.LastLogin);
                    util = util + "<th>" + AccountRepository.Instance.FindByUserName(player.Email).LastUpdate + "</th>";
                }catch(Exception ex)
                {
                    util = util + "<th>" + "--------" + "</th>";
                }



                try
                {
                    //cellsResults["E" + row].PutValue(device.Last_Update);
                    util = util + "<th>" + device.Last_Update + "</th>";
                }
                catch(Exception ex)
                {
                    //cellsResults["E" + row].PutValue("----");
                    util = util + "<th>" + "-----" + "</th>";
                }
                        


                //row++;
                util = util + "</tr>";
            }
            
            //ms = workbook.SaveToStream();

            //Send(new EmailSupportDTO { Msg = util , Category = "", Subject = "Contra-relatorio" },"m3iller@gmail.com", ms, filename);
            Send(new EmailSupportDTO { Msg = util, Category = "", Subject = "Contra-relatorio" }, "rafael@gamific.com.br", ms, filename);
            Send(new EmailSupportDTO { Msg = util, Category = "", Subject = "Contra-relatorio" }, "suporte@gamific.com.br", ms, filename);
            //Send(new EmailSupportDTO { Msg = util , Category = "", Subject = "Contra-relatorio" }, "victor@duplov.com.br", ms, filename);


            return null; //ms;
        }


    }

}