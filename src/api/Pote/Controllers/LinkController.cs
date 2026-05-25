using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pote.DbModels;

namespace Pote.Controllers
{
    [Route("[controller]")]
    public class LinkController : Controller
    {
        private readonly PoteDbContext _context;

        public LinkController(PoteDbContext context)
        {
            _context = context;
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

            _context.InterestingLinks.Add(link);
            _context.SaveChanges();

            return new OkResult();
        }

        [HttpGet("")]
        public IEnumerable<InterestingLink> Get()
        {
            return _context.InterestingLinks.Where(l => l.CreatedAt >= DateTimeOffset.MinValue).ToList();
        }
    }
}
