using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Certification.Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace Certification.Infrastructure.Data;
    public class Context : IdentityDbContext<User>

    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileAttachmentConfig());
          
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // DbSets for your entities
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<User> Users { get; set; }  
        public virtual DbSet<WorkshopParticipant> WorkshopParticipants { get; set; }
        public virtual DbSet<FileAttachment> FileAttachment { get; set; }
    }

