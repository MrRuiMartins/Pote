using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using src.DbModels;

namespace src.Controllers
{
    [Route("[controller]")]
    public class LinkController : Controller
    {
		private readonly InterestingLinkContext db;

		public LinkController(
			InterestingLinkContext dbContext
        )
        {
			this.db = dbContext;
		}
        // Post api/links/{url}
        [HttpPost("{url}")]
        public IActionResult Post(string url)
        {
            // log the entry
			db.Database.EnsureCreated();
			var link = new InterestingLink{
				Url = url,
				CreatedAt = DateTimeOffset.UtcNow
			};
			
			db.Links.Add(link);
			db.SaveChanges();
            // report everything went fine
            return new OkResult();
        }

		[HttpGet("")]
		public IEnumerable<InterestingLink> Get()
		{
			// log the entry
			return db.GetAllLinks();
		}
    }
}
