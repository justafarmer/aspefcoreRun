using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SprintOne.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> AdminUsers { get; set; }
        public IEnumerable<IdentityRole> UserRoles { get; set; }
    }
}
