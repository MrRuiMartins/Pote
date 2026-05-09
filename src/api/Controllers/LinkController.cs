using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.DbModels;

namespace src.Controllers
{
    [Route("[controller]")]
    public class LinkController : Controller
    {
        private readonly InterestingLinkContext _db;

        public LinkController(InterestingLinkContext db)
        {
            _db = db;
        }
        // Post api/links/{url}
        [HttpPost("{url}")]
        public IActionResult Post(string url)
        {
            var link = new InterestingLink
            {
                Id = Guid.NewGuid().ToString(),
                Url = url,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _db.Links.Add(link);
            _db.SaveChanges();

            return new OkResult();
        }

        [HttpGet("")]
        public IEnumerable<InterestingLink> Get()
        {
            return _db.Links.Where(l => l.CreatedAt >= DateTimeOffset.MinValue).ToList();
        }
    }
}
