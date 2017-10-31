using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace src.DbModels
{
    public class InterestingLinkContext : DbContext
    {
		public InterestingLinkContext()
		{
			// for mocking purposes
		}
		public InterestingLinkContext(DbContextOptions<InterestingLinkContext> options)
        : base(options)
    	{ }
        public DbSet<InterestingLink> Links { get; set; }

		public virtual List<InterestingLink> GetAllLinks()
		{
			return Links.ToList();
		}
    }
}