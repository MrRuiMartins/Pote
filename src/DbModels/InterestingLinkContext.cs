using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace src.DbModels
{
    public class InterestingLinkContext : DbContext
    {
		public InterestingLinkContext(DbContextOptions<InterestingLinkContext> options)
        : base(options)
    	{ }
        public DbSet<InterestingLink> Links { get; set; }
    }
}