using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintOne.Models
{
    public class BuddyState
    {

        //First User ID
        public int FirstID { get; set; }
        

        //Second User ID.
        public int SecondID { get; set; }
        

        // 1 = Matched, 2 = Requested, 3 = Blocked
        public int Status { get; set; }

        public Profile FirstProfile { get; set; }
        public Profile SecondProfile { get; set; }

    }
}
