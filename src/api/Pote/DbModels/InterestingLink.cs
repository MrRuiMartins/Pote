using System;

namespace Pote.DbModels
{
    public class InterestingLink
    {
        public string Id { get; set; } = default!;
        public string Url { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
