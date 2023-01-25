using System.Collections.Generic;
using Catalog.Api.Contracts;
using Catalog.Api.Helpers;

namespace Catalog.Api.Extensions
{
    public static class UserInfoExtensions
    {
        public static bool hasAccessToResource(this UserInfo userInfo, List<string> accessRoles)
        {
            List<string> userRoles = userInfo.Roles;
            bool hasAccess = Misc.Contains(userRoles, accessRoles);
            return hasAccess;
        }
    }
}
