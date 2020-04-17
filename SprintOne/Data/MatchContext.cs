using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SprintOne.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SprintOne.Data
{
    public class MatchContext : IdentityDbContext<User>
    {
        public MatchContext(DbContextOptions<MatchContext> options) : base(options)
        {

        }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<RaceRecord> RaceRecords { get; set; }
        public DbSet<BuddyState> BuddyList { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasOne(p => p.Profile).WithOne(u => u.LoginUser).HasForeignKey<Profile>(u => u.UserID).HasPrincipalKey<User>(p => p.UserID);
            modelBuilder.Entity<Profile>().ToTable("Profiles");
            modelBuilder.Entity<RaceRecord>().ToTable("RaceRecords");
            modelBuilder.Entity<BuddyState>().ToTable("BuddyList");
            modelBuilder.Entity<BuddyState>().HasKey(b => new { b.FirstID, b.SecondID });
            modelBuilder.Entity<BuddyState>()
                .HasOne(p => p.FirstProfile)
                .WithMany()
                .HasForeignKey(p => p.FirstID);

            modelBuilder.Entity<BuddyState>()
                .HasOne(p => p.SecondProfile)
                .WithMany()
                .HasForeignKey(p => p.SecondID)
                .OnDelete(DeleteBehavior.NoAction)
                ;

            modelBuilder.Entity<Thread>().ToTable("Threads");
            modelBuilder.Entity<Thread>()
                .HasOne(p => p.InitiatorProfile)
                .WithMany()
                .HasForeignKey(p => p.InitiatorID);

            modelBuilder.Entity<Thread>()
                .HasOne(p => p.ReceiverProfile)
                .WithMany()
                .HasForeignKey(p => p.ReceiverID)
                .OnDelete(DeleteBehavior.NoAction)
                ;

            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Conversation>().ToTable("Conversations");
            modelBuilder.Entity<Conversation>().HasKey(b => new { b.ThreadID, b.MessageID });



            //modelBuilder.Entity<PaceMatchModel>().HasNoKey();
        }

        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string password = "admin";
            string roleName = "Admin";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // if username doesn't exist, create it and add to role
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username};
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        public static async Task CreateUser(IServiceProvider serviceProvider, string a, string b)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = a;
            string password = b;
            string roleName = "User";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // if username doesn't exist, create it and add to role
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username};
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

    }
}
