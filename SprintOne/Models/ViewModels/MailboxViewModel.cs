using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SprintOne.Models.ViewModels
{
    public class MailboxViewModel
    {
        
        public string ReceiverString { get; set; }

        public int ReceiverID { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Header { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(1000)]
        public string Body { get; set; }

        [Range(1, 10000, ErrorMessage = "Recipient is required.")]
        public SelectList ReceiverList { get; set; }

        public IEnumerable<Thread> Threads { get; set; }
        public IEnumerable<Conversation> Conversations { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Profile> Profiles { get; set; }

    }
}
