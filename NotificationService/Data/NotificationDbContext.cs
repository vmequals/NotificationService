using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NotificationService.Models;

namespace NotificationService.Data
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=postgresdb;Username=myusername;Password=mypassword;Port=5432");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeTeam>()
                .HasKey(te => new { te.TeamId, te.EmployeeId });

            modelBuilder.Entity<EmployeeTeam>()
                .HasOne(te => te.Team)
                .WithMany(t => t.EmployeeTeams)
                .HasForeignKey(te => te.TeamId);

            modelBuilder.Entity<EmployeeTeam>()
                .HasOne(te => te.Employee)
                .WithMany(e => e.EmployeeTeams)
                .HasForeignKey(te => te.EmployeeId);


            modelBuilder.Entity<SubjectTeam>()
                .HasKey(st => new { st.SubjectId, st.TeamId });

            modelBuilder.Entity<SubjectTeam>()
                .HasOne(st => st.Subject)
                .WithMany(s => s.SubjectTeams)
                .HasForeignKey(st => st.SubjectId);

            modelBuilder.Entity<SubjectTeam>()
                .HasOne(st => st.Team)
                .WithMany(t => t.SubjectTeams)
                .HasForeignKey(st => st.TeamId);

        }
    }


}