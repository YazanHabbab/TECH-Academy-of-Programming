using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH_Academy_of_Programming.Constants
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly IReadOnlyList<string> All = new[] { Admin, User };
    }
}