using Microsoft.EntityFrameworkCore;
using portfolio.Models;

namespace portfolio.Data
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) 
        { 
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the User entity to the tbl_users table
            modelBuilder.Entity<User>().ToTable("tbl_users");
            modelBuilder.Entity<Setting>().ToTable("tbl_settings");
            modelBuilder.Entity<Member>().ToTable("tbl_members");
            modelBuilder.Entity<Service>().ToTable("tbl_services");
            modelBuilder.Entity<Skill>().ToTable("tbl_skills");
            modelBuilder.Entity<Project>().ToTable("tbl_projects");
            modelBuilder.Entity<Comment>().ToTable("tbl_comments");

        }
        public DbSet<portfolio.Models.Skill> Skill { get; set; } = default!;
        public DbSet<portfolio.Models.Project> Project { get; set; } = default!;
        public DbSet<portfolio.Models.Comment> Comment { get; set; } = default!;
    }
}
