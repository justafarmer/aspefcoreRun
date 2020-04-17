using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintOne.Models
{
    public class Message
    {
        //Message ID
        public int MessageID { get; set; }

        //Date message sent.
        public DateTime? DateSent { get; set; }

        //ID of sender of the message.
        public int MsgSenderID { get; set; }

        //Header of the message
        public string MsgHeader { get; set; }

        //Body of the message.
        public string MsgBody { get; set; }

        public Conversation Conversation { get; set; }
        public Profile Profile { get; set; }
    }
}
