using System;

namespace Repository.Entities
{
    public class UrlEntity
    {
        public Guid Id { get; set; }
        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime Date { get; set; }
    }
}
