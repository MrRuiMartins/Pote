using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace src.DbModels
{
    public class InterestingLinkContext : DbContext
    {
		private readonly IConfigurationRoot _configuration;

        public InterestingLinkContext(IConfigurationRoot configuration)
		{
			_configuration = configuration;
		}
        public DbSet<InterestingLink> Links { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_configuration.GetConnectionString("PoteDatabase"));
        }
    }
}