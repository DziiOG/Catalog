using Catalog.Contracts;
using Catalog.Helpers;

namespace Catalog.Extensions
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
