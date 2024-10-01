using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Certification.Infrastructure.Config;

namespace Certification.Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileAttachmentConfig());
          
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // DbSets for your entities
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkshopParticipant> WorkshopParticipants { get; set; }
        public DbSet<FileAttachment> FileAttachment { get; set; }
    }
}
