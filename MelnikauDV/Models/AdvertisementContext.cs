using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using MelnikauDV.Models.AccountModel;
using MelnikauDV;
using MelnikauDV.Models;

namespace MelnikauDV.Models
{
    public class AdvertisementContext : DbContext
    {
        public AdvertisementContext(DbContextOptions<AdvertisementContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public AdvertisementContext()
        {

        }
        public DbSet<Advertisement> Advertisements { get; set; }
        //  public DbSet<AdvMark> AdvMarks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProfitAdv> ProfitAdvs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@gmail.com";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });

            modelBuilder
                    .Entity<Advertisement>()
                    .HasMany(c => c.Users)
                    .WithMany(s => s.Advertisements)
                    .UsingEntity <AdvMark> (
                       j => j
                        .HasOne(pt => pt.User)
                        .WithMany(t => t.AdvMarks)
                        .HasForeignKey(pt => pt.UserId),
                    j => j
                        .HasOne(pt => pt.Advertisement)
                        .WithMany(p => p.AdvMarks)
                        .HasForeignKey(pt => pt.AdvertisementId),
                    j =>
                    {
                
                        j.Property(pt => pt.Mark).HasDefaultValue(3);
                        j.HasKey(t => new { t.AdvertisementId, t.UserId });
                        j.ToTable("AdvMark");
                    }
            );
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<AdvMark> AdvMark { get; set; }
        

    }
}
