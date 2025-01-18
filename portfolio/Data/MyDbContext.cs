using Microsoft.EntityFrameworkCore;

namespace portfolio.Data
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) 
        { 
            
        }
    }
}
