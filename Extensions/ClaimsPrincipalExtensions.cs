using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;

namespace System.Security.Claims
{
    public static class KnownRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public static readonly string[] AllRoles = { Admin, User };
        public const string AllRolesList = "Admin,User";
        public static readonly string[] BlogAuthorRoles = { Admin, User };
        public const string BlogAuthorRolesList = "Admin,User";
        public static readonly string[] CalendarCoordinatorRoles = { Admin };
        public const string CalendarCoordinatorRolesList = "Admin";
        public const string PolicyIsBlogAuthor = "IsBlogAuthor";
        public const string PolicyIsCalendarCoordinator = "IsCalendarCoordinator";
    }
    public static class ClaimsPrinicipalExtensions
    {
        public static bool IsInAnyRole(this ClaimsPrincipal user, params string[] roles)
        {
            if (null == roles)
            {
                return IsInAnyRole(user);
            }
            else
            {
                return roles.Any(user.IsInRole);
            }
        }
        public static bool IsInAnyRole(this ClaimsPrincipal user)
        {
            return KnownRoles.AllRoles.Any(user.IsInRole);
        }
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(KnownRoles.Admin);
        }
    }
}
