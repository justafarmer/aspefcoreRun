using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintOne.Models
{
    public class Conversation
    {
        //Thread ID the message belongs to.
        public int ThreadID { get; set; }

        //Message ID
        public int MessageID { get; set; }

        // 0 = Unread, 1 = Read
        public int ReadFlag { get; set; }

        // Read date, null if unread.
        public DateTime? DateRead { get; set; }

        public Thread Thread { get; set; }
        public Message Message { get; set; }
    }
}
