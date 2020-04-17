using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SprintOne.Models
{
    public class User : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        public Profile Profile { get; set; }

        [NotMapped]
        public IList<string> RoleNames { get; set; }
    }
}
