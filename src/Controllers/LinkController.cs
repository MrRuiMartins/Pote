﻿using System;
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
        private readonly IConfigurationRoot _configuration;

        public LinkController(
            IConfigurationRoot Configuration
        )
        {
            _configuration = Configuration;
        }
        // Post api/links/{url}
        [HttpPost("{url}")]
        public IActionResult Post(string url)
        {
            // log the entry
            using (var db = new InterestingLinkContext(_configuration))
            {
				db.Database.EnsureCreated();
                var link = new InterestingLink{
                    Url = url,
                    CreatedAt = DateTimeOffset.UtcNow
                };
                
                db.Links.Add(link);
                db.SaveChanges();
            }
            // report everything went fine
            return new OkResult();
        }

		[HttpGet("")]
		public IEnumerable<InterestingLink> Get()
		{
			List<InterestingLink> links;
			// log the entry
            using (var db = new InterestingLinkContext(_configuration))
            {                
                links = db.Links.Where(l => l.CreatedAt >= DateTime.MinValue).ToList();
            }

			return links;
		}
    }
}
