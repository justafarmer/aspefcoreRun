using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SprintOne.Models;


namespace SprintOne.Data
{
    public static class SeedData
    {

        public static void Initialize(MatchContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users.
          
            //If Any users, only create users for first run (seed AspNetUusers)
            if (context.Users.Any())
            {
                //If users exists, check if profiles also exist, if they do then data is already seeded.
                if (context.Profiles.Any())
                {
                    return;
                }
            
            var users = new Profile[]
            {
               // UserID = 1
               new Profile { FirstName = "John", LastName = "Smith", CreationDate = DateTime.Parse("2019-09-24"), UserID = 1 },
               new Profile { FirstName = "Susan", LastName = "Meyers", CreationDate = DateTime.Parse("2019-05-07"), UserID = 2 },
               new Profile { FirstName = "Michael", LastName = "Boylan", CreationDate = DateTime.Parse("2019-06-17"), UserID = 3 },
               new Profile { FirstName = "Jeff", LastName = "Mililer", CreationDate = DateTime.Parse("2020-01-12"), UserID = 4 },
               new Profile { FirstName = "Karen", LastName = "Filippelli", CreationDate = DateTime.Parse("2018-03-23"), UserID = 5 },
               // UserID = 5
               new Profile { FirstName = "Jim", LastName = "Halpert", CreationDate = DateTime.Parse("2019-07-05"), UserID = 6 },
               new Profile { FirstName = "Dwight", LastName = "Schrute", CreationDate = DateTime.Parse("2020-03-09"), UserID = 7 },
               new Profile { FirstName = "Pam", LastName = "Beesly", CreationDate = DateTime.Parse("2020-02-15"), UserID = 8 },
               new Profile { FirstName = "Kelly", LastName = "Kapoor", CreationDate = DateTime.Parse("2019-04-09"), UserID = 9 },
               new Profile { FirstName = "Ryan", LastName = "Howard", CreationDate = DateTime.Parse("2019-11-25"), UserID = 10 }
            };

            foreach (Profile u in users)
            {
                //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Profiles ON");
                context.Profiles.Add(u);
                //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Profiles OFF");
            }
            context.SaveChanges();

            
            var racerecords = new RaceRecord[]
            {

               //RaceRecordID = 1
               new RaceRecord { ProfileID = 1, RaceType = 1, RaceTime = 450, MileTime = ToMile(450,1) },
               new RaceRecord { ProfileID = 2, RaceType = 2, RaceTime = 1300, MileTime = ToMile(1300,2) },
               new RaceRecord { ProfileID = 3, RaceType = 4, RaceTime = 5875, MileTime = ToMile(5875, 4) },
               new RaceRecord { ProfileID = 4, RaceType = 3, RaceTime = 3100, MileTime = ToMile(3100, 3) },
               new RaceRecord { ProfileID = 5, RaceType = 2, RaceTime = 1400, MileTime = ToMile(1400, 2) },               //RaceRecordID = 5               new RaceRecord { ProfileID = 6, RaceType = 5, RaceTime = 12500, MileTime = ToMile(12500, 5)},
               new RaceRecord { ProfileID = 7, RaceType = 5, RaceTime = 11775, MileTime = ToMile(11775, 5)},
               new RaceRecord { ProfileID = 8, RaceType = 4, RaceTime = 5550, MileTime =  ToMile(5550, 4)},
               new RaceRecord { ProfileID = 9, RaceType = 3, RaceTime = 2735, MileTime =  ToMile(2735, 3)},
               new RaceRecord { ProfileID = 10, RaceType = 2, RaceTime = 1500, MileTime = ToMile(1500, 2)},
               //RaceRecordID = 10               new RaceRecord { ProfileID = 2, RaceType = 5, RaceTime = 12750, MileTime = ToMile(12750, 5)},
               new RaceRecord { ProfileID = 3, RaceType = 5, RaceTime = 11875, MileTime = ToMile(11875, 5)},
               new RaceRecord { ProfileID = 7, RaceType = 4, RaceTime = 5600, MileTime = ToMile(5600, 4)},
               new RaceRecord { ProfileID = 1, RaceType = 3, RaceTime = 2850, MileTime = ToMile(2850, 3)},
               new RaceRecord { ProfileID = 5, RaceType = 2, RaceTime = 1600, MileTime = ToMile(1600, 2)},
               //RaceRecordID = 15               new RaceRecord { ProfileID = 2, RaceType = 5, RaceTime = 13000, MileTime = ToMile(13000, 5)},
               new RaceRecord { ProfileID = 6, RaceType = 5, RaceTime = 12300, MileTime = ToMile(12300, 5)},
               new RaceRecord { ProfileID = 4, RaceType = 4, RaceTime = 5300, MileTime = ToMile(5300, 4)},
               new RaceRecord { ProfileID = 8, RaceType = 3, RaceTime = 2800, MileTime = ToMile(2800, 3)},
               new RaceRecord { ProfileID = 3, RaceType = 2, RaceTime = 1650, MileTime = ToMile(1650, 2)}
            };
            foreach (RaceRecord r in racerecords)
            {
                context.RaceRecords.Add(r);
            }
            context.SaveChanges();


            var buddies = new BuddyState[]
            {
               // 1 = Matched, 2 = Requested, 3 = Blocked
               new BuddyState { FirstID = 1, SecondID = 8, Status = 1 },
               new BuddyState { FirstID = 1, SecondID = 4, Status = 3 },
               new BuddyState { FirstID = 2, SecondID = 5, Status = 2 },
               new BuddyState { FirstID = 3, SecondID = 10, Status = 1 },
               new BuddyState { FirstID = 3, SecondID = 5, Status = 2 }
            };
            foreach (BuddyState b in buddies)
            {
                context.BuddyList.Add(b);
            }
            context.SaveChanges();
            

            var threads = new Thread[]
            {           
               new Thread { InitiatorID = 8, ReceiverID = 1 },
               new Thread { InitiatorID = 5, ReceiverID = 3 },
               new Thread { InitiatorID = 3, ReceiverID = 10 },
               new Thread { InitiatorID = 7, ReceiverID = 4 },
               new Thread { InitiatorID = 2, ReceiverID = 9 }

            };
            foreach (Thread t in threads)
            {
                context.Threads.Add(t);
            }
            context.SaveChanges();


            var messages = new Message[]
            {
               new Message
               {
                   DateSent = DateTime.Parse("2019-10-26"),
                   MsgSenderID = 2,
                   MsgHeader = "Hi",
                   MsgBody = "Hey, I saw you atteneded the same race in the same area the other day.  You live around there?  Train regularly?"
               },
               new Message
               {
                   DateSent = DateTime.Parse("2019-10-23"),
                   MsgSenderID = 7,
                   MsgHeader = "All in",
                   MsgBody = "So what do I do here?  I'm not looking to put anything less than 100% into my training."
               },
               new Message
               {
                   DateSent = DateTime.Parse("2019-10-17"),
                   MsgSenderID = 3,
                   MsgHeader = "Short Time",
                   MsgBody = "Yes, absolutely, we meet at 6 AM over in this area Mondays, Wednesdays and Saturdays if that works."
               },
               new Message
               {
                   DateSent = DateTime.Parse("2019-10-13"),
                   MsgSenderID = 10,
                   MsgHeader = "Short Time",
                   MsgBody = "Hi, I'm in the area for work for a month or two and will be in the same area.  Would you like to pair up for some runs?"
               },
               new Message
               {
                   DateSent = DateTime.Parse("2019-10-11"),
                   MsgSenderID = 5,
                   MsgHeader = "Really?!",
                   MsgBody = "We met at the race the other day and I saw your finish time, it was amazing.  How often do you train?"
               },
               new Message
               {
                   DateSent = DateTime.Parse("2019-10-04"),
                   MsgSenderID = 8,
                   MsgHeader = "Hello",
                   MsgBody = "Awesome!  I've been trying for a while to find someone, when's a good time to meet up?"
               },
               new Message
               {
                   DateSent = DateTime.Parse("2019-09-27"),
                   MsgSenderID = 1,
                   MsgHeader = "Hello",
                   MsgBody = "Yes, I'm already running with 2 others but we would love to have more.  We usually go in the morning but lately we've been having trouble synching up.  If you're up for it, we're trying in the early evening right after we all get off from work."
               },
               new Message
               {
                   DateSent = DateTime.Parse("2019-09-24"),
                   MsgSenderID = 8,
                   MsgHeader = "Hello",
                   MsgBody = "Hey, I see you're in the same area.  Are you already running with a group or would you like to start one?"
               },

            };
                foreach (Message m in messages)
                {
                    context.Messages.Add(m);
                }
                context.SaveChanges();



            var conversations = new Conversation[]
            {
               new Conversation { ThreadID = 1, MessageID = 1, ReadFlag = 1, DateRead = null },
               new Conversation { ThreadID = 1, MessageID = 2, ReadFlag = 1, DateRead = null },
               new Conversation { ThreadID = 1, MessageID = 3, ReadFlag = 1, DateRead = null },
               new Conversation { ThreadID = 2, MessageID = 4, ReadFlag = 0, DateRead = null },
               new Conversation { ThreadID = 3, MessageID = 5, ReadFlag = 1, DateRead = null },
               new Conversation { ThreadID = 3, MessageID = 6, ReadFlag = 0, DateRead = null },
               new Conversation { ThreadID = 4, MessageID = 7, ReadFlag = 0, DateRead = null },
               new Conversation { ThreadID = 5, MessageID = 8, ReadFlag = 0, DateRead = null }

            };
            foreach (Conversation c in conversations)
            {
                context.Conversations.Add(c);
            }
            context.SaveChanges();



            }   // DB has been seeded
            return;   
        }

        // 1 mile = / 60
        // 5k = / 60 / 3.106
        // 10k = / 60 / 6.21
        // Half = / 60 / 13.11
        // Full = / 60 / 26.22
        public static int ToMile(int time, int type)
        {
            int mileTime;

            if (type == 1)
            {
                mileTime = Convert.ToInt32(time);
            }
            else if (type == 2)
            {
                mileTime = Convert.ToInt32(time / 3.106);
            }
            else if (type == 3)
            {
                mileTime = Convert.ToInt32(time / 6.21);
            }
            else if (type == 4)
            {
                mileTime = Convert.ToInt32(time / 13.11);
            }
            else if (type == 5)
            {
                mileTime = Convert.ToInt32(time / 26.22);
            }
            else
            {
                mileTime = 0;
            }
            return mileTime;
        }
    }
}
