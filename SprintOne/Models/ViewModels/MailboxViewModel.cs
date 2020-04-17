using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintOne.Models.ViewModels
{
    public class MailboxViewModel
    {
        public SelectList ReceiverList { get; set; }
        public string ReceiverString { get; set; }

        public string Header { get; set; }
        public string Body { get; set; }
        public int ReceiverID { get; set; }

        public IEnumerable<Thread> Threads { get; set; }
        public IEnumerable<Conversation> Conversations { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Profile> Profiles { get; set; }

    }
}
