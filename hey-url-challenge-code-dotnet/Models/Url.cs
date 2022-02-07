using System;
namespace hey_url_challenge_code_dotnet.Models
{
    public class Url
    {
        public Guid Id { get; set; }
        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }

        int? _count;

        public int? Count
        {
            get { return _count; }
            set { _count = value == null ? 0 : value; }
        }

        public DateTime Date { get; set; }
    }
}
