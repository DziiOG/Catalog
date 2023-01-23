using System.Net;
using Catalog.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Catalog.Extensions;

namespace Catalog.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessRestriction : Attribute, IActionFilter
    {
        private readonly List<string> roles = new List<string>();

        public AccessRestriction(string[] accessRoles)
        {
            this.roles = accessRoles.ToList();
        }

        private bool Contains(List<string> list1, List<string> list2)
        {
            bool result = false;
            if (list1.Count != 0 && list2.Count != 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    string currentValue = list1[i];
                    if (list2.Contains(currentValue))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            UserInfo? userInfo = (UserInfo?)context.HttpContext.Items["LoggedInUser"];
            Bot? bot = (Bot?)context.HttpContext.Items["BotAccess"];
            if (userInfo != null)
            {
                bool hasAccess = userInfo.hasAccessToResource(roles);
                if (!hasAccess)
                {
                    context.ModelState.AddModelError("Forbidden", "Unauthorized access");
                    context.Result = new StatusCodeResult(403);
                }
            }
            else if (bot != null)
            {
                return;
            }
            else
            {
                context.ModelState.AddModelError("Forbidden", "Unauthorized access");
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
