using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SprintOne.Models
{
    public class Profile
    {
        [Key]
        public int ProfileID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Last name cannot be longer than 30 characters.")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Profile Creation Date")]
        public DateTime CreationDate { get; set; }

        public ICollection<RaceRecord> TimeEntries { get; set; }
        public ICollection<BuddyState> BuddyList { get; set; }
        public ICollection<Thread> MessageThread { get; set; }

        public int UserID { get; set; }
        public User LoginUser { get; set; }
    }
}
