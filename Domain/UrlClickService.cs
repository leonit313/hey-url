using Domain.Interfaces;
using Repository.Data;
using Repository.Entities;
using Repository.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public class UrlClickService : RepositoryBase<UrlClickEntity>, IUrlClickService
    {
        private readonly IUrlService _urlService;

        public UrlClickService(ApplicationContext repositoryContext, IUrlService urlService) : base(repositoryContext)
        {
            _urlService = urlService;
        }

        public async Task<UrlClickEntity> Add(string url, string os, string browser)
        {
            Enum.TryParse(os, true, out PlatformEnum platformEnum);
            Enum.TryParse(browser, true, out BrowserEnum browserEnum);
            return await AddAsync(new UrlClickEntity { Id = Guid.NewGuid(), UrlId = (await _urlService.GetByUrl(url)).Id, Browser = browserEnum, Platform = platformEnum, Date = DateTime.Now });
        }

        public async Task<IEnumerable<UrlClickEntity>> GetAll() => await GetAllAsync();
    }
}