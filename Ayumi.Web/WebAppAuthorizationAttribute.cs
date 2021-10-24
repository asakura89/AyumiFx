using System;
using System.Text;
using System.Web.Mvc;
using Arvy;
using WebApp.Config;

namespace WebApp {
    public class WebAppAuthorizationAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            try {
                var accountSvc =
                    new WebAppAccountService(new DbContextFactory(MainConfig.Section.ConnectionStringName));
                if (!accountSvc.IsAuthenticated())
                    filterContext.Result = new RedirectResult("~/");
                else {
                    String activityId = GetActivityId(filterContext);
                    if (!accountSvc.IsAuthorized(activityId))
                        filterContext.Result = new RedirectResult("~/Home/NotAuthorized");
                }
            }
            catch (Exception ex) {
                filterContext.Result = new JsonResult {
                    Data = ex.AsActionResponseViewModel(),
                    ContentType = "application/json; charset=utf-8",
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
        }

        // NOTE: http://stackoverflow.com/questions/18535241/how-to-get-controller-and-action-name-in-onactionexecuting
        String GetActivityId(ActionExecutingContext filterContext) {
            String action = filterContext.ActionDescriptor.ActionName;
            String controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            return controller + action;
        }
    }
}