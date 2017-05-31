//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Http.Filters;
//using System.Web.Mvc;
//using Vlast.Gamific.Web.Controllers;
//using Vlast.Gamific.Web.Controllers.Public;
//using Vlast.Gamific.Web.Controllers.Util;

//namespace Vlast.Gamific.Web.App_Start
//{
//    public class MessagesActionFilter : System.Web.Mvc.ActionFilterAttribute
//    {
//        // This method is called BEFORE the action method is executed
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            // Check for incoming Toastr objects, in case we've been redirected here
//            MessageControllerBase controller = filterContext.Controller as MessageControllerBase;
//            if (controller != null)
//                controller.Toastr = (controller.TempData["Toastr"] as Toastr)
//                                     ?? new Toastr();

//            base.OnActionExecuting(filterContext);
//        }

//        // This method is called AFTER the action method is executed but BEFORE the
//        // result is processed (in the view or in the redirection)
//        public override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            MessageControllerBase controller = filterContext.Controller as MessageControllerBase;
//            if (filterContext.Result.GetType() == typeof(ViewResult))
//            {
//                if (controller.Toastr != null && controller.Toastr.ToastMessages.Count() > 0)
//                {
//                    // We're going to a view so we store Toastr in the ViewData collection
//                    controller.ViewData["Toastr"] = controller.Toastr;
//                }
//            }
//            else if (filterContext.Result.GetType() == typeof(RedirectToRouteResult))
//            {
//                if (controller.Toastr != null && controller.Toastr.ToastMessages.Count() > 0)
//                {
//                    // User is being redirected to another action method so we store Toastr in
//                    // the TempData collection
//                    controller.TempData["Toastr"] = controller.Toastr;
//                }
//            }

//            base.OnActionExecuted(filterContext);
//        }
//    }
//}