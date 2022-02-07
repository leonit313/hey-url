using Repository.Enum;
using System;

namespace Repository.Entities
{
    public class UrlClickEntity
    {
        public Guid Id { get; set; }
        public Guid UrlId { get; set; }
        public DateTime Date { get; set; }
        public PlatformEnum Platform { get; set; }
        public BrowserEnum Browser { get; set; }

        public virtual UrlEntity Url { get; set; }
    }
}
