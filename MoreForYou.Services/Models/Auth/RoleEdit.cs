using Microsoft.AspNetCore.Identity;
using MoreForYou.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Models.Auth
{
    public class RoleEdit
    {
        public AspNetRole Role { get; set; }
        public IEnumerable<UserModel> Members { get; set; }
        public IEnumerable<UserModel> NonMembers { get; set; }
    }
}
