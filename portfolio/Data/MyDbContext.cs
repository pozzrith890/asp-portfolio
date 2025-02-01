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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the User entity to the tbl_users table
            modelBuilder.Entity<User>().ToTable("tbl_users");
            modelBuilder.Entity<Setting>().ToTable("tbl_settings");
            modelBuilder.Entity<Member>().ToTable("tbl_members");

        }
    }
}
