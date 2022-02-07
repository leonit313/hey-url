using Domain.Interfaces;
using Repository.Data;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class UrlService : RepositoryBase<UrlEntity>, IUrlService
    {
        public UrlService(ApplicationContext repositoryContext) : base(repositoryContext)
        { }

        public async Task<UrlEntity> Add(string url)
        {
            var uniqueUrl = ShortUrl();

            while ((await GetAllAsync()).Any(x => x.ShortUrl == uniqueUrl))
                uniqueUrl = ShortUrl();

            return await AddAsync(new UrlEntity { Id = Guid.NewGuid(), ShortUrl = uniqueUrl, OriginalUrl = url, Date = DateTime.Now });
        }

        public async Task<IEnumerable<UrlEntity>> GetAll() => await GetAllAsync();

        public async Task<UrlEntity> GetByUrl(string url) => await FirstOrDefaultAsync(x => x.ShortUrl == url);

        private string ShortUrl()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            return new String(stringChars);
        }
    }
}
