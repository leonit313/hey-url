using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUrlService
    {
        Task<IEnumerable<UrlEntity>> GetAll();
        Task<UrlEntity> Add(string url);
        Task<UrlEntity> GetByUrl(string url);
    }
}
