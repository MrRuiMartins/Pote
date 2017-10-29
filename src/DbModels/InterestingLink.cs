using System;

namespace src.DbModels
{
    public class InterestingLink
    {
		public string Id {get; set;}
        public string Url { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
