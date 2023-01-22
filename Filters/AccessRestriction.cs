using System.Net;
using Catalog.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            UserInfo? userInfo = (UserInfo?)context.HttpContext.Items["LoggedInUser"];
            if (userInfo != null)
            {
                List<string> userRoles = userInfo.Roles;
                bool hasAccess = Contains(userRoles, roles);
                if (!hasAccess)
                {
                    context.ModelState.AddModelError("Forbidden", "Unauthorized access");
                    context.Result = new StatusCodeResult(403);
                }
            }
        }
    }
}
