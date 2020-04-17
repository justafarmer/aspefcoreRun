using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SprintOne.Models.ViewModels
{
    public class PaceMatchViewModel
    {
        public int RunnerID { get; set; }
        public int MileTime { get; set; }

        public int Lower { get; set; }
        public int Upper { get; set; }

        /*
        public IEnumerable<RaceRecord> RaceRecords { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<BuddyState> Buddies { get; set; }
        */
    }
}
