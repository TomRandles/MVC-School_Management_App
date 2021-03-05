using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServicesLib.Domain.Models;
using ServicesLib.Domain.ViewModel;
using System;
using System.Linq;

namespace ServicesLib.Services.Database
{
    public class SchoolDbContext : IdentityDbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Remove Foreign Key Restriction
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Extend models to include a LastUpdated date shadow property
            modelBuilder.Entity<Student>()
                .Property<DateTime>("LastUpdated");
            modelBuilder.Entity<Programme>()
                .Property<DateTime>("LastUpdated");
            modelBuilder.Entity<Module>()
                .Property<DateTime>("LastUpdated");
            modelBuilder.Entity<Assessment>()
                .Property<DateTime>("LastUpdated");
            modelBuilder.Entity<AssessmentResult>()
                .Property<DateTime>("LastUpdated");


            //Avoiding Cycles

            //modelBuilder.Entity<Assessment>()
            //    .HasOne<Module>(sc => sc.Module)
            //    .WithMany(s => s.Assessments)
            //    .HasForeignKey(sc => sc.ModuleID);


            // modelBuilder.Entity<AssessmentResult>()   
            //.HasOne(u => u.Module).WithMany(u => u.AssessmentResults).IsRequired().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssessmentResult>()
                .HasOne<Module>(sc => sc.Module)
                .WithMany(s => s.AssessmentResults)
                .HasForeignKey(sc => sc.ModuleID);

            modelBuilder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Student", NormalizedName = "STUDENT" },
                new { Id = "2", Name = "Teacher", NormalizedName = "TEACHER" },
                new { Id = "3", Name = "Admin", NormalizedName = "ADMIN" }
            );
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Programme> Programmes { get; set; }

        public DbSet<Module> Modules { get; set; }

        // public DbSet<ProgrammeModule> ProgrammeModules { get; set; }

        public DbSet<Assessment> Assessments { get; set; }

        public DbSet<AssessmentResult> AssessmentResults { get; set; }

        public DbSet<LoginVm> LoginVm { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
    }
}
