using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintOne.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Profile MyProfile { get; set; }

        //Only used when viewing other profiles, otherwise a null value.
        public int? BuddyFlag { get; set; }

        //Three separate lists to group buddy states.
        public List<Profile> MyListFriends { get; set; }
        public List<Profile> MyListPending { get; set; }
        public List<Profile> MyListBlocked { get; set; }
        public IEnumerable<RaceRecord> MyRaceRecords { get; set; }

        /*
        public User myProfile { get; set; }

        public RaceRecord myBestTime { get; set; }

        public ICollection<User> MyBuddyList { get; set; }
        public ICollection<RaceRecord> MyRaceRecords { get; set; }
        */
    }
}
