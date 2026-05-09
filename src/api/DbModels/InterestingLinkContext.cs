using Microsoft.EntityFrameworkCore;


namespace src.DbModels
{
    public class InterestingLinkContext : DbContext
    {
        public InterestingLinkContext(DbContextOptions<InterestingLinkContext> options)
            : base(options)
        {
        }

        public DbSet<InterestingLink> Links { get; set; }
    }
}